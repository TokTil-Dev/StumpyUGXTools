using OpenTK;
using OpenTK.Graphics.OpenGL;
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
        OGLCamera camera = new OGLCamera();
        Model ugxModel;
        List<Model> imports = new List<Model>();
        List<TreeNode> rootNodes = new List<TreeNode>();
        public void FBXImportPrompt()
        {
            if (gui.fbd.ShowDialog() == DialogResult.OK)
            {
                if (!gui.fbd.FileName.Contains(".fbx"))
                {
                    gui.LogOut("File selected was not an fbx.");
                    return;
                }
                
                Model m = gui.ImportAsset(gui.fbd.FileName);
                TreeNode root = new TreeNode(Path.GetFileNameWithoutExtension(gui.fbd.FileName));
                foreach (Model.Mesh mesh in m.meshes) { root.Nodes.Add(mesh.name); }
                gui.meshEditorTab.fbxMeshTree.Nodes.Add(root);
                root.Expand();
                imports.Add(m);
            }
        }

        Model m;
        OGLMesh mes;

        public int shaderProgram;
        public void InitViewport()
        {

            ugxModel = ugx.GetModel();

            viewport = new OpenTK.GLControl();
            viewport.Location = new Point((gui.EditorToolsLeftMargin * 2) + gui.EditorToolSideWindowWidth, 44);
            viewport.Size = new Size(gui.Width - 50 - gui.EditorToolSideWindowWidth, gui.Height - 95);
            viewport.Paint += new PaintEventHandler(viewport_Paint);
            viewport.KeyDown += new KeyEventHandler(viewport_KeyDown);
            //viewport.KeyUp += new KeyEventHandler(viewport_KeyUp);
            viewport.MakeCurrent();

            #region Init OpenGL
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            #region Shader Strings
            string vs =
    @"#version 330 core
layout (location = 0) in vec3 aPosition;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main()
{
    gl_Position = viewMatrix * vec4(aPosition, 1.0);
}";
            string fs =
    @"#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}";
            #endregion

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
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(1.223f, gui.Width / gui.Height, .01f, 1000f);
            GL.UniformMatrix4(loc, false, ref proj);
            Console.WriteLine(loc);
            #endregion

            gui.Controls.Add(viewport);
            m = gui.ImportAsset("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\backpack.fbx");
            mes = new OGLMesh(m.meshes[0]);
        }


        //WinForm consumables
        void viewport_Paint(object o, EventArgs e)
        {
            viewport.MakeCurrent();
            GL.ClearColor(.3f, .3f, .3f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            mes.Draw();
            viewport.SwapBuffers();
        }
        void viewport_KeyDown(object o, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.position.Z += .025f;
            }
            if (e.KeyCode == Keys.S)
            {
                camera.position.Z -= .025f;
            }
            camera.UpdateCamera();
            viewport.Invalidate();
        }
    }

    #region 3D Editor Classes
    class ViewportObject
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 rotation = Vector3.Zero;
        public Vector3 scale = Vector3.One;
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
            Matrix4 m = Matrix4.LookAt(position, position + new Vector3(0, 0, 1), new Vector3(0, 1, 0));

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
    #endregion
}