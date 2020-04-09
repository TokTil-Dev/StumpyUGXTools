using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using Assimp;
using SystemHalf;
using static Program;
using StumpyUGXTools;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;
using OpenTK;
using Quaternion = OpenTK.Quaternion;
using OpenTK.Input;
using Examples.TextureLoaders;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;
using System.Reflection;

namespace StumpyUGXTools
{
    class Editor : Form
    {
        
        public int EditorToolSideWindowWidth;
        public int EditorToolsLeftMargin;
        public OpenFileDialog fbd;
        public TextBox version;
        private System.ComponentModel.IContainer components;
        public ToolTip toolTips;
        public TabControl editorSelecter;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openUGXToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem fBXToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem keyBindingsToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem textureReferencePathToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;

        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.fbd = new System.Windows.Forms.OpenFileDialog();
            this.version = new System.Windows.Forms.TextBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.editorSelecter = new System.Windows.Forms.TabControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openUGXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fBXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyBindingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureReferencePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // version
            // 
            this.version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.version.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.version.Location = new System.Drawing.Point(523, 13);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(140, 13);
            this.version.TabIndex = 6;
            this.version.Text = "0.0.0";
            this.version.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // editorSelecter
            // 
            this.editorSelecter.Location = new System.Drawing.Point(12, 24);
            this.editorSelecter.Name = "editorSelecter";
            this.editorSelecter.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.editorSelecter.SelectedIndex = 0;
            this.editorSelecter.Size = new System.Drawing.Size(400, 23);
            this.editorSelecter.TabIndex = 9;
            this.editorSelecter.SelectedIndexChanged += new System.EventHandler(this.EditorSelect);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1450, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openUGXToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.importToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openUGXToolStripMenuItem
            // 
            this.openUGXToolStripMenuItem.Name = "openUGXToolStripMenuItem";
            this.openUGXToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.openUGXToolStripMenuItem.Text = "Open UGX";
            this.openUGXToolStripMenuItem.Click += new System.EventHandler(this.openUGXToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fBXToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.importToolStripMenuItem.Text = "Import...";
            // 
            // fBXToolStripMenuItem
            // 
            this.fBXToolStripMenuItem.Name = "fBXToolStripMenuItem";
            this.fBXToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.fBXToolStripMenuItem.Text = "FBX";
            this.fBXToolStripMenuItem.Click += new System.EventHandler(this.fBXToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyBindingsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // keyBindingsToolStripMenuItem
            // 
            this.keyBindingsToolStripMenuItem.Name = "keyBindingsToolStripMenuItem";
            this.keyBindingsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.keyBindingsToolStripMenuItem.Text = "Key Bindings";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textureReferencePathToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // textureReferencePathToolStripMenuItem
            // 
            this.textureReferencePathToolStripMenuItem.Name = "textureReferencePathToolStripMenuItem";
            this.textureReferencePathToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.textureReferencePathToolStripMenuItem.Text = "Texture Reference Path";
            this.textureReferencePathToolStripMenuItem.Click += new System.EventHandler(this.textureReferencePathToolStripMenuItem_Click);
            // 
            // Editor
            // 
            this.ClientSize = new System.Drawing.Size(1450, 720);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.editorSelecter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "Editor";
            this.Text = "UGXTools";
            this.Load += new System.EventHandler(this.GUIInit);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public void LogOut(string s)
        {
            Console.WriteLine(s);
            //if(s != lastString)
            //{
            //    logBox.Text = s;
            //    lastString = s;
            //    numRepeated = 1;
            //}
            //else
            //{
            //    numRepeated++;
            //    logBox.Text = s + " (" + numRepeated + ")";
            //}
        }

        public static UGXFile ugx = new UGXFile();
        public Viewport viewport;
        public MeshEditor meshEditorTab;
        public MaterialEditor materialEditorTab;

        List<Mesh> imports = new List<Mesh>();
        List<uint> materialDiffuseTextures = new List<uint>();
        uint nullTexture;

        string textureReferencePath = "F:\\HaloWarsModding\\HaloWarsDE\\Extract\\art";

        // UI //
        enum SelectedView { Null, Mesh, Material }
        void SwapView(SelectedView v)
        {
            if (v == SelectedView.Null)
            {
                materialEditorTab.Hide();
                meshEditorTab.Hide();
            }

            if (v == SelectedView.Material && ugxLoaded)
            {
                meshEditorTab.Hide();
                materialEditorTab.Show();
                int i = materialEditorTab.materialSelector.SelectedIndex;
                materialEditorTab.materialSelector.SelectedIndex = -1;
                materialEditorTab.materialSelector.SelectedIndex = i;
            }

            if (v == SelectedView.Mesh && ugxLoaded)
            {
                materialEditorTab.Hide();
                meshEditorTab.Show();
            }
        }
        void DoUI()
        {
            SwapView(SelectedView.Mesh);
            Controls.Add(editorSelecter);
            materialEditorTab.SetupPathView();
            meshEditorTab.SetupMeshView();
            viewport.InitViewport();

            int sel = editorSelecter.SelectedIndex;
            editorSelecter.SelectedIndex = -1;
            editorSelecter.SelectedIndex = sel;
        }
        /////// WinForms Functions
        void GUIInit(object o, EventArgs e)
        {
            EditorToolSideWindowWidth = 400;
            EditorToolsLeftMargin = 11;
            meshEditorTab = new MeshEditor();
            materialEditorTab = new MaterialEditor();
            viewport = new Viewport();
            DoUGXLoad("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\assault_rifle_01_out.ugx");
        }
        void EditorSelect(object o, EventArgs e)
        {
            if (editorSelecter.SelectedIndex == 0)
            {
                SwapView(SelectedView.Mesh);
            }
            if (editorSelecter.SelectedIndex == 1)
            {
                SwapView(SelectedView.Material);
            }
        }

        // Loading/Saving //
        string openFilePath;
        bool ugxLoaded = false;
        int  DoUGXLoad(string path)
        {
            if (File.Exists(path))
            {
                UGXFile f = new UGXFile();
                if (f.Load(path) == -1)
                { return -1; }
                else
                {
                    materialEditorTab.Clear();
                    f.fileName = Path.GetFileNameWithoutExtension(path);
                    f.InitTextureEditing();
                    f.InitMeshEditing();
                    ugx = f;
                    LogOut("UGX Loaded: " + path);
                    openFilePath = path;
                    ugxLoaded = true;
                    DoUI();
                    LoadDiffuseTextures();
                    return 1;
                }
            }
            else
            {
                LogOut("File not found: " + path);
                return -1;
            }
        }
        void DoUGXUnload()
        {
            ugxLoaded = false;
            SwapView(SelectedView.Null);
            materialEditorTab.ClearTextBoxes();
            openFilePath = "";
        }
        void DoUGXSave()
        {
            if (!ugxLoaded) return;
            materialEditorTab.SaveMaterial();
            ugx.SaveNewMaterial();
            ugx.Save(openFilePath);
            LogOut("File saved.");
        }
        public List<Mesh> ImportAsset(string path)
        {
            //use assimp to load a model.
            AssimpContext imp = new AssimpContext();
            Scene asset = imp.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FindDegenerates | PostProcessSteps.FixInFacingNormals);

            List<Mesh> meshes = new List<Mesh>();
            //for each mesh in the imported file, collect vertices and indices.
            for (int i = 0; i < asset.MeshCount; i++)
            {
                Mesh mesh = new Mesh();
                mesh.vertices = new List<Mesh.Vertex>();
                mesh.indices = new List<uint>();
                mesh.isSkinned = false;
                mesh.vertexSize = 24;
                mesh.name = asset.Meshes[i].Name;

                //collet vertices for this mesh
                for (int ve = 0; ve < asset.Meshes[i].VertexCount; ve++)
                {
                    Mesh.Vertex v = new Mesh.Vertex();
                    v.x = (SystemHalf.Half)asset.Meshes[i].Vertices[ve].X;
                    v.y = (SystemHalf.Half)asset.Meshes[i].Vertices[ve].Y;
                    v.z = (SystemHalf.Half)asset.Meshes[i].Vertices[ve].Z;
                    v.nx = (SystemHalf.Half)asset.Meshes[i].Normals[ve].X;
                    v.ny = (SystemHalf.Half)asset.Meshes[i].Normals[ve].Y;
                    v.nz = (SystemHalf.Half)asset.Meshes[i].Normals[ve].Z;
                    v.u = (SystemHalf.Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].X;
                    v.v = (SystemHalf.Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].Y;
                    mesh.vertices.Add(v);
                }
                //collect indices for this mesh
                for (int j = 0; j < asset.Meshes[i].FaceCount; j++)
                {
                    for (int k = 0; k < asset.Meshes[i].Faces[j].IndexCount; k++)
                    {
                        mesh.indices.Add((uint)asset.Meshes[i].Faces[j].Indices[k]);
                    }
                    mesh.faceCount++;
                }
                meshes.Add(mesh);
            }

            return meshes;
        }
        public void LoadDiffuseTextures()
        {
            materialDiffuseTextures.Clear();
            for (int i = 0; i < materialEditorTab.matData.Count; i++)
            {
                uint name = 65535;
                TextureTarget t;
                try
                {
                    ImageDDS.LoadFromDisk(textureReferencePath + materialEditorTab.matData[i].pathStrings[0] + ".ddx", out name, out t);
                }
                catch { Console.WriteLine("A"); }
                materialDiffuseTextures.Add(name);
            }
        }
        /////// WinForms Functions
        private void openUGXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                DoUGXLoad(fbd.FileName);
            }
        }
        private void fBXToolStripMenuItem_Click(object o, EventArgs e)
        {
            if (editor.fbd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(editor.fbd.FileName) != ".fbx" && Path.GetExtension(editor.fbd.FileName) != ".FBX")
                {
                    editor.LogOut("File selected was not an fbx.");
                    return;
                }

                List<Mesh> m = editor.ImportAsset(editor.fbd.FileName);
                MeshEditor.RootNode rn = new MeshEditor.RootNode();
                rn.SetName(Path.GetFileNameWithoutExtension(fbd.FileName));

                foreach(Mesh mesh in m)
                {
                    mesh.InitDrawing();

                    MeshEditor.MeshNode mn = new MeshEditor.MeshNode();
                    mn.SetName(mesh.name);
                    rn.AddChild(mn);
                    mn.meshIndex = imports.Count;
                    imports.Add(mesh);
                }

                rn.AddToTree(meshEditorTab.fbxMeshTree);
                viewport.viewport.Invalidate();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoUGXSave();
        }
        private void textureReferencePathToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        // Classes ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public enum AttribType { FLOAT, UINT8, UINT16, UINT32 }
        public class PathBox
        {
            MaterialEditor materialEditor;
            int index;
            public PathBox(string nameStr, int offset, MaterialEditor me)
            {
                index = offset;
                materialEditor = me;

                int space = 42;
                int initOffset = 80;
                //gui.Controls.Add(value);
                //gui.Controls.Add(name);
                ////gui.Controls.Add(buttonAdd);
                ////gui.Controls.Add(buttonRemove);
                //gui.Controls.Add(buttonRevert);
                //gui.Controls.Add(nameUVW);
                //gui.Controls.Add(U);
                //gui.Controls.Add(V);
                //gui.Controls.Add(W);
                //gui.Controls.Add(buttonRevertUVW);

                //buttonAdd.Location = new Point(160 - m, 165 + y + (offset * space));
                //buttonAdd.Size = new Size(20, 20);
                //buttonAdd.Text = "+";
                //buttonAdd.Click += new EventHandler(ButtonAddPress);
                //buttonAdd.Font = new Font("Microsoft Sans Serif", 10F);

                //buttonRemove.Location = new Point(616- 120 - m, 165 + y + (offset * space));
                //buttonRemove.Size = new Size(20, 20);
                //buttonRemove.Text = "×";
                //buttonRemove.Click += new EventHandler(ButtonRemovePress);
                //buttonRemove.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                //buttonRemove.Font = new Font("Microsoft Sans Serif", 10F);

                name.BorderStyle = BorderStyle.None;
                name.Location = new Point(editor.EditorToolsLeftMargin - 1, initOffset + (offset * space));
                name.Name = nameStr + "_name";
                name.Size = new Size(100, 13);
                name.TabIndex = offset;
                name.Text = nameStr;

                value.Location = new Point(editor.EditorToolsLeftMargin, initOffset + 13 + (offset * space));
                value.Name = nameStr + "_value";
                value.Size = new Size(editor.EditorToolSideWindowWidth - 24 - 90, 20);
                value.TabIndex = offset + 1;
                value.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
                value.TextChanged += new EventHandler(PathUpdated);

                buttonRevert.Location = new Point(editor.EditorToolsLeftMargin + editor.EditorToolSideWindowWidth - 22, initOffset + 13 + (offset * space));
                buttonRevert.Size = new Size(20, 20);
                buttonRevert.Text = "↶";
                buttonRevert.Font = new Font("Microsoft Sans Serif", 10F);
                buttonRevert.Click += new EventHandler(RevertButtonPress);
                buttonRevert.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                nameUVW.BorderStyle = BorderStyle.None;
                nameUVW.Location = new Point(editor.EditorToolsLeftMargin + value.Width, initOffset + (offset * space));
                nameUVW.Name = nameStr + "_nameUVW";
                nameUVW.Size = new Size(100, 13);
                nameUVW.TabIndex = offset;
                nameUVW.Text = "UVW Velocity";
                nameUVW.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                U.Location = new Point(editor.EditorToolsLeftMargin + value.Width - 1, initOffset + 13 + (offset * space));
                U.Size = new Size(31, 20);
                U.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                U.TextChanged += new EventHandler(UUpdated);
                V.Location = new Point(U.Location.X + U.Width - 1, initOffset + 13 + (offset * space));
                V.Size = new Size(31, 20);
                V.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                V.TextChanged += new EventHandler(VUpdated);
                W.Location = new Point(V.Location.X + V.Width - 1, initOffset + 13 + (offset * space));
                W.Size = new Size(31, 20);
                W.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                W.TextChanged += new EventHandler(WUpdated);
                //buttonRevertUVW.Location = new Point(gui.EditorToolsLeftMargin, 165 + (offset * space));
                //buttonRevertUVW.Size = new Size(20, 20);
                //buttonRevertUVW.Text = "↶";
                //buttonRevertUVW.Font = new Font("Microsoft Sans Serif", 10F);
                //buttonRevertUVW.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                //buttonRevertUVW.Click += new EventHandler(RevertPressedUVW);

                editor.toolTips.SetToolTip(buttonRevert, "Revert to default value.");
                editor.toolTips.SetToolTip(buttonRevertUVW, "Revert to default value.");
                editor.toolTips.SetToolTip(buttonRemove, "Remove this texture map from this material.");
                editor.toolTips.SetToolTip(buttonAdd, "Add a new texture map to this material.");

                Update(false);
            }
            public void Update(bool hasValue)
            {
                if (hasValue)
                {
                    editor.Controls.Add(value);
                    editor.Controls.Add(buttonRevert);
                    //gui.Controls.Add(buttonRemove); //removed for now
                    //gui.Controls.Remove(buttonAdd); //removed for now
                    editor.Controls.Add(nameUVW);
                    editor.Controls.Add(U);
                    editor.Controls.Add(V);
                    editor.Controls.Add(W);
                    editor.Controls.Add(buttonRevertUVW);
                }
                if (!hasValue)
                {
                    editor.Controls.Remove(value);
                    editor.Controls.Remove(buttonRevert);
                    //gui.Controls.Remove(buttonRemove); //removed for now
                    //gui.Controls.Add(buttonAdd);       //removed for now.
                    editor.Controls.Remove(nameUVW);
                    editor.Controls.Remove(U);
                    editor.Controls.Remove(V);
                    editor.Controls.Remove(W);
                    editor.Controls.Remove(buttonRevertUVW);
                }
            }
            void ButtonAddPress(object o, EventArgs e)
            {
                if (materialEditor.materialSelector.SelectedIndex > -1)
                {
                    materialEditor.matData[materialEditor.materialSelector.SelectedIndex].hasValue[index] = true;
                    Update(true);
                }
            }
            void ButtonRemovePress(object o, EventArgs e)
            {
                if (materialEditor.materialSelector.SelectedIndex > -1)
                {
                    materialEditor.matData[materialEditor.materialSelector.SelectedIndex].hasValue[index] = false;
                    Update(false);
                }
            }
            void RevertButtonPress(object o, EventArgs e)
            {
                if (materialEditor.materialSelector.SelectedIndex > -1) value.Text = materialEditor.matData[materialEditor.materialSelector.SelectedIndex].pathStrings_original[index];
            }
            void PathUpdated(object o, EventArgs e)
            {
                if (materialEditor.materialSelector.SelectedIndex > -1) materialEditor.matData[materialEditor.materialSelector.SelectedIndex].pathStrings[index] = value.Text;
                if(index == 0)
                {
                    editor.LoadDiffuseTextures();
                }
            }
            void UUpdated(object o, EventArgs e)
            {
                if (U.Text != "")
                {
                    float f;
                    if (!float.TryParse(U.Text, out f))
                    {
                        U.Text = "0";
                        U.Select(1, 0);
                    }
                    U.Text = new string(U.Text.Where(c => c >= '0' && c <= '9' || c == '-' || c == '.').ToArray());
                    if (materialEditor.materialSelector.SelectedIndex > -1) materialEditor.matData[materialEditor.materialSelector.SelectedIndex].uStrings[index] = U.Text;
                }
            }
            void VUpdated(object o, EventArgs e)
            {
                if (V.Text != "")
                {
                    float f;
                    if (!float.TryParse(V.Text, out f))
                    {
                        V.Text = "0";
                        V.Select(1, 0);
                    }
                    V.Text = new string(V.Text.Where(c => c >= '0' && c <= '9' || c == '-' || c == '.').ToArray());
                    if (materialEditor.materialSelector.SelectedIndex > -1) materialEditor.matData[materialEditor.materialSelector.SelectedIndex].vStrings[index] = V.Text;
                }
            }
            void WUpdated(object o, EventArgs e)
            {
                if (W.Text != "")
                {
                    float f;
                    if (!float.TryParse(W.Text, out f))
                    {
                        W.Text = "0";
                        W.Select(1, 0);
                    }
                    W.Text = new string(W.Text.Where(c => c >= '0' && c <= '9' || c == '-' || c == '.').ToArray());
                    if (materialEditor.materialSelector.SelectedIndex > -1) materialEditor.matData[materialEditor.materialSelector.SelectedIndex].wStrings[index] = W.Text;
                }
            }
            void RevertPressedUVW(object o, EventArgs e)
            {
                if (materialEditor.materialSelector.SelectedIndex > -1)
                {
                    U.Text = materialEditor.matData[materialEditor.materialSelector.SelectedIndex].uStrings_original[index];
                    V.Text = materialEditor.matData[materialEditor.materialSelector.SelectedIndex].vStrings_original[index];
                    W.Text = materialEditor.matData[materialEditor.materialSelector.SelectedIndex].wStrings_original[index];
                }
            }

