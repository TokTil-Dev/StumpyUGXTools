using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Program;

class GUI : Form
{
    private TextBox pathBox;
    private OpenFileDialog fbd;
    private TextBox logBox;
    private Button saveButton;
    private TextBox version;
    private System.ComponentModel.IContainer components;
    public TabControl tabControl;
    private Button findButton;

    public void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.findButton = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.fbd = new System.Windows.Forms.OpenFileDialog();
            this.logBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.version = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
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
            this.pathBox.Size = new System.Drawing.Size(626, 23);
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
            this.logBox.Size = new System.Drawing.Size(522, 20);
            this.logBox.TabIndex = 4;
            this.logBox.Text = "Please select a UGX file, or paste in a path.";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(12, 86);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(626, 27);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save File";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // version
            // 
            this.version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.version.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.version.Location = new System.Drawing.Point(498, 13);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(140, 13);
            this.version.TabIndex = 6;
            this.version.Text = "0.3.0";
            this.version.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new System.Drawing.Point(12, 119);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(626, 23);
            this.tabControl.TabIndex = 7;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);

            // 
            // GUI
            // 
            this.ClientSize = new System.Drawing.Size(650, 120);
            this.Controls.Add(this.tabControl);
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
    public void LogOut(string s)
    {
        logBox.Text = s;
    }

    AttribBox[] attribBoxes = new AttribBox[12];
    PathBox[] pathBoxes = new PathBox[13];
    public List<MatData> matDat = new List<MatData>();
    public void Init()
    {
        SuspendLayout();
        for(int i = 0; i < 12; i++)
        {
            if (i == 0) attribBoxes[i] = new AttribBox("SpecPower", i);
            if (i == 1) attribBoxes[i] = new AttribBox("SpecColorR", i);
            if (i == 2) attribBoxes[i] = new AttribBox("SpecColorG", i);
            if (i == 3) attribBoxes[i] = new AttribBox("SpecColorB", i);
            if (i == 4) attribBoxes[i] = new AttribBox("EnvReflectivity", i);
            if (i == 5) attribBoxes[i] = new AttribBox("EnvSharpness", i);
            if (i == 6) attribBoxes[i] = new AttribBox("EnvFresnel", i);
            if (i == 7) attribBoxes[i] = new AttribBox("EnvFresnelPower", i);
            if (i == 8) attribBoxes[i] = new AttribBox("AccessoryIndex", i);
            if (i == 9) attribBoxes[i] = new AttribBox("Flags", i);
            if (i == 10) attribBoxes[i] = new AttribBox("BlendType", i);
            if (i == 11) attribBoxes[i] = new AttribBox("Opacity", i);
        }
        for (int i = 0; i < 13; i++)
        {
            if (i == 0) pathBoxes[i] = new PathBox("Diffuse", i, i);
            if (i == 1) pathBoxes[i] = new PathBox("Normal", i, i);
            if (i == 2) pathBoxes[i] = new PathBox("Gloss", i, i);
            if (i == 3) pathBoxes[i] = new PathBox("Opacity", i, i);
            if (i == 4) pathBoxes[i] = new PathBox("Xform", i, i);
            if (i == 5) pathBoxes[i] = new PathBox("Emmissive", i, i);
            if (i == 6) pathBoxes[i] = new PathBox("Ao", i, i);
            if (i == 7) pathBoxes[i] = new PathBox("Env", i, i);
            if (i == 8) pathBoxes[i] = new PathBox("EnvMask", i, i);
            if (i == 9) pathBoxes[i] = new PathBox("EmXform", i, i);
            if (i == 10) pathBoxes[i] = new PathBox("Distortion", i, i);
            if (i == 11) pathBoxes[i] = new PathBox("Highlight", i, i);
            if (i == 12) pathBoxes[i] = new PathBox("Modulate", i, i);
        }
        ResumeLayout(false);
        PerformLayout();
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
        if (Program.LoadUGX(pathBox.Text) == -1) return;
        gui.ClientSize = new Size(650, 675);
        SetupPathView();
    }
    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(tabControl.SelectedIndex >= 0)
        {
            MatData m = matDat[tabControl.SelectedIndex];
            for(int i = 0; i < 13; i++)
            {
                pathBoxes[i].Update(m.hasValue[i]);
                pathBoxes[i].value.Text = m.pathStrings[i];
                if (i < 12) attribBoxes[i].value.Text = m.attribValues[i];
            }
        }
    }

    bool init = false;
    public void SetupPathView()
    {
        if (!init) { Init(); init = true; };
        foreach (MatData m in matDat)
        {
            tabControl.Controls.Remove(m.tab);
        }
        matDat.Clear();
        for(int num = 0; num < ugx.nodes[0].childNodes.Count; num++)
        {
            MatData d = new MatData(num);
            for (int i = 0; i < 13; i++)
            {
                if(ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].childNodes.Count == 1)
                {
                    d.pathStrings[i] = (string)ugx.nodes[0].childNodes[num].childNodes[1].childNodes[i].childNodes[0].attributeNameValues[0].decodedValue;
                    d.hasValue[i] = true;
                }
                else
                {
                    d.pathStrings[i] = "";
                    d.hasValue[i] = false;
                }
                d.pathStrings_original[i] = d.pathStrings[i];
            }
            for(int i = 0; i < 12; i++)
            {
                if (ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue != null)
                {
                    d.attribValues[i] = ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue.ToString();
                }
            }
            matDat.Add(d);
        }
        ClientSize = new Size(ClientSize.Width, 690);
        tabControl_SelectedIndexChanged(null, null);
    }
    public void Save()
    {
    }

    private void GUI_Load(object sender, EventArgs e)
    {

    }
}

