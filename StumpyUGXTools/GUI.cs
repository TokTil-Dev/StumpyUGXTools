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
using System.Threading;
using System.Runtime.InteropServices;

namespace StumpyUGXTools
{
    class GUI : Form
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
        private ToolStripMenuItem saveToolStripMenuItem;

        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
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
            this.openUGXToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openUGXToolStripMenuItem.Text = "Open UGX";
            this.openUGXToolStripMenuItem.Click += new System.EventHandler(this.openUGXToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fBXToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            this.keyBindingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.keyBindingsToolStripMenuItem.Text = "Key Bindings";
            // 
            // GUI
            // 
            this.ClientSize = new System.Drawing.Size(1450, 720);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.editorSelecter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "GUI";
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

        public Editor _3dEditor;
        public MeshEditor meshEditorTab;
        public MaterialEditor materialEditorTab;
        #region UI
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
            _3dEditor.InitViewport();

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
            _3dEditor = new Editor();
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
        #endregion

        #region Loading/Saving
        string openFilePath;
        bool ugxLoaded = false;
        int DoUGXLoad(string path)
        {
            if (File.Exists(path))
            {
                if (ugx.Load(path) == -1)
                {
                    DoUGXUnload();
                    gui.LogOut("Invalid or corrupt file: " + path);
                    return -1;
                }
                materialEditorTab.Unload();
                ugx.fileName = Path.GetFileNameWithoutExtension(path);
                ugx.InitTextureEditing();
                ugx.InitMeshEditing();
                gui.LogOut("UGX Loaded: " + path);
                openFilePath = path;
                ugxLoaded = true;
                DoUI();
                return 1;
            }
            else
            {
                DoUGXUnload();
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
        public Model ImportAsset(string path)
        {
            //use assimp to load a model.
            AssimpContext imp = new AssimpContext();
            Assimp.Configs.RemoveDegeneratePrimitivesConfig c = new Assimp.Configs.RemoveDegeneratePrimitivesConfig(false);
            imp.SetConfig(c);
            Scene asset = imp.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FindDegenerates);

            Model model = new Model();
            model.meshes = new List<Model.Mesh>();
            //for each mesh in the imported file, collect vertices and indices.
            for (int i = 0; i < asset.MeshCount; i++)
            {
                Model.Mesh mesh = new Model.Mesh();
                mesh.vertices = new List<Model.Mesh.Vertex>();
                mesh.indices = new List<int>();
                mesh.isSkinned = false;
                mesh.vertexSize = 24;
                mesh.name = asset.Meshes[i].Name;

                //collet vertices for this mesh
                for (int ve = 0; ve < asset.Meshes[i].VertexCount; ve++)
                {
                    Model.Mesh.Vertex v = new Model.Mesh.Vertex();
                    v.vx = (SystemHalf.Half)asset.Meshes[i].Vertices[ve].X;
                    v.vy = (SystemHalf.Half)asset.Meshes[i].Vertices[ve].Y;
                    v.vz = (SystemHalf.Half)asset.Meshes[i].Vertices[ve].Z;
                    v.nx = (SystemHalf.Half)asset.Meshes[i].Normals[ve].X;
                    v.ny = (SystemHalf.Half)asset.Meshes[i].Normals[ve].Y;
                    v.nz = (SystemHalf.Half)asset.Meshes[i].Normals[ve].Z;
                    v.tu = (SystemHalf.Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].X;
                    v.tv = (SystemHalf.Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].Y;
                    mesh.vertices.Add(v);
                }
                //collect indices for this mesh
                for (int j = 0; j < asset.Meshes[i].FaceCount; j++)
                {
                    for (int k = 0; k < asset.Meshes[i].Faces[j].IndexCount; k++)
                    {
                        mesh.indices.Add(asset.Meshes[i].Faces[j].Indices[k]);
                    }
                    mesh.faceCount++;
                }

                model.meshes.Add(mesh);
            }

            return model;
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
            _3dEditor.FBXImportPrompt();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoUGXSave();
        }
        #endregion

    }


    enum AttribType { FLOAT, UINT8, UINT16, UINT32 }
    class PathBox
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
            name.Location = new Point(gui.EditorToolsLeftMargin - 1, initOffset + (offset * space));
            name.Name = nameStr + "_name";
            name.Size = new Size(100, 13);
            name.TabIndex = offset;
            name.Text = nameStr;

            value.Location = new Point(gui.EditorToolsLeftMargin, initOffset + 13 + (offset * space));
            value.Name = nameStr + "_value";
            value.Size = new Size(gui.EditorToolSideWindowWidth - 24 - 90, 20);
            value.TabIndex = offset + 1;
            value.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            value.TextChanged += new EventHandler(PathUpdated);

            buttonRevert.Location = new Point(gui.EditorToolsLeftMargin + gui.EditorToolSideWindowWidth - 22, initOffset + 13 + (offset * space));
            buttonRevert.Size = new Size(20, 20);
            buttonRevert.Text = "↶";
            buttonRevert.Font = new Font("Microsoft Sans Serif", 10F);
            buttonRevert.Click += new EventHandler(RevertButtonPress);
            buttonRevert.Anchor = AnchorStyles.Right | AnchorStyles.Top;

            nameUVW.BorderStyle = BorderStyle.None;
            nameUVW.Location = new Point(gui.EditorToolsLeftMargin + value.Width, initOffset + (offset * space));
            nameUVW.Name = nameStr + "_nameUVW";
            nameUVW.Size = new Size(100, 13);
            nameUVW.TabIndex = offset;
            nameUVW.Text = "UVW Velocity";
            nameUVW.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            U.Location = new Point(gui.EditorToolsLeftMargin + value.Width - 1, initOffset + 13 + (offset * space));
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

            gui.toolTips.SetToolTip(buttonRevert, "Revert to default value.");
            gui.toolTips.SetToolTip(buttonRevertUVW, "Revert to default value.");
            gui.toolTips.SetToolTip(buttonRemove, "Remove this texture map from this material.");
            gui.toolTips.SetToolTip(buttonAdd, "Add a new texture map to this material.");

            Update(false);
        }
        public void Update(bool hasValue)
        {
            if (hasValue)
            {
                gui.Controls.Add(value);
                gui.Controls.Add(buttonRevert);
                //gui.Controls.Add(buttonRemove); //removed for now
                //gui.Controls.Remove(buttonAdd); //removed for now
                gui.Controls.Add(nameUVW);
                gui.Controls.Add(U);
                gui.Controls.Add(V);
                gui.Controls.Add(W);
                gui.Controls.Add(buttonRevertUVW);
            }
            if (!hasValue)
            {
                gui.Controls.Remove(value);
                gui.Controls.Remove(buttonRevert);
                //gui.Controls.Remove(buttonRemove); //removed for now
                //gui.Controls.Add(buttonAdd);       //removed for now.
                gui.Controls.Remove(nameUVW);
                gui.Controls.Remove(U);
                gui.Controls.Remove(V);
                gui.Controls.Remove(W);
                gui.Controls.Remove(buttonRevertUVW);
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
            gui.Controls.Add(name);
        }
        public void DisableView()
        {
            gui.Controls.Remove(value);
            gui.Controls.Remove(name);
            gui.Controls.Remove(buttonAdd);
            gui.Controls.Remove(buttonRemove);
            gui.Controls.Remove(buttonRevert);
            gui.Controls.Remove(nameUVW);
            gui.Controls.Remove(U);
            gui.Controls.Remove(V);
            gui.Controls.Remove(W);
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
    class AttribBox
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

            value.Location = new Point(gui.EditorToolsLeftMargin + (gridX * xSpace), 645 + (gridY * ySpace));
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
            name.Location = new Point(gui.EditorToolsLeftMargin + (gridX * xSpace) - 2, 632 + (gridY * ySpace));
            name.Name = nameStr + "_name";
            name.Size = new Size(buttonRevert.Width + 8 + value.Width, 13);
            name.TabIndex = ind;
            name.Text = nameStr;

            gui.toolTips.SetToolTip(buttonRevert, "Revert to default value.");
            gui.toolTips.SetToolTip(name, name.Text);
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
            gui.Controls.Add(value);
            gui.Controls.Add(buttonRevert);
            gui.Controls.Add(name);
        }
        public void DisableView()
        {
            gui.Controls.Remove(value);
            gui.Controls.Remove(buttonRevert);
            gui.Controls.Remove(name);
        }

        AttribType t;
        public TextBox value = new TextBox();
        Button buttonRevert = new Button();
        Label name = new Label();

    }
    class MatData
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