            public void EnableView()
            {
                editor.Controls.Add(name);
            }
            public void DisableView()
            {
                editor.Controls.Remove(value);
                editor.Controls.Remove(name);
                editor.Controls.Remove(buttonAdd);
                editor.Controls.Remove(buttonRemove);
                editor.Controls.Remove(buttonRevert);
                editor.Controls.Remove(nameUVW);
                editor.Controls.Remove(U);
                editor.Controls.Remove(V);
                editor.Controls.Remove(W);
            }

            public Label name = new Label();
            public TextBox value = new TextBox();
            Button buttonAdd = new Button();
            Button buttonRemove = new Button();
            Button buttonRevert = new Button();

            Label nameUVW = new Label();
            public TextBox U = new TextBox();
            public TextBox V = new TextBox();
            public TextBox W = new TextBox();
            Button buttonRevertUVW = new Button();
        }
        public class AttribBox
        {
            int index;
            MaterialEditor materialEditor;
            public AttribBox(string nameStr, int ind, int gridX, int gridY, AttribType type, MaterialEditor me)
            {
                materialEditor = me;
                index = ind;
                t = type;

                int ySpace = 40;
                int xSpace = 67;

                value.Location = new Point(editor.EditorToolsLeftMargin + (gridX * xSpace), 645 + (gridY * ySpace));
                value.Name = nameStr + "_value";
                value.Size = new Size(38, 20);
                value.TabIndex = ind + 1;
                value.TextChanged += new EventHandler(ValueUpdated);

                buttonRevert.Location = new Point(value.Location.X + value.Width + 2, value.Location.Y);
                buttonRevert.Size = new Size(20, 20);
                buttonRevert.Font = new Font("Microsoft Sans Serif", 10F);
                buttonRevert.Text = "↶";
                buttonRevert.Click += new EventHandler(RevertButtonPress);

                name.BorderStyle = BorderStyle.None;
                name.Location = new Point(editor.EditorToolsLeftMargin + (gridX * xSpace) - 2, 632 + (gridY * ySpace));
                name.Name = nameStr + "_name";
                name.Size = new Size(buttonRevert.Width + 8 + value.Width, 13);
                name.TabIndex = ind;
                name.Text = nameStr;

                editor.toolTips.SetToolTip(buttonRevert, "Revert to default value.");
                editor.toolTips.SetToolTip(name, name.Text);
            }