class AttribBox
{
    public AttribBox(string nameStr, int offset)
    {

        gui.Controls.Add(name);
        gui.Controls.Add(value);

        name.BorderStyle = BorderStyle.None;
        name.Location = new Point(15, 150 + (offset*40));
        name.Name = nameStr + "_name";
        name.ReadOnly = true;
        name.Size = new Size(100, 13);
        name.TabIndex = offset;
        name.Text = nameStr;

        value.Location = new Point(15, 165 + (offset * 40));
        value.Name = nameStr + "_value";
        value.Size = new Size(100, 20);
        value.TabIndex = offset + 1;
    }
    TextBox name = new TextBox();
    public TextBox value = new TextBox();
}
class PathBox
{
    private int ind;
    public PathBox(string nameStr, int offset, int index)
    {
        ind = index;

        gui.Controls.Add(name);
        gui.Controls.Add(button);

        name.BorderStyle = BorderStyle.None;
        name.Location = new Point(160, 150 + (offset * 40));
        name.Name = nameStr + "_name";
        name.ReadOnly = true;
        name.Size = new Size(100, 13);
        name.TabIndex = offset;
        name.Text = nameStr;

        value.Location = new Point(160, 165 + (offset * 40));
        value.Name = nameStr + "_value";
        value.Size = new Size(475, 20);
        value.TabIndex = offset + 1;
        value.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;

        button.Location = new Point(160, 165 + (offset * 40));
        button.Size = new Size(20, 20);
        button.Text = "+";
        button.Click += new EventHandler(ButtonPress);
    }
    public void Update(bool hasValue)
    {
        if(hasValue)
        {
            gui.Controls.Add(value);
            gui.Controls.Remove(button);
        }
        if(!hasValue)
        {
            gui.Controls.Remove(value);

            gui.Controls.Add(button);
        }
    }
    void ButtonPress(object o, EventArgs e)
    {
        gui.matDat[gui.tabControl.SelectedIndex].hasValue[ind] = true;
        Update(true);
    }

    TextBox name = new TextBox();
    public TextBox value = new TextBox();
    public Button button = new Button();
}

class MatData
{
    BDTNode linkedNode;
    public TabPage tab = new TabPage();
    public MatData(int index)
    {
        gui.tabControl.Controls.Add(tab);
        tab.Text = "Material " + (index + 1).ToString();
    }

    public string[] pathStrings          = new string[13];
    public string[] pathStrings_original = new string[13];
    public bool  [] hasValue             = new bool  [13];
    public string[] attribValues         = new string[12];

}