    class EditorTab
    {
        protected TabPage tab = new TabPage();
        public EditorTab()
        {
            gui.editorSelecter.Controls.Add(tab);
        }

        public virtual void Show()
        {

        }
        public virtual void Hide()
        {

        }
    }
    class MeshViewWindow
    {
        private bool canEdit;
        private OpenTK.GLControl window;
        public MeshViewWindow(int xPos, int yPos, int width, int height, bool canBeEdited)
        {
            canEdit = canBeEdited;
            window = new OpenTK.GLControl();
            window.Location = new Point(xPos, yPos);
            window.Size = new Size(width, height);
            window.Paint += new PaintEventHandler(Paint);
            gui.Controls.Add(window);
        }
        private void Paint(object o, EventArgs e)
        {
            window.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Draw();
            window.SwapBuffers();
        }
        public virtual void Draw()
        {

        }
    }

    class MeshEditor : EditorTab
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
            ugxMeshTree.Location = new Point(gui.EditorToolsLeftMargin, 118);
            ugxMeshTree.Name = "ugxMeshList";
            ugxMeshTree.Size = new Size((gui.EditorToolSideWindowWidth / 2) - (xSpace), 500);
            ugxMeshTree.TabIndex = 11;

            fbxMeshTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            fbxMeshTree.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            fbxMeshTree.ItemHeight = 15;
            fbxMeshTree.Location = new Point(gui.EditorToolsLeftMargin + (gui.EditorToolSideWindowWidth / 2) + (xSpace), 118);
            fbxMeshTree.Name = "fbxMeshList";
            fbxMeshTree.Size = new Size((gui.EditorToolSideWindowWidth / 2) - (xSpace), 500);
            fbxMeshTree.TabIndex = 13;
        }
        public override void Show()
        {
            gui.Controls.Add(ugxMeshTree);
            gui.Controls.Add(fbxMeshTree);
        }
        public override void Hide()
        {
            gui.Controls.Remove(ugxMeshTree);
            gui.Controls.Remove(fbxMeshTree);
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

    }
    class MaterialEditor : EditorTab
    {

        public TabControl materialSelector = new TabControl();

        AttribBox[] attribBoxes = new AttribBox[12];
        PathBox[] pathBoxes = new PathBox[13];
        public List<MatData> matData = new List<MatData>();

        public MaterialEditor() : base()
        {
            tab.Text = "Material Editor";

            this.materialSelector.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.materialSelector.Location = new Point(gui.EditorToolsLeftMargin, 50);
            this.materialSelector.Name = "materialSelector";
            this.materialSelector.SelectedIndex = -1;
            this.materialSelector.Size = new Size(gui.EditorToolSideWindowWidth, 23);
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
            gui.Controls.Add(materialSelector);
            foreach (PathBox b in pathBoxes)
            {
                b.EnableView();
            }
            foreach (AttribBox a in attribBoxes)
            {
                a.EnableView();
            }
        }
        public override void Hide()
        {
            gui.Controls.Remove(materialSelector);
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
            gui.LogOut("Found " + matData.Count + " materials.");
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
        public void Unload()
        {
            foreach(MatData m in matData)
            {
                materialSelector.Controls.Remove(m.tab);
            }
            matData.Clear();
        }
    }
}