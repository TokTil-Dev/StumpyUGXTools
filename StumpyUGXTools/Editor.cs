using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Program;

namespace StumpyUGXTools
{
    class Editor {

        private OpenTK.GLControl viewport;
        Timer viewportRenderTimer;
        Timer viewportUpdateTimer;
        OGLCamera camera = new OGLCamera();

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

        public int shaderProgram;
        public void InitViewport()
        {
            ugxModel = ugx.GetModel();

            viewport = new OpenTK.GLControl();
            viewport.Location = new Point((gui.EditorToolsLeftMargin * 2) + gui.EditorToolSideWindowWidth, 44);
            viewport.Size = new Size(gui.Width - 50 - gui.EditorToolSideWindowWidth, gui.Height - 95);
            viewport.KeyDown += new KeyEventHandler(viewport_KeyDown);
            viewport.KeyUp += new KeyEventHandler(viewport_KeyUp);
            viewport.LostFocus += new EventHandler(viewport_LostFocus);
            viewport.MakeCurrent();

            InitOGL();
            viewport.SwapBuffers();
            InitViewportInput();

            //temp
            m = gui.ImportAsset("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\backpack.fbx");
            mes = new OGLMesh(m.meshes[0]);

            gui.Controls.Add(viewport);

            viewportUpdateTimer = new Timer();
            viewportUpdateTimer.Tick += new EventHandler(viewport_Update);
            viewportUpdateTimer.Interval = 10;
            viewportUpdateTimer.Start();

            viewportRenderTimer = new Timer();
            viewportRenderTimer.Tick += new EventHandler(viewport_Paint);
            viewportRenderTimer.Interval = 10;
            viewportRenderTimer.Start();
        }
        public void InitOGL()
        {
            #region Shader Strings
            string vs =
    @"#version 330 core
layout (location = 0) in vec3 aPosition;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main()
{
    gl_Position = projectionMatrix * viewMatrix * vec4(aPosition, 1.0);
}";
            string fs =
    @"#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}";
            #endregion

            #region Init OpenGL
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
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertS);
            GL.AttachShader(shaderProgram, fragS);
            GL.LinkProgram(shaderProgram);
            GL.UseProgram(shaderProgram);

            int loc = GL.GetUniformLocation(shaderProgram, "projectionMatrix");
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(1.523f, gui.Width / gui.Height, .01f, 250f);
            GL.UniformMatrix4(loc, false, ref proj);
            Console.WriteLine(loc);
            #endregion
        }

        //viewport functions
        void viewport_Paint(object o, EventArgs e)
        {
            GL.ClearColor(.3f, .3f, .3f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            viewport_Draw();
            viewport.SwapBuffers();
        }
        void viewport_Draw()
        {
            camera.UpdateCamera();
            mes.Draw();
        }

        void viewport_Update(object o, EventArgs e)
        {
            if (keys[KeyName.forward].isPressed)  camera.Translate(0, 0, .0025f);
            if (keys[KeyName.backward].isPressed) camera.Translate(0, 0, -.0025f);
            if (keys[KeyName.left].isPressed)     camera.Translate(-.0025f, 0, 0);
            if (keys[KeyName.right].isPressed)    camera.Translate(.0025f, 0, 0);
        }

        void viewport_LostFocus(object o, EventArgs e)
        {
            foreach(InputKey k in keys.Values)
            {
                k.isPressed = false;
            }
        }

        //input
        enum KeyName { forward,backward,left,right,up,down }
        Dictionary<KeyName, InputKey> keys;
        void InitViewportInput()
        {
            keys = new Dictionary<KeyName, InputKey>
            {
                {KeyName.forward,   new InputKey(Keys.W) },
                {KeyName.backward,  new InputKey(Keys.S) },
                {KeyName.left,      new InputKey(Keys.A) },
                {KeyName.right,     new InputKey(Keys.D) },
                {KeyName.up,        new InputKey(Keys.Space) },
                {KeyName.down,      new InputKey(Keys.LShiftKey) }
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
    class OGLCamera : ViewportObject
    {
        public float speed = 1.5f;
        public float mouseSensitivity = 0.0025f;

        public Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

        public void UpdateCamera()
        {
            Console.WriteLine(position);
            Matrix4 m = Matrix4.LookAt(position, position + new Vector3(0, 0, 1), new Vector3(0, 1, 0)) * new Matrix4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            int loc = GL.GetUniformLocation(gui._3dEditor.shaderProgram, "viewMatrix");
            GL.UniformMatrix4(loc, false, ref m);
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
            //foreach (Model.Mesh.Vertex v in mesh.vertices)
            //{
            //    GLVertex glv = new GLVertex();
            //    glv.x = v.vx;
            //    glv.y = v.vy;
            //    glv.z = v.vz;
            //    glv.u = v.tu;
            //    glv.v = v.tv;
            //    glVertices.Add(glv);
            //}
            //foreach (int i in mesh.indices)
            //{
            //    glIndices.Add((uint)i);
            //}

            GLVertex v1 = new GLVertex();
            GLVertex v2 = new GLVertex();
            GLVertex v3 = new GLVertex();
            GLVertex v4 = new GLVertex();
            v1.x = 0; v1.y = 0; v1.z = 0;
            v2.x = 1; v2.y = 0; v2.z = 0;
            v3.x = 0; v3.y = 1; v3.z = 0;
            v4.x = 1; v4.y = 1; v4.z = 0;
            glVertices.Add(v1);
            glVertices.Add(v2);
            glVertices.Add(v3);
            glVertices.Add(v4);
            glIndices.Add(0);
            glIndices.Add(1);
            glIndices.Add(2);
            glIndices.Add(1);
            glIndices.Add(2);
            glIndices.Add(3);

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