            void ValueUpdated(object o, EventArgs e)
            {
                if (value.Text != "")
                {
                    if (t == AttribType.UINT8 || t == AttribType.UINT16 || t == AttribType.UINT32)
                    {
                        Int64 i;
                        if (!Int64.TryParse(value.Text, out i)) { value.Text = "0"; value.Select(1, 0); }
                        if (t == AttribType.UINT8)
                        {
                            if (i > 255) { value.Text = "255"; value.Select(3, 0); }
                            if (i < 0) { value.Text = "0"; value.Select(1, 0); }
                        }
                        if (t == AttribType.UINT16)
                        {
                            if (i > UInt16.MaxValue) { value.Text = UInt16.MaxValue.ToString(); value.Select(5, 0); }
                            if (i < UInt16.MinValue) { value.Text = UInt16.MinValue.ToString(); value.Select(1, 0); }
                        }
                        if (t == AttribType.UINT32)
                        {
                            if (i > UInt32.MaxValue) { value.Text = UInt32.MaxValue.ToString(); value.Select(10, 0); }
                            if (i < UInt32.MinValue) { value.Text = UInt32.MinValue.ToString(); value.Select(1, 0); }
                        }
                        value.Text = new string(value.Text.Where(c => c >= '0' && c <= '9').ToArray());
                    }
                    if (t == AttribType.FLOAT)
                    {
                        float f;
                        if (!float.TryParse(value.Text, out f))
                        {
                            value.Text = "0";
                            value.Select(1, 0);
                        }
                        value.Text = new string(value.Text.Where(c => c >= '0' && c <= '9' || c == '-' || c == '.').ToArray());

                    }
                    if (materialEditor.materialSelector.SelectedIndex > -1) materialEditor.matData[materialEditor.materialSelector.SelectedIndex].attribValues[index] = value.Text;
                }
            }
            void RevertButtonPress(object o, EventArgs e)
            {
                if (materialEditor.materialSelector.SelectedIndex > -1) value.Text = materialEditor.matData[materialEditor.materialSelector.SelectedIndex].attribValues_original[index];
            }

