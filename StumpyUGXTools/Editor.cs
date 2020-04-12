﻿using System;
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
        private Label versionLabel;
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
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureReferencePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyBindingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionLabel = new System.Windows.Forms.Label();
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
            // versionLabel
            // 
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(1397, 9);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.versionLabel.Size = new System.Drawing.Size(41, 15);
            this.versionLabel.TabIndex = 10;
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Editor
            // 
            this.ClientSize = new System.Drawing.Size(1450, 720);
            this.Controls.Add(this.versionLabel);
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
        List<Mesh> UGXmeshes = new List<Mesh>();
        List<Bone> UGXbones = new List<Bone>();
        List<uint> materialDiffuseTextures = new List<uint>();
        uint nullTexture = 65535;

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
        /////// WinForms Functions
        void GUIInit(object o, EventArgs e)
        {
            versionLabel.Text = Program.version;
            EditorToolSideWindowWidth = 400;
            EditorToolsLeftMargin = 11;
            meshEditorTab = new MeshEditor();
            materialEditorTab = new MaterialEditor();
            viewport = new Viewport();
            viewport.InitViewport();
            Controls.Add(editorSelecter);
            DoUGXLoad("F:\\HaloWarsModding\\HaloWarsDE\\Extract\\art\\unsc\\infantry\\marine_01\\marine_01.ugx");
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
        bool ugxLoaded = false;
        int  DoUGXLoad(string path)
        {
            if (File.Exists(path))
            {
                UGXFile f = new UGXFile();
                if (f.Load(path) == -1) { return -1; }
                else
                {
                    UGXmeshes.Clear();
                    UGXbones.Clear();
                    materialEditorTab.Clear();
                    meshEditorTab.ClearUGXTree();
                    f.fileName = Path.GetFileNameWithoutExtension(path);
                    f.InitTextureEditing();
                    f.InitMeshEditing();
                    f.filePath = path;
                    UGXmeshes.AddRange(f.GetMeshes());
                    foreach (Mesh m in UGXmeshes) { m.InitDrawing(); }
                    UGXbones.AddRange(f.GetBones());
                    foreach(Bone b in UGXbones) { b.InitDrawing(); }
                    ugx = f;
                    ugxLoaded = true;

                    LogOut("UGX Loaded: " + path);
                    editorSelecter.SelectedIndex = -1;
                    editorSelecter.SelectedIndex = 0;
                    materialEditorTab.SetupPathView();
                    meshEditorTab.PopulateUGXTree();
                    LoadDiffuseTextures();
                    viewport.viewport.Invalidate();
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
            ugx.filePath = "";
        }
        void DoUGXSave()
        {
            if (!ugxLoaded) return;
            materialEditorTab.SaveMaterial();
            ugx.SaveNewMaterial();
            ugx.Save(ugx.filePath);
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
            foreach(int i in materialDiffuseTextures)
            {
                GL.DeleteTexture(i);
            }
            materialDiffuseTextures.Clear();
            for (int i = 0; i < materialEditorTab.matData.Count; i++)
            {
                uint name = 65535;
                TextureTarget t;
                if (materialEditorTab.matData[i].hasValue[0])
                {
                    try
                    {
                        if (File.Exists(textureReferencePath + materialEditorTab.matData[i].pathStrings[0] + ".ddx"))
                        {
                            ImageDDS.LoadFromDisk(textureReferencePath + materialEditorTab.matData[i].pathStrings[0] + ".ddx", out name, out t);
                        }
                        if (File.Exists(textureReferencePath + materialEditorTab.matData[i].pathStrings[0] + ".dds"))
                        {
                            ImageDDS.LoadFromDisk(textureReferencePath + materialEditorTab.matData[i].pathStrings[0] + ".dds", out name, out t);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Could not load DDX/S at " + textureReferencePath + materialEditorTab.matData[i].pathStrings[0]);
                    }
                }
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
                MeshEditor.ImportRootNode rn = new MeshEditor.ImportRootNode();
                rn.SetName(Path.GetFileNameWithoutExtension(fbd.FileName));

                foreach(Mesh mesh in m)
                {
                    mesh.InitDrawing();

                    MeshEditor.ImportMeshNode mn = new MeshEditor.ImportMeshNode();
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
            public TreeView ugxBoneTree = new TreeView();
            public TreeView fbxMeshTree = new TreeView();

            public MeshEditor() : base()
            {
                int xSpace = 5;
                tab.Text = "Mesh Editor";

                ugxMeshTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                ugxMeshTree.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
                ugxMeshTree.ItemHeight = 15;
                ugxMeshTree.Location = new Point(editor.EditorToolsLeftMargin, 50);
                ugxMeshTree.Name = "ugxMeshList";
                ugxMeshTree.Size = new Size((editor.EditorToolSideWindowWidth / 2) - (xSpace), editor.Height - 501);
                ugxMeshTree.TabIndex = 11;
                ugxMeshTree.HideSelection = false;
                ugxMeshTree.DrawMode = TreeViewDrawMode.OwnerDrawText;
                ugxMeshTree.DrawNode += new DrawTreeNodeEventHandler(DrawNode);
                ugxMeshTree.BeforeSelect += new TreeViewCancelEventHandler(BeforeUGXNodeSelect);
                ugxMeshTree.AfterSelect += new TreeViewEventHandler(AfterUGXNodeSelect);

                ugxBoneTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                ugxBoneTree.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
                ugxBoneTree.ItemHeight = 15;
                ugxBoneTree.Location = new Point(editor.EditorToolsLeftMargin, 50 + ugxMeshTree.Height + 9);
                ugxBoneTree.Name = "ugxBoneList";
                ugxBoneTree.Size = new Size((editor.EditorToolSideWindowWidth / 2) - (xSpace), 200);
                ugxBoneTree.TabIndex = 11;
                ugxBoneTree.HideSelection = false;
                ugxBoneTree.DrawMode = TreeViewDrawMode.OwnerDrawText;
                
                fbxMeshTree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                fbxMeshTree.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                fbxMeshTree.ItemHeight = 15;
                fbxMeshTree.Location = new Point(editor.EditorToolsLeftMargin + (editor.EditorToolSideWindowWidth / 2) + (xSpace), 50);
                fbxMeshTree.Name = "fbxMeshList";
                fbxMeshTree.Size = new Size((editor.EditorToolSideWindowWidth / 2) - (xSpace), editor.Height - 301 + 9);
                fbxMeshTree.TabIndex = 13;
                fbxMeshTree.HideSelection = false;
            }
            public override void Show()
            {
                editor.Controls.Add(ugxMeshTree);
                editor.Controls.Add(ugxBoneTree);
                editor.Controls.Add(fbxMeshTree);
                editor.Controls.Add(editor.viewport.viewport);
            }
            public override void Hide()
            {
                editor.Controls.Remove(ugxMeshTree);
                editor.Controls.Remove(ugxBoneTree);
                editor.Controls.Remove(fbxMeshTree);
            }

            public class ImportRootNode
            {
                public void AddChild(ImportMeshNode mn)
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
                private List<ImportMeshNode> children = new List<ImportMeshNode>();
            }
            public class ImportMeshNode
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem importedMeshMenuItem = new ToolStripMenuItem();
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem();
                public ImportMeshNode()
                {
                    cms.Items.Add(importedMeshMenuItem);
                    importedMeshMenuItem.Click += new EventHandler(showMeshMenu);
                    importedMeshMenuItem.Text = "Mesh Settings";
                    treeNode.ContextMenuStrip = cms;
                }
                void showMeshMenu(object o, EventArgs e)
                {
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
            public class UGXRootNode
            {
                public void AddChild(UGXMeshNode mn)
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
                    treeNode.Checked = true;
                    tv.Nodes.Add(treeNode);
                    treeNode.Expand();
                }
                private TreeNode treeNode = new TreeNode();
                public List<UGXMeshNode> children = new List<UGXMeshNode>();
            }
            public class UGXMeshNode
            {
                public void SetParent(TreeNode t)
                {
                    t.Nodes.Add(treeNode);
                }

                public void SetName(string s)
                {
                    name = s;
                    UpdateName();
                }
                public void SetNameSuffix(string s)
                {
                    suffix = s;
                    UpdateName();
                }
                public void SetNamePrefix(string s)
                {
                    prefix = s;
                    UpdateName();
                }
                private void UpdateName()
                {
                    string newName = "";
                    if(prefix != "") { newName += "(" + prefix + ") "; }
                    newName += name;
                    if(suffix != "") { newName += " (" + suffix + ")"; }
                    treeNode.Text = newName;
                }

                public string name = "", suffix = "", prefix = "";
                private TreeNode treeNode = new TreeNode();
                public int meshIndex = -1;
            }
            
            void DrawNode(object o, DrawTreeNodeEventArgs e)
            {
                e.DrawDefault = true;
            }

            UGXRootNode rn;
            void BeforeUGXNodeSelect(object o, TreeViewCancelEventArgs e)
            {
                if (ugxMeshTree.SelectedNode != null) ugxMeshTree.SelectedNode.ForeColor = SystemColors.WindowText;
                foreach (Mesh m in editor.imports)
                {
                    m.isHighlighted = false;
                }
            }
            void AfterUGXNodeSelect(object o, TreeViewEventArgs e)
            {
                e.Node.ForeColor = SystemColors.HighlightText;
                for(int i = 0; i < rn.children.Count; i++)
                {
                    if (i == ugxMeshTree.SelectedNode.Index) editor.UGXmeshes[i].isHighlighted = true;
                    if (i != ugxMeshTree.SelectedNode.Index || ugxMeshTree.SelectedNode.Parent == null) editor.UGXmeshes[i].isHighlighted = false;
                }
                editor.viewport.viewport.Invalidate();
            }

            public void PopulateUGXTree()
            {
                if (editor.ugxLoaded == false) return;
                rn = new UGXRootNode();
                foreach(Mesh m in editor.UGXmeshes)
                {
                    UGXMeshNode n = new UGXMeshNode();
                    n.SetName("Mesh " + (editor.UGXmeshes.IndexOf(m) + 1));
                    rn.AddChild(n);
                }
                rn.SetName(ugx.fileName);
                rn.AddToTree(ugxMeshTree);
            }
            public void ClearUGXTree()
            {
                ugxMeshTree.Nodes.Clear();
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
            Matrix4 projectionMatrix;

            public int UBO;
            public int selectionFBO, selColorAtt, selDepthAtt;
            public int highlightFBO, hglColorAtt, hglDepthAtt;

            public void InitViewport()
            {
                viewport = new OpenTK.GLControl();
                viewport.Location = new Point((editor.EditorToolsLeftMargin * 2) + editor.EditorToolSideWindowWidth, 44);
                viewport.Size = new Size(editor.Width - 50 - editor.EditorToolSideWindowWidth, editor.Height - 95);
                viewport.KeyDown += new KeyEventHandler(viewport_KeyDown);
                viewport.KeyUp += new KeyEventHandler(viewport_KeyUp);
                viewport.MouseClick += new MouseEventHandler(viewport_MouseClicked);
                viewport.LostFocus += new EventHandler((object o, EventArgs e) =>
                {
                    fauxFocus = false;
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
            void InitOGL()
            {
                TextureTarget t;
                byte[] b;
                using (var streamReader = new MemoryStream())
                {
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("StumpyUGXTools.null.dds").CopyTo(streamReader);
                    b = streamReader.ToArray();
                }
                ImageDDS.LoadFromDisk(b, out editor.nullTexture, out t);

                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.525f, viewport.Width / viewport.Height, .01f, 2500f);

                UBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.UniformBuffer, UBO);
                GL.BufferData(BufferTarget.UniformBuffer, Marshal.SizeOf(new Matrix4()) * 2, IntPtr.Zero, BufferUsageHint.StreamDraw);
                GL.BindBuffer(BufferTarget.UniformBuffer, 0);

                GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, UBO);

                GL.BindBuffer(BufferTarget.UniformBuffer, UBO);
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, Marshal.SizeOf(new Matrix4()), ref projectionMatrix);
                GL.BindBuffer(BufferTarget.UniformBuffer, 0);

                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.StencilTest);

                selectionFBO = GL.GenFramebuffer();
                selColorAtt = GL.GenTexture();
                selDepthAtt = GL.GenRenderbuffer();
                GL.Enable(EnableCap.Texture2D);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, selectionFBO);
                GL.BindTexture(TextureTarget.Texture2D, selColorAtt);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, viewport.Width, viewport.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, selColorAtt, 0);

                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, selDepthAtt);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, viewport.Width, viewport.Height);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, selDepthAtt);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);


                highlightFBO = GL.GenFramebuffer();
                hglColorAtt = GL.GenTexture();
                hglDepthAtt = GL.GenRenderbuffer();
                GL.Enable(EnableCap.Texture2D);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, selectionFBO);
                GL.BindTexture(TextureTarget.Texture2D, hglColorAtt);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, viewport.Width, viewport.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, hglColorAtt, 0);

                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, hglDepthAtt);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, viewport.Width, viewport.Height);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, hglDepthAtt);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }
            
            //viewport functions
            void viewport_Loop()
            { 
                PollInputs();
                camera.UpdateCamera();
                GL.ClearColor(.3f, .3f, .3f, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                //draw normal
                grid.DrawGrid();
                foreach (Mesh m in editor.UGXmeshes)
                {
                   m.Draw();
                }
                foreach (Mesh m in editor.imports)
                {
                    m.Draw();
                }

                GL.Disable(EnableCap.DepthTest);
                foreach (Bone b in editor.UGXbones)
                {
                    b.Draw();
                }
                GL.Enable(EnableCap.DepthTest);

                viewport.SwapBuffers();


                //draw color id to selection buffer
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, selectionFBO);
                GL.ClearColor(0f, 0f, 0f, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                foreach (Mesh m in editor.UGXmeshes)
                {
                    m.DrawFlat();
                }
                foreach (Mesh m in editor.imports)
                {
                    m.DrawFlat();
                }
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
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
            bool fauxFocus = true;
            void viewport_MouseClicked(object o, System.Windows.Forms.MouseEventArgs e)
            {
                if (!fauxFocus) { fauxFocus = true; return; }
                int pixelData = 0;
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, selectionFBO);
                GL.ReadPixels(e.X, viewport.Height - e.Y, 1, 1, PixelFormat.Bgra, PixelType.UnsignedByte, ref pixelData);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                byte r = Color.FromArgb(pixelData).R;
                byte g = Color.FromArgb(pixelData).G;
                byte b = Color.FromArgb(pixelData).B;

                if (r == 0 && g == 0 && b == 0)
                {
                    foreach (Mesh m in editor.UGXmeshes)
                    {
                        if (editor.meshEditorTab.ugxMeshTree.SelectedNode != null) editor.meshEditorTab.ugxMeshTree.SelectedNode.ForeColor = SystemColors.WindowText;
                        editor.meshEditorTab.ugxMeshTree.SelectedNode = null;
                        m.isHighlighted = false;
                    }
                    foreach (Mesh m in editor.imports)
                    {
                        m.isHighlighted = false;
                    }
                    return;
                }

                Vector3 pixelColor = new Vector3((float)r / (float)255, (float)g / (float)255, (float)b / (float)255);
                foreach (Mesh m in editor.imports)
                {
                    if (m.color == pixelColor) m.isHighlighted = true;
                    else m.isHighlighted = false;
                }
                foreach (Mesh m in editor.UGXmeshes)
                {
                    if (m.color == pixelColor)
                    {
                        editor.meshEditorTab.ugxMeshTree.SelectedNode = editor.meshEditorTab.ugxMeshTree.Nodes[0].Nodes[editor.UGXmeshes.IndexOf(m)];
                        m.isHighlighted = true;
                    }
                    else m.isHighlighted = false;
                }

                fauxFocus = true;
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
                public Matrix4 viewMatrix;
                float radius = 0;
                public void UpdateCamera()
                {
                    viewMatrix = Matrix4.LookAt(position, target, new Vector3(0, 1, 0)) * new Matrix4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                    GL.BindBuffer(BufferTarget.UniformBuffer, editor.viewport.UBO);
                    GL.BufferSubData(BufferTarget.UniformBuffer, new IntPtr(Marshal.SizeOf(new Matrix4())), Marshal.SizeOf(new Matrix4()), ref viewMatrix);
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
                position += new Vector3(x, y, z);
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
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Vertex
            {
                public SystemHalf.Half x, y, z;
                public SystemHalf.Half u, v;
                public float nx, ny, nz;
                public byte b1, b2, b3, b4;
                public byte w1, w2, w3, w4;
            }
            public List<Vertex> vertices = new List<Vertex>();
            public List<uint> indices = new List<uint>();
            public int
                materialID = -1,
                boneID,
                vertexSize,
                faceCount;
            public bool isSkinned;
            public string name;

            //misc
            public bool isHighlighted = false;
            public bool isEnabled = true;

            //OGL
            int vb, ib, indexCount;
            int mLoc, cLoc, SmLoc, ScLoc, HmLoc;
            static int meshShader, selectionShader, highlightShader;
            static bool shaderInit = false;
            public Vector3 color;

            public void InitDrawing()
            {
                if (!shaderInit)
                    {
                        #region Mesh Shader

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
    FragColor = texture(textureSampler, gs_out.uv) * gs_out.lightStrength * color;
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

                        #endregion
                    }
                if (!shaderInit)
                    {
                        #region Selection Shader

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

                        selectionShader = GL.CreateProgram();
                        GL.AttachShader(selectionShader, vertS);
                        GL.AttachShader(selectionShader, fragS);
                        GL.LinkProgram(selectionShader);
                        #endregion
                    }
                if (!shaderInit)
                    {
                        #region Highlight Shader

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
out vec2 uv;
void main()
{
    uv = uvIn;
    vec3 newPos = positionIn;
    gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(newPos, 1.0);
}";
                        //////////////////////////////////////////////////////
                        string fs =
            @"#version 420 core

in vec2 uv;
out vec4 FragColor;
uniform sampler2D sampler;

void main()
{
    FragColor = texture(sampler, uv) + vec4(0, .45f, 0f, 1f);
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

                        highlightShader = GL.CreateProgram();
                        GL.AttachShader(highlightShader, vertS);
                        GL.AttachShader(highlightShader, fragS);
                        GL.LinkProgram(highlightShader);
                        #endregion
                    }
                shaderInit = true;

                mLoc = GL.GetUniformLocation(meshShader, "modelMatrix");
                cLoc = GL.GetUniformLocation(meshShader, "color");
                GL.UniformBlockBinding(meshShader, GL.GetUniformBlockIndex(meshShader, "GlobalMatrices"), 0);
                SmLoc = GL.GetUniformLocation(selectionShader, "modelMatrix");
                ScLoc = GL.GetUniformLocation(selectionShader, "color");
                GL.UniformBlockBinding(meshShader, GL.GetUniformBlockIndex(selectionShader, "GlobalMatrices"), 0);
                HmLoc = GL.GetUniformLocation(highlightShader, "modelMatrix");
                GL.UniformBlockBinding(meshShader, GL.GetUniformBlockIndex(highlightShader, "GlobalMatrices"), 0);

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
                byte[] rgb = new byte[3];
                random.NextBytes(rgb);
                color = new Vector3((float)rgb[0] / (float)255, (float)rgb[1] / (float)255, (float)rgb[2] / (float)255);
            }
            public void Draw()
            {
                if (!isEnabled) return;
                if (isHighlighted)
                {
                    GL.UseProgram(highlightShader);
                    GL.UniformMatrix4(HmLoc, false, ref modelMatrix);
                }
                else
                {
                    GL.UseProgram(meshShader);
                    GL.UniformMatrix4(mLoc, false, ref modelMatrix);
                }
                
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
            public void DrawFlat()
            {
                if (!isEnabled) return;
                GL.UseProgram(selectionShader);

                GL.UniformMatrix4(SmLoc, false, ref modelMatrix);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.HalfFloat, false, Marshal.SizeOf(new Mesh.Vertex()), 0);  //pos
                GL.EnableVertexAttribArray(0);

                GL.Uniform4(ScLoc, new Vector4(color, 1));

                GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.UseProgram(0);
            }
            //public void DrawHighlight()
            //{
            //    GL.UseProgram(highlightShader);
            //    GL.UniformMatrix4(HmLoc, false, ref modelMatrix);

            //    GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
            //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);

            //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.HalfFloat, false, Marshal.SizeOf(new Mesh.Vertex()), 0);  //pos
            //    GL.EnableVertexAttribArray(0);
            //    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Mesh.Vertex()), 10); //normal
            //    GL.EnableVertexAttribArray(1);

            //    GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //    GL.UseProgram(0);
            //}

            //misc
            static Random random = new Random();
        }
        public class Bone
        {
            public Matrix4 boneMatrix;
            public string name;
            int vb, ib, mLoc;
            static int boneShader; static bool init = false;
            public void InitDrawing()
            {
                if (!init)
                {
                    #region Bone Shader

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

out float lightStrength;

void main()
{
    vec3 a = gl_in[0].gl_Position.xyz;
    vec3 b = gl_in[1].gl_Position.xyz;
    vec3 c = gl_in[2].gl_Position.xyz;
    vec3 normal = normalize(cross(a-b, b-c));
    vec3 lightDir = vec3(0, 0, -1); //out from the camera.
    lightStrength = max(0, dot(normalize(normal), lightDir));

    gl_Position = gl_in[0].gl_Position;
    EmitVertex();
    gl_Position = gl_in[1].gl_Position;
    EmitVertex();
    gl_Position = gl_in[2].gl_Position;
    EmitVertex();
    EndPrimitive();
}
";
                    //////////////////////////////////////////////////////
                    string fs =
        @"#version 420 core

in float lightStrength;

uniform vec4 color;
uniform sampler2D textureSampler;
out vec4 FragColor;

void main()
{
    FragColor = vec4(.8f, .8f, 1.0f, 1.0f);
}";
                    //////////////////////////////////////////////////////
                    #endregion
                    
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

                    boneShader = GL.CreateProgram();
                    GL.AttachShader(boneShader, vertS);
                    GL.AttachShader(boneShader, fragS);
                    GL.AttachShader(boneShader, geomS);
                    GL.LinkProgram(boneShader);
                    GL.UseProgram(boneShader);

                    #endregion
                    init = true;
                }
                float bvs = .0625f; //boneVisualScale
                Vector3[] v =
                {
                    new Vector3(0, 0, 0),

                    new Vector3(-bvs, bvs, 0),
                    new Vector3(-bvs, 0, bvs),
                    new Vector3(-bvs, -bvs, 0),
                    new Vector3(-bvs, 0, -bvs),

                    new Vector3(-bvs * 4, 0, 0)
                };
                uint[] i =
                {
                    0,1,2,
                    0,2,3,
                    0,3,4,
                    0,4,1,


                    1,5,2,
                    2,5,3,
                    3,5,4,
                    4,5,1
                };

                vb = GL.GenBuffer();
                ib = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
                GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(new Vector3()) * 6, v, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);
                GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * 24, i, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                mLoc = GL.GetUniformLocation(boneShader, "modelMatrix");
                GL.UniformBlockBinding(boneShader, GL.GetUniformBlockIndex(boneShader, "GlobalMatrices"), 0);
            }
            public void Draw()
            {
                GL.UseProgram(boneShader);
                GL.UniformMatrix4(mLoc, false, ref boneMatrix);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(new Vector3()), 0);  //pos
                GL.EnableVertexAttribArray(0);

                GL.DrawElements(PrimitiveType.Triangles, 24, DrawElementsType.UnsignedInt, 0);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.UseProgram(0);
            }
        }
    }
}
