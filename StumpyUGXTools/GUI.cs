using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Program;

public class GUI : Form
{
    private TextBox pathBox;
    private OpenFileDialog fbd;
    private TextBox logBox;
    private Button saveButton;
    private TextBox version;
    private Button findButton;

    public void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.findButton = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.fbd = new System.Windows.Forms.OpenFileDialog();
            this.logBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.version = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // findButton
            // 
            this.findButton.Location = new System.Drawing.Point(11, 28);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(101, 27);
            this.findButton.TabIndex = 0;
            this.findButton.Text = "Search Files";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // pathBox
            // 
            this.pathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.pathBox.Location = new System.Drawing.Point(12, 59);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(476, 23);
            this.pathBox.TabIndex = 1;
            this.pathBox.TextChanged += new System.EventHandler(this.pathBox_textChanged);
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Location = new System.Drawing.Point(116, 32);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(372, 20);
            this.logBox.TabIndex = 4;
            this.logBox.Text = "Please select a UGX file, or paste in a path.";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(12, 87);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(477, 27);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save File";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // version
            // 
            this.version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.version.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.version.Location = new System.Drawing.Point(349, 13);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(140, 13);
            this.version.TabIndex = 6;
            this.version.Text = "v0.2.0";
            this.version.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // GUI
            // 
            this.ClientSize = new System.Drawing.Size(500, 125);
            this.Controls.Add(this.version);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.findButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI";
            this.Text = "UGXTools";
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    private void buttonFind_Click(object sender, EventArgs e)
    {
        if (fbd.ShowDialog() == DialogResult.OK)
        {
            pathBox.Text = fbd.FileName;
        }
    }
    private void buttonSave_Click(object sender, EventArgs e)
    {
        Save();
    }
    private void pathBox_textChanged(object sender, EventArgs e)
    {
        Program.LoadUGX(pathBox.Text);
        SetupPathView();
    }

    public void LogOut(string s)
    {
        logBox.Text = s;
    }

    List<TexturePathBox> paths = new List<TexturePathBox>();
    public void SetupPathView()
    {
    //{
    //    paths.Clear();

    //    for (int i = 0; i < ugx.nodes.Length; i++)
    //    {
    //        if (ugx.nodes[i].nodeNameValue.decodedName == "Map")
    //        {
    //            for(int j = 0; j < ugx.nodes[i].attributeNameValues.Length; j++)
    //            {
    //                if (ugx.nodes[i].attributeNameValues[j].decodedName == "Name")
    //                {
    //                    TexturePathBox tpb = new TexturePathBox();
    //                    tpb.tb.Location = new Point(12, 125 + (paths.Count * 25));
    //                    tpb.tb.Text = (string)ugx.nodes[i].attributeNameValues[j].decodedValue;
    //                    tpb.originalString = (string)ugx.nodes[i].attributeNameValues[j].decodedValue;
    //                    tpb.linkedNode = ugx.nodes[i];
    //                    tpb.linkedNameValueOffset = j;
    //                    paths.Add(tpb);
    //                }
    //            }
    //        }
    //    }
    //    gui.Size = new Size(gui.Size.Width, 164 + (paths.Count * 25) + 10);
    //    LogOut("Found " + paths.Count.ToString() + " texture paths.");
    }
    public void Save()
    {
        //foreach(TexturePathBox t in paths)
        //{
        //    if(t.tb.Text != t.originalString)
        //    {
        //        ugx.EditNameValueValueDataString(t.linkedNode, t.linkedNameValueOffset, t.tb.Text);
        //    }
        //}
    }
}

class TexturePathBox
{
    public TexturePathBox()
    {
        gui.Controls.Add(tb);
        tb.Size = new Size(gui.Size.Width - 40, 20);
        tb.Anchor = AnchorStyles.Right | AnchorStyles.Top;
    }
    ~TexturePathBox()
    {
        gui.Controls.Remove(tb);
    }
    public TextBox tb = new TextBox();
    public string originalString;
    public BDTNode linkedNode;
    public int linkedNameValueOffset;
}

//class GUI_BDTNode
//{
//    //static int yTracker;
//    public GUI_BDTNode()
//    {
//        gui.Controls.Add(textBox);
//        //thisNode = rootNode;
//        //yTracker++;
//        //textBox.Location = new Point(25+ thisNode.depth * 35, 110 + (yTracker * 25));
//        //textBox.Size = new Size(460 - (thisNode.depth * 35), 20);
//        //textBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
//    }
//    public TextBox textBox = new TextBox();
//    public List<GUI_BDTNode> childNodes = new List<GUI_BDTNode>();
//    BDTNode thisNode;
//}
//class GUI_BDTMaterialNode : GUI_BDTNode
//{
//    public GUI_BDTMaterialNode()
//    {
//        gui.Controls.Add(hideButton);
//    }
//    CheckBox hideButton;
//}
//class GUI_BDTNodeGraph
//{
//    GUI_BDTNodeGraph()
//    {

//    }


//    GUI_BDTMaterialNode[] masterNodes;
//    public void Create(BDTNode node)
//    {
//        masterNodes = new GUI_BDTMaterialNode[node.childNodes.Count];
//        for(int i = 0; i < node.childNodes.Count; i++)
//        {
//            masterNodes[i] = new GUI_BDTMaterialNode();

//        }
//    }
//}