            public void EnableView()
            {
                editor.Controls.Add(value);
                editor.Controls.Add(buttonRevert);
                editor.Controls.Add(name);
            }
            public void DisableView()
            {
                editor.Controls.Remove(value);
                editor.Controls.Remove(buttonRevert);
                editor.Controls.Remove(name);
            }

            AttribType t;
            public TextBox value = new TextBox();
            Button buttonRevert = new Button();
            Label name = new Label();

        }
        public class MatData
        {
            public BDTNode linkedNode;
            public TabPage tab = new TabPage();
            public MatData(int index, MaterialEditor me)
            {
                me.materialSelector.Controls.Add(tab);
                tab.Text = "Material " + (index + 1).ToString();
            }
            public bool[] hasValue = new bool[13];
            public bool[] hasValue_original = new bool[13];
            public string[] pathStrings = new string[13];
            public string[] pathStrings_original = new string[13];
            public string[] uStrings = new string[13];
            public string[] vStrings = new string[13];
            public string[] wStrings = new string[13];
            public string[] uStrings_original = new string[13];
            public string[] vStrings_original = new string[13];
            public string[] wStrings_original = new string[13];
            public string[] attribValues = new string[12];
            public string[] attribValues_original = new string[12];

        }

        public class EditorTab
        {
            protected TabPage tab = new TabPage();
            public EditorTab()
            {
                editor.editorSelecter.Controls.Add(tab);
            }

            public virtual void Show()
            {

            }
            public virtual void Hide()
            {

            }
        }
        public class MeshEditor : EditorTab
        {
            public TreeView ugxMeshTree = new TreeView();
            public TreeView fbxMeshTree = new TreeView();

            public MeshEditor() : base()
            {
                int xSpace = 10;
                tab.Text = "Mesh Editor";

                ugxMeshTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                ugxMeshTree.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
                ugxMeshTree.ItemHeight = 15;
                ugxMeshTree.Location = new Point(editor.EditorToolsLeftMargin, 118);
                ugxMeshTree.Name = "ugxMeshList";
                ugxMeshTree.Size = new Size((editor.EditorToolSideWindowWidth / 2) - (xSpace), 500);
                ugxMeshTree.TabIndex = 11;

                fbxMeshTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                fbxMeshTree.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                fbxMeshTree.ItemHeight = 15;
                fbxMeshTree.Location = new Point(editor.EditorToolsLeftMargin + (editor.EditorToolSideWindowWidth / 2) + (xSpace), 118);
                fbxMeshTree.Name = "fbxMeshList";
                fbxMeshTree.Size = new Size((editor.EditorToolSideWindowWidth / 2) - (xSpace), 500);
                fbxMeshTree.TabIndex = 13;
            }
            public override void Show()
            {
                editor.Controls.Add(ugxMeshTree);
                editor.Controls.Add(fbxMeshTree);
                editor.Controls.Add(editor.viewport.viewport);
            }
            public override void Hide()
            {
                editor.Controls.Remove(ugxMeshTree);
                editor.Controls.Remove(fbxMeshTree);
            }
            public void SetupMeshView()
            {
                TreeNode n = ugxMeshTree.Nodes.Add(ugx.fileName);
                for (int i = 0; i < ugx.subDataCount[0]; i++)
                {
                    n.Nodes.Add(ugx.meshNames[i]);
                }
                ugxMeshTree.ExpandAll();
            }

            public class RootNode
            {
                public void AddChild(MeshNode mn)
                {
                    mn.SetParent(treeNode);
                    children.Add(mn);
                }
                public void SetName(string s)
                {
                    treeNode.Text = s;
                }
                public void AddToTree(TreeView tv)
                {
                    tv.Nodes.Add(treeNode);
                    treeNode.Expand();
                }
                private TreeNode treeNode = new TreeNode();
                private List<MeshNode> children = new List<MeshNode>();
            }
            public class MeshNode
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem importedMeshMenuItem = new ToolStripMenuItem();
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem();
                public MeshNode()
                {
                    cms.Items.Add(importedMeshMenuItem);
                    importedMeshMenuItem.Click += new EventHandler(showMeshMenu);
                    importedMeshMenuItem.Text = "Mesh Settings";
                    treeNode.ContextMenuStrip = cms;
                }
                void showMeshMenu(object o, EventArgs e)
                {
                    Console.WriteLine("A");
                    //editor.ShowImportedMeshMenu(meshIndex);
                }

