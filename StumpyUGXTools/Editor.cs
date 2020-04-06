using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Program;

namespace StumpyUGXTools
{
    class Editor {

        private OpenTK.GLControl viewport;
        Timer viewportUpdateTimer;
        OGLCamera camera = new OGLCamera();
        ViewportGrid grid = new ViewportGrid();

        Model ugxModel;
        List<Model> imports = new List<Model>();
        List<TreeNode> rootNodes = new List<TreeNode>();

        //temp
        Model m;
        OGLMesh mes;

        public void FBXImportPrompt()
        {
            if (gui.fbd.ShowDialog() == DialogResult.OK)
            {
                if (!gui.fbd.FileName.Contains(".fbx"))
                {
                    gui.LogOut("File selected was not an fbx.");
                    return;
                }

                gui.materialEditorTab.Unload();

                Model m = gui.ImportAsset(gui.fbd.FileName);
                TreeNode root = new TreeNode(Path.GetFileNameWithoutExtension(gui.fbd.FileName));
                foreach (Model.Mesh mesh in m.meshes) { root.Nodes.Add(mesh.name); }
                gui.meshEditorTab.fbxMeshTree.Nodes.Add(root);
                root.Expand();
                imports.Add(m);
            }
        }

        public int globalShader;
        public void InitViewport()
        {
            ugxModel = ugx.GetModel();

            viewport = new OpenTK.GLControl();
            viewport.Location = new Point((gui.EditorToolsLeftMargin * 2) + gui.EditorToolSideWindowWidth, 44);
            viewport.Size = new Size(gui.Width - 50 - gui.EditorToolSideWindowWidth, gui.Height - 95);
            viewport.KeyDown += new KeyEventHandler(viewport_KeyDown);
            viewport.KeyUp += new KeyEventHandler(viewport_KeyUp);
            viewport.LostFocus += new EventHandler((object o, EventArgs e) => 
            {
                viewportUpdateTimer.Stop();
                foreach (InputKey k in keys.Values)
                {
                    k.isPressed = false;
                }
            });
            viewport.GotFocus  += new EventHandler((object o, EventArgs e) => 
            {
                viewportUpdateTimer.Start();
            });
            viewport.Paint += new PaintEventHandler((object o, PaintEventArgs e) => { viewport_Loop(); });
            viewport.MakeCurrent();

            //temp
            m = gui.ImportAsset("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\backpack.fbx");
            mes = new OGLMesh(m.meshes[0]);
            //

            InitOGL();
            InitViewportInput();
            grid.InitGrid();

            gui.Controls.Add(viewport);

            viewportUpdateTimer = new Timer();
            viewportUpdateTimer.Tick += new EventHandler((object o, EventArgs e) => { PollInputs(); });
            viewportUpdateTimer.Interval = 10;
        }
        public void InitOGL()
        {
            #region Shader Strings
            string vs =
    @"#version 330 core
layout (location = 0) in vec3 aPosition;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main()
{
    gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(aPosition, 1.0);
}";
            string fs =
    @"#version 330 core
out vec4 FragColor;
uniform vec4 color;

void main()
{
    FragColor = color;
}";
            #endregion
            
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            
            int vertS, fragS;
            vertS = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertS, vs);
            fragS = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragS, fs);
            GL.CompileShader(vertS);
            string infoLogVert = GL.GetShaderInfoLog(vertS);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);
            GL.CompileShader(fragS);
            string infoLogFrag = GL.GetShaderInfoLog(fragS);
            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);
            globalShader = GL.CreateProgram();
            GL.AttachShader(globalShader, vertS);
            GL.AttachShader(globalShader, fragS);
            GL.LinkProgram(globalShader);
            GL.UseProgram(globalShader);

            int loc = GL.GetUniformLocation(globalShader, "projectionMatrix");
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(1.523f, gui.Width / gui.Height, .01f, 250f);
            GL.UniformMatrix4(loc, false, ref proj);
            Console.WriteLine(loc);
        }

        //viewport functions
        public void viewport_Loop()
        {
            GL.ClearColor(.3f, .3f, .3f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //draw
            camera.UpdateCamera();
            grid.DrawGrid();
            mes.Draw();
            //

            viewport.SwapBuffers();
        }

        //input
        void PollInputs()
        {
            if (keys[KeyName.up].isPressed) { camera.Rotate(0, .025f); }
            if (keys[KeyName.down].isPressed) { camera.Rotate(0, -.025f); }
            if (keys[KeyName.left].isPressed) { camera.Rotate(-.025f, 0); }
            if (keys[KeyName.right].isPressed) { camera.Rotate(.025f, 0); }
            if (keys[KeyName.zoomIn].isPressed) { camera.AddRadius(-.25f); }
            if (keys[KeyName.zoomOut].isPressed) { camera.AddRadius(.25f); }
                foreach (InputKey k in keys.Values)
            {
                if (k.isPressed)
                {
                    viewport.Invalidate();
                    break;
                }
            }
        }

        enum KeyName { forward,backward,left,right,up,down,zoomIn,zoomOut }
        Dictionary<KeyName, InputKey> keys;
        void InitViewportInput()
        {
            keys = new Dictionary<KeyName, InputKey>
            {
                {KeyName.up,   new InputKey(Keys.W) },
                {KeyName.down,  new InputKey(Keys.S) },
                {KeyName.left,      new InputKey(Keys.A) },
                {KeyName.right,     new InputKey(Keys.D) },
                {KeyName.zoomIn,     new InputKey(Keys.E) },
                {KeyName.zoomOut,     new InputKey(Keys.Q) }
            };
        }
        void viewport_KeyDown(object o, KeyEventArgs e)
        {
            foreach(InputKey k in keys.Values)
            {
                k.PollKeyDown(e);
            }
        }
        void viewport_KeyUp(object o, KeyEventArgs e)
        {
            foreach (InputKey k in keys.Values)
            {
                k.PollKeyUp(e);
            }
        }
    }

    #region 3D Editor Classes
    class ViewportObject
    {
        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 scale = Vector3.One;
        public void Rotate(float yaw, float pitch, float roll)
        {
            rotation *= Quaternion.FromEulerAngles(pitch, yaw, roll);
        }
        public void Translate(float x, float y, float z)
        {
            position += new Vector3(x, y, z);
        }
    }
    class OGLCamera
    {
        public OGLCamera()
        {
            AddRadius(10f);
        }

        Vector3 position = new Vector3(0,0,-10);
        Vector3 target = new Vector3(0,0,0);
        float radius = 0;
        public void UpdateCamera()
        {
            Matrix4 m = Matrix4.LookAt(position, target, new Vector3(0, 1, 0)) * new Matrix4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            int loc = GL.GetUniformLocation(gui._3dEditor.globalShader, "viewMatrix");
            GL.UniformMatrix4(loc, false, ref m);
        }

        public void Rotate(float yaw, float pitch)
        {
            Vector3 worldUp = new Vector3(0, 1, 0);

            Vector3 cameraDirection = Vector3.Normalize(position - target);
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(worldUp, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

            Vector4 cameraFocusVector = new Vector4(position - target, 0);


            Console.WriteLine(cameraDirection);
            Console.WriteLine(pitch);
            if (cameraDirection.Y > .99f && pitch > 0) pitch = 0;
            if (cameraDirection.Y < -.99f && pitch < 0) pitch = 0;
            Matrix4 pitchMat = Matrix4.CreateFromAxisAngle(cameraRight, -pitch);

            float cameraRadiusXZ = (Vector3.Distance(new Vector3(position.X, 0, position.Z), new Vector3(target.X, 0, target.Y)));
            yaw *= (cameraRadiusXZ / radius);
            Matrix4 yawMat = Matrix4.CreateFromAxisAngle(cameraUp, yaw);

            cameraFocusVector *= yawMat;
            cameraFocusVector *= pitchMat;

            cameraFocusVector.X += target.X;
            cameraFocusVector.Y += target.Y;
            cameraFocusVector.Z += target.Z;

            position.X = cameraFocusVector.X;
            position.Y = cameraFocusVector.Y;
            position.Z = cameraFocusVector.Z;
        }
        public void AddRadius(float f)
        {
            float v = Vector3.Distance(position, target);
            if (v < .5f && f < 0) return;

            radius += f;
            position = Vector3.Normalize(position - target) * radius;
            
            Console.WriteLine(radius);
            Console.WriteLine(position);
        }


    }
    class OGLMesh : ViewportObject
    {

        struct GLVertex
        {
            public float x, y, z, u, v;
        }
        public int vbuffer, ibuffer, indexCount;
        public OGLMesh(Model.Mesh mesh)
        {
            vbuffer = GL.GenBuffer();
            ibuffer = GL.GenBuffer();

            List<GLVertex> glVertices = new List<GLVertex>();
            List<uint> glIndices = new List<uint>();
            foreach (Model.Mesh.Vertex v in mesh.vertices)
            {
                GLVertex glv = new GLVertex();
                glv.x = v.vx;
                glv.y = v.vy;
                glv.z = v.vz;
                glv.u = v.tu;
                glv.v = v.tv;
                glVertices.Add(glv);
            }
            foreach (int i in mesh.indices)
            {
                glIndices.Add((uint)i);
            }

            //GLVertex v1 = new GLVertex();
            //GLVertex v2 = new GLVertex();
            //GLVertex v3 = new GLVertex();
            //GLVertex v4 = new GLVertex();
            //v1.x = -1; v1.y = -1; v1.z = 10;
            //v2.x = 1; v2.y = -1; v2.z = 10;
            //v3.x = -1; v3.y = 1; v3.z = 10;
            //v4.x = 1; v4.y = 1; v4.z = 10;
            //glVertices.Add(v1);
            //glVertices.Add(v2);
            //glVertices.Add(v3);
            //glVertices.Add(v4);
            //glIndices.Add(0);
            //glIndices.Add(1);
            //glIndices.Add(2);
            //glIndices.Add(1);
            //glIndices.Add(2);
            //glIndices.Add(3);

            indexCount = glIndices.Count;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (sizeof(float) * 5) * glVertices.Count, glVertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * mesh.indices.Count, glIndices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public void Draw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibuffer);
            GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
    class ViewportGrid
    {
        int xLine;
        int zLine;
        int ib;
        public void InitGrid()
        {
            xLine = GL.GenBuffer();
            zLine = GL.GenBuffer();
            ib = GL.GenBuffer();

            Vector3[] xData = { new Vector3(10, 0, 0), new Vector3(-10, 0, 0) };
            Vector3[] zData = { new Vector3(0, 0, 10), new Vector3(0, 0, -10) };
            int[] indices = { 0, 1 };

            GL.BindBuffer(BufferTarget.ArrayBuffer, xLine);
            GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 2, xData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, zLine);
            GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 2, zData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * 2, indices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            cLoc = GL.GetUniformLocation(gui._3dEditor.globalShader, "color");
            mLoc = GL.GetUniformLocation(gui._3dEditor.globalShader, "modelMatrix");
        }
        int cLoc;
        int mLoc;
        public void DrawGrid()
        {
            Matrix4 m;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);

            #region X-Axis Lines
            GL.BindBuffer(BufferTarget.ArrayBuffer, xLine);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
            GL.EnableVertexAttribArray(0);

            GL.Uniform4(cLoc, new Vector4(0.25f, 0.25f, .25f, 1f));
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 1));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 2));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 3));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 4));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 5));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 6));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 7));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 8));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 9));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 10));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);

            GL.Uniform4(cLoc, new Vector4(0.25f, 0.25f, .25f, 1f));
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -1));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -2));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0,- 3));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0,- 4));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -5));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -6));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -7));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -8));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -9));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(0, 0, -10));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            #endregion
            
            #region Y-Axis Lines
            GL.BindBuffer(BufferTarget.ArrayBuffer, zLine);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
            GL.EnableVertexAttribArray(0);

            GL.Uniform4(cLoc, new Vector4(0.25f, 0.25f, .25f, 1f));
            m = Matrix4.CreateTranslation(new Vector3(1, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(2, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(3, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(4, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(5, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(6, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(7, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(8, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(9, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(10, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);

            GL.Uniform4(cLoc, new Vector4(0.25f, 0.25f, .25f, 1f));
            m = Matrix4.CreateTranslation(new Vector3(-1, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-2, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-3, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-4, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-5, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-6, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-7, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-8, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-9, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            m = Matrix4.CreateTranslation(new Vector3(-10, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
            #endregion

            GL.Uniform4(cLoc, new Vector4(0.4f, 0.4f, 1.0f, 1.0f));
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.BindBuffer(BufferTarget.ArrayBuffer, xLine);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
            GL.EnableVertexAttribArray(0);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);

            GL.Uniform4(cLoc, new Vector4(1.0f, 0.4f, 0.4f, 1.0f));
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.BindBuffer(BufferTarget.ArrayBuffer, zLine);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
            GL.EnableVertexAttribArray(0);
            GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);

            //reset uniforms
            m = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            GL.UniformMatrix4(mLoc, false, ref m);
            GL.Uniform4(cLoc, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
        }
    }

    class InputKey
    {
        Keys k;
        public bool isPressed;
        public InputKey(Keys key)
        {
            k = key;
        }
        public void PollKeyDown(KeyEventArgs e)
        {
            if (k == e.KeyCode) 
            {
                isPressed = true;
            }
        }
        public void PollKeyUp(KeyEventArgs e)
        {
            if (k == e.KeyCode) isPressed = false;
        }
        public void UpdateKeyBinding(Keys newKey)
        {
            k = newKey;
        }

    }
    #endregion
}