                public void SetParent(TreeNode t)
                {
                    t.Nodes.Add(treeNode);
                }
                public void SetName(string s)
                {
                    treeNode.Text = s;
                }
                private TreeNode treeNode = new TreeNode();
                public int meshIndex = -1;

            }

        }
        public class MaterialEditor : EditorTab
        {
            public TabControl materialSelector = new TabControl();

            AttribBox[] attribBoxes = new AttribBox[12];
            PathBox[] pathBoxes = new PathBox[13];
            public List<MatData> matData = new List<MatData>();

            public MaterialEditor() : base()
            {
                tab.Text = "Material Editor";

                this.materialSelector.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.materialSelector.Location = new Point(editor.EditorToolsLeftMargin, 50);
                this.materialSelector.Name = "materialSelector";
                this.materialSelector.SelectedIndex = -1;
                this.materialSelector.Size = new Size(editor.EditorToolSideWindowWidth, 23);
                this.materialSelector.SelectedIndexChanged += new EventHandler(SelectedMaterialChanged);
                materialSelector.SelectedIndex = 100;
                materialSelector.SelectedIndex = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (i == 0) attribBoxes[i] = new AttribBox("SpecPower", i, 0, 0, AttribType.FLOAT, this);
                    if (i == 1) attribBoxes[i] = new AttribBox("SpecColorR", i, 1, 0, AttribType.FLOAT, this);
                    if (i == 2) attribBoxes[i] = new AttribBox("SpecColorG", i, 2, 0, AttribType.FLOAT, this);
                    if (i == 3) attribBoxes[i] = new AttribBox("SpecColorB", i, 3, 0, AttribType.FLOAT, this);
                    if (i == 4) attribBoxes[i] = new AttribBox("Reflectivity", i, 4, 0, AttribType.FLOAT, this);
                    if (i == 5) attribBoxes[i] = new AttribBox("Sharpness", i, 5, 0, AttribType.FLOAT, this);
                    if (i == 6) attribBoxes[i] = new AttribBox("Fresnel", i, 0, 1, AttribType.FLOAT, this);
                    if (i == 7) attribBoxes[i] = new AttribBox("FresnelPwr", i, 1, 1, AttribType.FLOAT, this);
                    if (i == 8) attribBoxes[i] = new AttribBox("AccIndex", i, 2, 1, AttribType.UINT32, this);
                    if (i == 9) attribBoxes[i] = new AttribBox("Flags", i, 3, 1, AttribType.UINT32, this);
                    if (i == 10) attribBoxes[i] = new AttribBox("BlendType", i, 4, 1, AttribType.UINT8, this);
                    if (i == 11) attribBoxes[i] = new AttribBox("Opacity", i, 5, 1, AttribType.UINT8, this);
                }
                for (int i = 0; i < 13; i++)
                {
                    if (i == 0) pathBoxes[i] = new PathBox("Diffuse", i, this);
                    if (i == 1) pathBoxes[i] = new PathBox("Normal", i, this);
                    if (i == 2) pathBoxes[i] = new PathBox("Gloss", i, this);
                    if (i == 3) pathBoxes[i] = new PathBox("Opacity", i, this);
                    if (i == 4) pathBoxes[i] = new PathBox("Xform", i, this);
                    if (i == 5) pathBoxes[i] = new PathBox("Emmissive", i, this);
                    if (i == 6) pathBoxes[i] = new PathBox("Ao", i, this);
                    if (i == 7) pathBoxes[i] = new PathBox("Env", i, this);
                    if (i == 8) pathBoxes[i] = new PathBox("EnvMask", i, this);
                    if (i == 9) pathBoxes[i] = new PathBox("EmXform", i, this);
                    if (i == 10) pathBoxes[i] = new PathBox("Distortion", i, this);
                    if (i == 11) pathBoxes[i] = new PathBox("Highlight", i, this);
                    if (i == 12) pathBoxes[i] = new PathBox("Modulate", i, this);
                }

                materialSelector.SelectedIndex = 1;
                materialSelector.SelectedIndex = 0;
            }
            public override void Show()
            {
                editor.Controls.Add(materialSelector);
                foreach (PathBox b in pathBoxes)
                {
                    b.EnableView();
                }
                foreach (AttribBox a in attribBoxes)
                {
                    a.EnableView();
                }
                editor.Controls.Add(editor.viewport.viewport);
            }
            public override void Hide()
            {
                editor.Controls.Remove(materialSelector);
                foreach (PathBox b in pathBoxes)
                {
                    b.DisableView();
                }
                foreach (AttribBox a in attribBoxes)
                {
                    a.DisableView();
                }
            }

            private void SelectedMaterialChanged(object o, EventArgs e)
            {
                if (materialSelector.SelectedIndex >= 0)
                {
                    MatData m = matData[materialSelector.SelectedIndex];
                    for (int i = 0; i < 13; i++)
                    {
                        pathBoxes[i].Update(m.hasValue[i]);
                        pathBoxes[i].value.Text = m.pathStrings[i];
                        pathBoxes[i].U.Text = m.uStrings[i];
                        pathBoxes[i].V.Text = m.vStrings[i];
                        pathBoxes[i].W.Text = m.wStrings[i];
                        if (i < 12) attribBoxes[i].value.Text = m.attribValues[i];
                        pathBoxes[i].Update(m.hasValue[i]);
                    }
                }
            }

            public void SetupPathView()
            {
                for (int num = 0; num < ugx.nodes[0].childNodes.Count; num++)
                {
                    MatData d = new MatData(num, this);
                    d.linkedNode = ugx.nodes[0].childNodes[num];
                    for (int i = 0; i < 13; i++)
                    {
                        if (ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].childNodes.Count == 1)
                        {
                            d.pathStrings[i] = (string)ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].childNodes[0].attributeNameValues[0].decodedValue;
                            d.uStrings[i] = ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].attributeNameValues[0].f[0].ToString();
                            d.vStrings[i] = ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].attributeNameValues[0].f[1].ToString();
                            d.wStrings[i] = ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].attributeNameValues[0].f[2].ToString();
                            d.hasValue[i] = true;
                        }
                        else
                        {
                            d.pathStrings[i] = "";
                            d.uStrings[i] = "0";
                            d.vStrings[i] = "0";
                            d.wStrings[i] = "0";
                            d.hasValue[i] = false;
                        }
                        d.pathStrings_original[i] = d.pathStrings[i];
                        d.hasValue_original[i] = d.hasValue[i];
                        d.uStrings_original[i] = d.uStrings[i];
                        d.vStrings_original[i] = d.vStrings[i];
                        d.wStrings_original[i] = d.wStrings[i];

                    }


                    for (int i = 0; i < 12; i++)
                    {
                        if (ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue != null)
                        {
                            //if (i < 8) d.attribValues[i] = string.Format("{0:F1}", ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue);
                            d.attribValues[i] = ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue.ToString();
                        }
                        d.attribValues_original[i] = d.attribValues[i];
                    }
                    matData.Add(d);
                }
                editor.LogOut("Found " + matData.Count + " materials.");
            }
            public void SaveMaterial()
            {
                foreach (MatData m in matData)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (m.hasValue[i])
                        {
                            if (m.pathStrings[i] != m.pathStrings_original[i])
                            {
                                ugx.EncodeNameValueValueString(m.linkedNode.childNodes[1].childNodes[i].childNodes[0].nameValueIndex + 1, m.pathStrings[i]);
                            }
                            if (m.uStrings[i] != m.uStrings_original[i] || m.vStrings[i] != m.vStrings_original[i] || m.wStrings[i] != m.wStrings_original[i])
                            {
                                float q, w, e;
                                float.TryParse(m.uStrings[i], out q); float.TryParse(m.vStrings[i], out w); float.TryParse(m.wStrings[i], out e);
                                ugx.EncodeUVWVelocity(m.linkedNode.childNodes[1].childNodes[i].nameValueIndex + 1, q, w, e);
                            }
                        }
                        m.pathStrings_original[i] = m.pathStrings[i];
                        m.uStrings_original[i] = m.uStrings[i];
                        m.vStrings_original[i] = m.vStrings[i];
                        m.wStrings_original[i] = m.wStrings[i];
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        if (m.attribValues[i] != m.attribValues_original[i])
                        {
                            if (m.linkedNode.childNodes[0].childNodes[i].nodeNameValue.decodedValueType == NameValueFlags_Type.INT)
                            {
                                UInt32 q;
                                if (!UInt32.TryParse(m.attribValues[i], out q)) q = 0;
                                ugx.EncodeNameValueValueUInt(m.linkedNode.childNodes[0].childNodes[i].nameValueIndex, q);
                            }
                            if (m.linkedNode.childNodes[0].childNodes[i].nodeNameValue.decodedValueType == NameValueFlags_Type.FLOAT)
                            {
                                float q;
                                if (!float.TryParse(m.attribValues[i], out q)) q = 0;
                                ugx.EncodeNameValueValueFloat(m.linkedNode.childNodes[0].childNodes[i].nameValueIndex, q);
                            }
                        }
                        m.attribValues_original[i] = m.attribValues[i];
                    }
                }
            }
            public void ClearTextBoxes()
            {
                foreach (PathBox p in pathBoxes)
                {
                    p.value.Text = "";
                    p.Update(false);
                }
                foreach (AttribBox a in attribBoxes)
                {
                    a.value.Text = "0";
                }
            }
            public void Clear()
            {
                foreach (MatData m in matData)
                {
                    materialSelector.Controls.Remove(m.tab);
                }
                matData.Clear();
            }
        }

        public class Viewport
        {
            public OpenTK.GLControl viewport;
            System.Windows.Forms.Timer viewportUpdateTimer;
            ViewportCamera camera = new ViewportCamera();
            ViewportGrid grid = new ViewportGrid();

            public int ubo;

            public void InitViewport()
            {
                viewport = new OpenTK.GLControl();
                viewport.Location = new Point((editor.EditorToolsLeftMargin * 2) + editor.EditorToolSideWindowWidth, 44);
                viewport.Size = new Size(editor.Width - 50 - editor.EditorToolSideWindowWidth, editor.Height - 95);
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
                viewport.GotFocus += new EventHandler((object o, EventArgs e) =>
                {
                    viewportUpdateTimer.Start();
                });
                viewport.Paint += new PaintEventHandler((object o, PaintEventArgs e) => { viewport_Loop(); });
                viewport.MakeCurrent();

                InitOGL();
                InitViewportInput();
                grid.InitGrid();

                editor.Controls.Add(viewport);

                viewportUpdateTimer = new Timer();
                viewportUpdateTimer.Tick += new EventHandler((object o, EventArgs e) => { viewport.Invalidate(); });
                viewportUpdateTimer.Interval = 10;
            }
            public void InitOGL()
            {
                TextureTarget t;
                byte[] b = new byte[16];
                using (var streamReader = new MemoryStream())
                {
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("StumpyUGXTools.null.dds").CopyTo(streamReader);
                    b = streamReader.ToArray();
                }
                ImageDDS.LoadFromDisk(b, out editor.nullTexture, out t);

                Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(1.525f, viewport.Width / viewport.Height, .01f, 250f);

                ubo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.UniformBuffer, ubo);
                GL.BufferData(BufferTarget.UniformBuffer, Marshal.SizeOf(new Matrix4()) * 2, IntPtr.Zero, BufferUsageHint.StreamDraw);
                GL.BindBuffer(BufferTarget.UniformBuffer, 0);

                GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, ubo);

                GL.BindBuffer(BufferTarget.UniformBuffer, ubo);
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, Marshal.SizeOf(new Matrix4()), ref proj);
                GL.BindBuffer(BufferTarget.UniformBuffer, 0);

                GL.Enable(EnableCap.DepthTest);

                //temp
                Mesh m = editor.ImportAsset("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\focus.fbx")[0];
                m.InitDrawing();
                m.Rotate(1.1f, .56f, .343f);
                m.Move(1, 1, 1);
                editor.imports.Add(m);
                //
            }

            //viewport functions
            public void viewport_Loop()
            {
                PollInputs();

                GL.ClearColor(.3f, .3f, .3f, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                //draw
                camera.UpdateCamera();
                grid.DrawGrid();
                foreach (Mesh m in editor.imports)
                {
                    m.Draw();
                }

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

            enum KeyName { forward, backward, left, right, up, down, zoomIn, zoomOut }
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
                foreach (InputKey k in keys.Values)
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

            public class ViewportCamera
            {
                public ViewportCamera()
                {
                    AddRadius(10f);
                    Rotate(0, .75f);
                }

                Vector3 position = new Vector3(0, 0, -10);
                Vector3 target = new Vector3(0, 0, 0);
                float radius = 0;
                public void UpdateCamera()
                {
                    Matrix4 m = Matrix4.LookAt(position, target, new Vector3(0, 1, 0)) * new Matrix4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                    GL.BindBuffer(BufferTarget.UniformBuffer, editor.viewport.ubo);
                    GL.BufferSubData(BufferTarget.UniformBuffer, new IntPtr(Marshal.SizeOf(new Matrix4())), Marshal.SizeOf(new Matrix4()), ref m);
                    GL.BindBuffer(BufferTarget.UniformBuffer, 0);
                }

                public void Rotate(float yaw, float pitch)
                {
                    Vector3 worldUp = new Vector3(0, 1, 0);

                    Vector3 cameraDirection = Vector3.Normalize(position - target);
                    Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(worldUp, cameraDirection));
                    Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

                    Vector4 cameraFocusVector = new Vector4(position - target, 0);


                    if (cameraDirection.Y > .99f && pitch > 0) pitch = 0;
                    if (cameraDirection.Y < -.99f && pitch < 0) pitch = 0;
                    Matrix4 pitchMat = Matrix4.CreateFromAxisAngle(cameraRight, -pitch);

                    Matrix4 yawMat = Matrix4.CreateFromAxisAngle(worldUp, -yaw);

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
            public class ViewportGrid
            {
                int xLineVB, zLineVB, lineIB;
                int xLetterVB, xLetterIB;
                int zLetterVB, zLetterIB;
                public int gridShader;
                public void InitGrid()
                {
                    #region Grid Shader
                    #region Shader Strings
                    //////////////////////////////////////////////////////
                    string vs =
        @"#version 420 core

layout (location = 0) in vec3 positionIn;

layout(std140, binding = 0) uniform GlobalMatrices
{
    mat4 projectionMatrix;
    mat4 viewMatrix;
};
uniform mat4 modelMatrix;

void main()
{
    gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(positionIn, 1.0);
}";
                    //////////////////////////////////////////////////////
                    string fs =
        @"#version 420 core

uniform vec4 color;
out vec4 FragColor;

void main()
{
    FragColor = color;
}";
                    //////////////////////////////////////////////////////
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


                    gridShader = GL.CreateProgram();
                    GL.AttachShader(gridShader, vertS);
                    GL.AttachShader(gridShader, fragS);
                    GL.LinkProgram(gridShader);
                    GL.UseProgram(gridShader);
                    cLoc = GL.GetUniformLocation(gridShader, "color");
                    mLoc = GL.GetUniformLocation(gridShader, "modelMatrix");
                    #endregion

                    #region Line Buffers
                    xLineVB = GL.GenBuffer();
                    zLineVB = GL.GenBuffer();
                    lineIB = GL.GenBuffer();

                    Vector3[] xData = { new Vector3(10, 0, 0), new Vector3(-10, 0, 0) };
                    Vector3[] zData = { new Vector3(0, 0, 10), new Vector3(0, 0, -10) };
                    int[] indices = { 0, 1 };

                    GL.BindBuffer(BufferTarget.ArrayBuffer, xLineVB);
                    GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 2, xData, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, zLineVB);
                    GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 2, zData, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, lineIB);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * 2, indices, BufferUsageHint.StaticDraw);

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    #endregion

                    #region Letter Buffers
                    #region Z
                    Vector3[] zV =
                    {
                        new Vector3(-1f, 0, 1.2f),  //0
                        new Vector3(1f, 0, 1.2f),   //1
                        new Vector3(-1f, 0, 0.8f),  //2
                        new Vector3(1f, 0, 0.8f),   //3
                        new Vector3(0.4f, 0, 0.8f), //4
                        new Vector3(-1f, 0, -0.8f), //5
                        new Vector3(-.4f, 0, -0.8f),//6
                        new Vector3(1,0,-.8f)      ,//7
                        new Vector3(1,0,-1.2f)     ,//8
                        new Vector3(-1,0,-1.2f)     //9
                    };
                    uint[] zI =
                    {
                        0, 1, 2, //top
                        1, 2, 3,

                        4, 3, 5, //middle
                        3, 5, 6,

                        5, 7, 9, //bottom
                        7, 9, 8
                    };
                

                    zLetterVB = GL.GenBuffer();
                    zLetterIB = GL.GenBuffer();

                    GL.BindBuffer(BufferTarget.ArrayBuffer, zLetterVB);
                    GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 10, zV, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, zLetterIB);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * 18, zI, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    #endregion
                    #region X
                    Vector3[] xV =
{
                        new Vector3(1,0,1.2f),
                        new Vector3(-.4f,0,-1.2f),
                        new Vector3(-1,0,-1.2f),
                        new Vector3(.4f,0,1.2f),

                        new Vector3(-1,0,1.2f),
                        new Vector3(-.4f,0,1.2f),
                        new Vector3(1,0,-1.2f),
                        new Vector3(.4f,0,-1.2f),

                    };
                    uint[] xI =
                    {
                        0, 1, 2, //BL-TR
                        0, 2, 3,

                        4, 5, 6, //BR-TL
                        4, 6, 7
                    };


                    xLetterVB = GL.GenBuffer();
                    xLetterIB = GL.GenBuffer();

                    GL.BindBuffer(BufferTarget.ArrayBuffer, xLetterVB);
                    GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 8, xV, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, xLetterIB);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * 12, xI, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    #endregion
                    #endregion
                }
                int cLoc;
                int mLoc;
                int gridQuadrantSize = 10;
                public void DrawGrid()
                {
                    GL.UseProgram(gridShader);
                    Matrix4 m;
                    
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, lineIB);
                    GL.Uniform4(cLoc, new Vector4(0.2f, 0.2f, .2f, 1f));

                    GL.BindBuffer(BufferTarget.ArrayBuffer, xLineVB);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
                    GL.EnableVertexAttribArray(0);
                    for(int i = 1; i < gridQuadrantSize + 1; i++)
                    {
                        m = Matrix4.CreateTranslation(new Vector3(0, 0, i));
                        GL.UniformMatrix4(mLoc, false, ref m);
                        GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
                        m = Matrix4.CreateTranslation(new Vector3(0, 0, -i));
                        GL.UniformMatrix4(mLoc, false, ref m);
                        GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
                    }

                    GL.BindBuffer(BufferTarget.ArrayBuffer, zLineVB);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
                    GL.EnableVertexAttribArray(0);
                    for (int i = 1; i < gridQuadrantSize + 1; i++)
                    {
                        m = Matrix4.CreateTranslation(new Vector3(i, 0, 0));
                        GL.UniformMatrix4(mLoc, false, ref m);
                        GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
                        m = Matrix4.CreateTranslation(new Vector3(-i, 0, 0));
                        GL.UniformMatrix4(mLoc, false, ref m);
                        GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);
                    }
                    

                    GL.Uniform4(cLoc, new Vector4(0.4f, 0.4f, 1.0f, 1.0f));
                    m = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
                    GL.UniformMatrix4(mLoc, false, ref m);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, xLineVB);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
                    GL.EnableVertexAttribArray(0);
                    GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);

                    GL.Uniform4(cLoc, new Vector4(1.0f, 0.4f, 0.4f, 1.0f));
                    m = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
                    GL.UniformMatrix4(mLoc, false, ref m);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, zLineVB);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
                    GL.EnableVertexAttribArray(0);
                    GL.DrawElements(PrimitiveType.Lines, 2, DrawElementsType.UnsignedInt, 0);

                    #region Draw Letters
                    #region Z
                    m = Matrix4.CreateScale(.25f, 1, .25f) * Matrix4.CreateTranslation(new Vector3(0, 0, gridQuadrantSize + .5f));
                    
                    GL.UniformMatrix4(mLoc, false, ref m);
                    GL.Uniform4(cLoc, new Vector4(1.0f, 0.4f, 0.4f, 1.0f));

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, zLetterIB);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, zLetterVB);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
                    GL.EnableVertexAttribArray(0);
                    GL.DrawElements(PrimitiveType.Triangles, 18, DrawElementsType.UnsignedInt, 0);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    #endregion
                    #region X
                    m = Matrix4.CreateRotationY((float)Math.PI / 2) * Matrix4.CreateScale(.25f, 1, .25f) * Matrix4.CreateTranslation(new Vector3(gridQuadrantSize + .5f, 0, 0));

                    GL.UniformMatrix4(mLoc, false, ref m);
                    GL.Uniform4(cLoc, new Vector4(0.4f, 0.4f, 1.0f, 1.0f));

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, xLetterIB);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, xLetterVB);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);
                    GL.EnableVertexAttribArray(0);
                    GL.DrawElements(PrimitiveType.Triangles, 12, DrawElementsType.UnsignedInt, 0);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    #endregion
                    #endregion

                    //reset uniforms
                    m = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
                    GL.UniformMatrix4(mLoc, false, ref m);
                    GL.Uniform4(cLoc, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));

                    GL.UseProgram(0);
                }
            }
            public class InputKey
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
        }

        public class Mesh
        {
            //transform
            private Matrix4 modelMatrix;
            private Vector3 position = Vector3.Zero;
            private Quaternion rotation = Quaternion.Identity;
            private Vector3 scale = Vector3.One;
            public void Move(float x, float y, float z)
            {
                position += new Vector3(x,y,z);
                UpdateModelMatrix();
            }
            public void Rotate(float yaw, float pitch, float roll)
            {
                rotation *= Quaternion.FromEulerAngles(pitch, yaw, roll);
                UpdateModelMatrix();
            }
            public void SetScale(float x, float y, float z)
            {
                scale = new Vector3(x, y, z);
            }
            private void UpdateModelMatrix()
            {
                modelMatrix = Matrix4.CreateTranslation(position);
                modelMatrix *= Matrix4.CreateFromQuaternion(rotation);
                modelMatrix *= Matrix4.CreateScale(scale);
            }

            //data
            public struct Vertex
            {
                public SystemHalf.Half x, y, z;
                public SystemHalf.Half u, v;
                public float nx, ny, nz;
                public byte b1, b2, b3, b4;
                public byte w1, w2, w3, w4;
            }
            public List<Vertex> vertices;
            public List<uint> indices;
            public int
                materialID = -1,
                boneID,
                vertexSize,
                faceCount;
            public bool isSkinned;
            public string name;

            //OGL
            int vb, ib, indexCount;
            int mLoc, cLoc;
            static int meshShader;
            static bool shaderInit = false;
            Vector3 color;
            public void InitDrawing()
            {
                #region Mesh Shader
                if (!shaderInit)
                {
                    #region Shader Strings
                    //////////////////////////////////////////////////////
                    string vs =
        @"#version 420 core

layout (location = 0) in vec3 positionIn;
layout (location = 1) in vec2 uvIn;
layout (location = 2) in vec3 normalIn;

layout(std140, binding = 0) uniform GlobalMatrices
{
    mat4 projectionMatrix;
    mat4 viewMatrix;
};
uniform mat4 modelMatrix;

out VS_OUT {
    vec2 uv;
    vec3 normal; 
} vs_out;

void main()
{
    vs_out.uv = uvIn;
    vs_out.normal = normalIn;
    gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(positionIn, 1.0);
}";
                    string gs =
        @"#version 420 core
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

layout(std140, binding = 0) uniform GlobalMatrices
{
    mat4 projectionMatrix;
    mat4 viewMatrix;
};
uniform mat4 modelMatrix;

in VS_OUT {
    vec2 uv;
    vec3 normal; 
} vs_out[];

out GS_OUT {
vec2 uv;
vec3 normal; 
float lightStrength;
} gs_out;

void main()
{
    vec3 a = gl_in[0].gl_Position.xyz;
    vec3 b = gl_in[1].gl_Position.xyz;
    vec3 c = gl_in[2].gl_Position.xyz;
    vec3 normal = normalize(cross(a-b, b-c));
    vec3 lightDir = vec3(0, 0, -1); //out from the camera.
    gs_out.lightStrength = max(0, dot(normalize(normal), lightDir));

    gl_Position = gl_in[0].gl_Position;
    gs_out.uv = vs_out[0].uv;
    gs_out.normal = vs_out[0].normal;
    EmitVertex();
    gl_Position = gl_in[1].gl_Position;
    gs_out.uv = vs_out[1].uv;
    gs_out.normal = vs_out[1].normal;
    EmitVertex();
    gl_Position = gl_in[2].gl_Position;
    gs_out.uv = vs_out[2].uv;
    gs_out.normal = vs_out[2].normal;
    EmitVertex();
    EndPrimitive();
}
";
                    //////////////////////////////////////////////////////
                    string fs =
        @"#version 420 core

in GS_OUT {
vec2 uv;
vec3 normal; 
float lightStrength;
} gs_out;

uniform vec4 color;
uniform sampler2D textureSampler;
out vec4 FragColor;

void main()
{
    FragColor = texture(textureSampler, gs_out.uv) * (color * gs_out.lightStrength);
}";
                    //////////////////////////////////////////////////////
                    #endregion

                    int vao = GL.GenVertexArray();
                    GL.BindVertexArray(vao);

                    int vertS, geomS, fragS;
                    vertS = GL.CreateShader(ShaderType.VertexShader);
                    GL.ShaderSource(vertS, vs);
                    geomS = GL.CreateShader(ShaderType.GeometryShader);
                    GL.ShaderSource(geomS, gs);
                    fragS = GL.CreateShader(ShaderType.FragmentShader);
                    GL.ShaderSource(fragS, fs);

                    GL.CompileShader(vertS);
                    string infoLogVert = GL.GetShaderInfoLog(vertS);
                    if (infoLogVert != System.String.Empty)
                        System.Console.WriteLine(infoLogVert);
                    GL.CompileShader(geomS);
                    string infoLogGeom = GL.GetShaderInfoLog(geomS);
                    if (infoLogGeom != System.String.Empty)
                        System.Console.WriteLine(infoLogGeom);
                    GL.CompileShader(fragS);
                    string infoLogFrag = GL.GetShaderInfoLog(fragS);
                    if (infoLogFrag != System.String.Empty)
                        System.Console.WriteLine(infoLogFrag);

                    meshShader = GL.CreateProgram();
                    GL.AttachShader(meshShader, vertS);
                    GL.AttachShader(meshShader, fragS);
                    GL.AttachShader(meshShader, geomS);
                    GL.LinkProgram(meshShader);
                    GL.UseProgram(meshShader);

                    shaderInit = true;
                }
                #endregion

                mLoc = GL.GetUniformLocation(meshShader, "modelMatrix");
                cLoc = GL.GetUniformLocation(meshShader, "color");
                GL.UniformBlockBinding(meshShader, GL.GetUniformBlockIndex(meshShader, "GlobalMatrices"), 0);
                
                vb = GL.GenBuffer();
                ib = GL.GenBuffer();
                
                GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
                GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vertex()) * vertices.Count, vertices.ToArray(), BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);
                GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Count, indices.ToArray(), BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                indexCount = indices.Count;

                UpdateModelMatrix();
                color = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            }
            public void Draw()
            {
                GL.UseProgram(meshShader);
                
                GL.UniformMatrix4(mLoc, false, ref modelMatrix);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.HalfFloat, false, Marshal.SizeOf(new Mesh.Vertex()), 0);  //pos
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.HalfFloat, false, Marshal.SizeOf(new Mesh.Vertex()), Marshal.SizeOf(new SystemHalf.Half()) * 3);  //uv
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Mesh.Vertex()), Marshal.SizeOf(new SystemHalf.Half()) * 5); //normal
                GL.EnableVertexAttribArray(2);

                if (materialID > -1)
                {
                    GL.BindTexture(TextureTarget.Texture2D, editor.materialDiffuseTextures[materialID]);
                    GL.Uniform4(cLoc, new Vector4(1, 1, 1, 1));
                }
                if (materialID == -1)
                {
                    GL.BindTexture(TextureTarget.Texture2D, editor.nullTexture);
                    GL.Uniform4(cLoc, new Vector4(color, 1));
                }

                GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.UseProgram(0);
            }

            //misc
            static Random random = new Random();
        }

        //public struct Model
        //{
        //    public struct Mesh
        //    {
        //        public struct Vertex
        //        {
        //            public SystemHalf.Half  vx, vy, vz,
        //                                    tu, tv;
        //            public float            nx, ny, nz;
        //            public byte             b1, b2, b3, b4,
        //                                    w1, w2, w3, w4;
        //        }

        //        public List<Vertex> vertices;
        //        public List<int> indices;
        //        public int
        //            materialID,
        //            boneID,
        //            vertexSize,
        //            faceCount;
        //        public bool isSkinned;
        //        public string name;
        //    }
        //    public List<Mesh> meshes;
        //}
        //
        //class OGLMesh
        //{
        //    //transform
        //    public Vector3 position = new Vector3(0, 0, 0);
        //    public Quaternion rotation = Quaternion.Identity;
        //    public Vector3 scale = Vector3.One;

        //    public void Rotate(float yaw, float pitch, float roll)
        //    {
        //        rotation *= Quaternion.FromEulerAngles(pitch, yaw, roll);
        //    }
        //    public void Translate(float x, float y, float z)
        //    {
        //        position += new Vector3(x, y, z);
        //    }
        //    public Model.Mesh GetTranslatedMesh()
        //    {
        //        //TODO: Implement this function to create a model from this OGLMesh that takes into account its translation.
        //        return new Model.Mesh();
        //    }

        //    int cLoc;
        //    public OGLMesh(Model.Mesh mesh)
        //    {
        //        cLoc = GL.GetUniformLocation(gui._3dEditor.globalShader, "color");
        //        Console.WriteLine(cLoc);
        //        Init(mesh);
        //        InitDebug(mesh);
        //    }

        //    public int vbuffer, ibuffer, indexCount;
        //    void Init(Model.Mesh mesh)
        //    {
        //        vbuffer = GL.GenBuffer();
        //        ibuffer = GL.GenBuffer();

        //        indexCount = mesh.indices.Count;

        //        GL.BindBuffer(BufferTarget.ArrayBuffer, vbuffer);
        //        GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Model.Mesh.Vertex()) * mesh.vertices.Count, mesh.vertices.ToArray(), BufferUsageHint.StaticDraw);
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibuffer);
        //        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * mesh.indices.Count, mesh.indices.ToArray(), BufferUsageHint.StaticDraw);
        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        //    }
        //    public void Draw()
        //    {
        //        Matrix4 m = Matrix4.CreateTranslation(position);
        //        int loc = GL.GetUniformLocation(gui._3dEditor.globalShader, "modelMatrix");
        //        GL.UniformMatrix4(loc, false, ref m);

        //        GL.BindBuffer(BufferTarget.ArrayBuffer, vbuffer);

        //        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.HalfFloat, false, Marshal.SizeOf(new Model.Mesh.Vertex()), 0);  //pos
        //        GL.EnableVertexAttribArray(0);
        //        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.HalfFloat, false, Marshal.SizeOf(new Model.Mesh.Vertex()), Marshal.SizeOf(new SystemHalf.Half()) * 3);  //uv
        //        GL.EnableVertexAttribArray(1);
        //        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Model.Mesh.Vertex()), Marshal.SizeOf(new SystemHalf.Half()) * 5); //normal
        //        GL.EnableVertexAttribArray(2);

        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibuffer);

        //        GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);

        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    }

        //    public int dvbuffer, dibuffer;
        //    public int vertexNormalIndexCount, faceNormalIndexCount;
        //    void InitDebug(Model.Mesh mesh)
        //    {
        //        dvbuffer = GL.GenBuffer();
        //        dibuffer = GL.GenBuffer();

        //        List<Vector3> vertices = new List<Vector3>();
        //        List<int> indices = new List<int>();

        //        int i = 0;
        //        foreach (Model.Mesh.Vertex v in mesh.vertices)
        //        {
        //            vertices.Add(new Vector3(v.vx, v.vy, v.vz));
        //            vertices.Add(new Vector3(v.vx + v.nx, v.vy + v.ny, v.vz + v.nz));
        //            indices.Add(i);
        //            i++;
        //            indices.Add(i);
        //            i++;
        //        }
        //        vertexNormalIndexCount = i;
        //        for (int j = 0; j < mesh.indices.Count; j += 3)
        //        {
        //            Vector3 v1 = new Vector3(mesh.vertices[j].vx, mesh.vertices[j].vy, mesh.vertices[j].vz);
        //            Vector3 v2 = new Vector3(mesh.vertices[j + 1].vx, mesh.vertices[j + 1].vy, mesh.vertices[j + 1].vz);
        //            Vector3 v3 = new Vector3(mesh.vertices[j + 2].vx, mesh.vertices[j + 2].vy, mesh.vertices[j + 2].vz);

        //            Vector3 a = v2 - v1;
        //            Vector3 b = v3 - v1;

        //            Vector3 n = Vector3.Cross(a, b);

        //            vertices.Add(new Vector3(mesh.vertices[j].vx, mesh.vertices[j].vy, mesh.vertices[j].vz));
        //            vertices.Add(n);
        //            indices.Add(i);
        //            i++;
        //            indices.Add(i);
        //            i++;
        //        }
        //        faceNormalIndexCount = i - vertexNormalIndexCount;

        //        GL.BindBuffer(BufferTarget.ArrayBuffer, dvbuffer);
        //        GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * vertices.Count, vertices.ToArray(), BufferUsageHint.StaticDraw);
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, dibuffer);
        //        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Count, indices.ToArray(), BufferUsageHint.StaticDraw);
        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        //    }
        //    public void DrawDebug()
        //    {
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, dvbuffer);
        //        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);  //pos
        //        GL.EnableVertexAttribArray(0);
        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, dibuffer);

        //        GL.Uniform4(cLoc, new Vector4(0f, 1.0f, 0f, 1.0f));
        //        //GL.DrawElements(PrimitiveType.Lines, vertexNormalIndexCount, DrawElementsType.UnsignedInt, 0);

        //        GL.Uniform4(cLoc, new Vector4(0f, 0f, 1.0f, 1.0f));
        //        //GL.DrawElements(PrimitiveType.Lines, faceNormalIndexCount, DrawElementsType.UnsignedInt, vertexNormalIndexCount);

        //        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        //        GL.Uniform4(cLoc, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
        //    }

        //}
    }
}
