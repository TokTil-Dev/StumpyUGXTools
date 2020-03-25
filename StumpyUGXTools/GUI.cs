using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
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
            //this.pathBox.Text = 
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
            if (i == 0) attribBoxes[i] = new AttribBox("SpecPower", i, Type.FLOAT);
            if (i == 1) attribBoxes[i] = new AttribBox("SpecColorR", i, Type.FLOAT);
            if (i == 2) attribBoxes[i] = new AttribBox("SpecColorG", i, Type.FLOAT);
            if (i == 3) attribBoxes[i] = new AttribBox("SpecColorB", i, Type.FLOAT);
            if (i == 4) attribBoxes[i] = new AttribBox("EnvReflectivity", i, Type.FLOAT);
            if (i == 5) attribBoxes[i] = new AttribBox("EnvSharpness", i, Type.FLOAT);
            if (i == 6) attribBoxes[i] = new AttribBox("EnvFresnel", i, Type.FLOAT);
            if (i == 7) attribBoxes[i] = new AttribBox("EnvFresnelPower", i, Type.FLOAT);
            if (i == 8) attribBoxes[i] = new AttribBox("AccessoryIndex", i, Type.UINT32);
            if (i == 9) attribBoxes[i] = new AttribBox("Flags", i, Type.UINT32);
            if (i == 10) attribBoxes[i] = new AttribBox("BlendType", i, Type.UINT8);
            if (i == 11) attribBoxes[i] = new AttribBox("Opacity", i, Type.UINT8);
        }
        for (int i = 0; i < 13; i++)
        {
            if (i == 0) pathBoxes[i] = new PathBox("Diffuse", i);
            if (i == 1) pathBoxes[i] = new PathBox("Normal", i);
            if (i == 2) pathBoxes[i] = new PathBox("Gloss", i);
            if (i == 3) pathBoxes[i] = new PathBox("Opacity", i);
            if (i == 4) pathBoxes[i] = new PathBox("Xform", i);
            if (i == 5) pathBoxes[i] = new PathBox("Emmissive", i);
            if (i == 6) pathBoxes[i] = new PathBox("Ao", i);
            if (i == 7) pathBoxes[i] = new PathBox("Env", i);
            if (i == 8) pathBoxes[i] = new PathBox("EnvMask", i);
            if (i == 9) pathBoxes[i] = new PathBox("EmXform", i);
            if (i == 10) pathBoxes[i] = new PathBox("Distortion", i);
            if (i == 11) pathBoxes[i] = new PathBox("Highlight", i);
            if (i == 12) pathBoxes[i] = new PathBox("Modulate", i);
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
                d.pathStrings_original[i] = d.pathStrings[i];
            }
            for(int i = 0; i < 12; i++)
            {
                if (ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue != null)
                {
                    d.attribValues[i] = ugx.nodes[0].childNodes[num].childNodes[0].childNodes[i].nodeNameValue.decodedValue.ToString();
                }
                d.attribValues_original[i] = d.attribValues[i];
            }
            matDat.Add(d);
        }
        ClientSize = new Size(ClientSize.Width, 690);
        LogOut("Found " + matDat.Count + " materials.");
        tabControl_SelectedIndexChanged(null, null);
    }
    public void Save()
    {
    }

    private void GUI_Load(object sender, EventArgs e)
    {

    }
}

enum Type { FLOAT, UINT8, UINT16, UINT32 }
class AttribBox
{
    int index;
    Type t;
    public AttribBox(string nameStr, int offset, Type type)
    {
        index = offset;
        t = type;

        gui.Controls.Add(name);
        gui.Controls.Add(value);
        gui.Controls.Add(revertButton);

        name.BorderStyle = BorderStyle.None;
        name.Location = new Point(15, 150 + (offset*40));
        name.Name = nameStr + "_name";
        name.Size = new Size(100, 13);
        name.TabIndex = offset;
        name.Text = nameStr;

        value.Location = new Point(15, 165 + (offset * 40));
        value.Name = nameStr + "_value";
        value.Size = new Size(100, 20);
        value.TabIndex = offset + 1;
        value.TextChanged += new EventHandler(ValueUpdated);

        revertButton.Location = new Point(117, 165 + (offset * 40));
        revertButton.Size = new Size(20, 20);
        revertButton.Text = "↻";
        revertButton.Click += new EventHandler(RevertButtonPress);
    }
    public TextBox value = new TextBox();
    Label name = new Label();
    Button revertButton = new Button();
    
    void ValueUpdated(object o, EventArgs e)
    {
        if(t == Type.UINT8 || t == Type.UINT16 || t == Type.UINT32)
        {
            Int64 i;
            if(!Int64.TryParse(value.Text, out i)) { value.Text = "0"; value.Select(1, 0); }
            if (t == Type.UINT8)
            {
                if (i > 255) { value.Text = "255"; value.Select(3, 0); }
                if (i < 0) {value.Text = "0"; value.Select(1, 0); }
            }
            if (t == Type.UINT16)
            {
                if (i > UInt16.MaxValue) { value.Text = UInt16.MaxValue.ToString(); value.Select(5, 0); }
                if (i < UInt16.MinValue) { value.Text = UInt16.MinValue.ToString(); value.Select(1, 0); }
            }
            if (t == Type.UINT32)
            {
                if (i > UInt32.MaxValue) { value.Text = UInt32.MaxValue.ToString(); value.Select(10, 0); }
                if (i < UInt32.MinValue) { value.Text = UInt32.MinValue.ToString(); value.Select(1, 0); }
            }
            value.Text = new string(value.Text.Where(c => c >= '0' && c <= '9').ToArray());
        }
        if(t == Type.FLOAT)
        {
            float f;
            if (!float.TryParse(value.Text, out f))
            {
                value.Text = "0";
                value.Select(1, 0);
            }
            value.Text = new string(value.Text.Where(c => c >= '0' && c <= '9' || c == '-' || c== '.').ToArray());
        }
    }
    void RevertButtonPress(object o, EventArgs e)
    {
        value.Text = gui.matDat[gui.tabControl.SelectedIndex].attribValues_original[index];
    }
}
class PathBox
{
    int index;
    public PathBox(string nameStr, int offset)
    {
        index = offset;

        gui.Controls.Add(name);
        gui.Controls.Add(buttonAdd);
        gui.Controls.Add(buttonRemove);
        gui.Controls.Add(buttonRevert);

        name.BorderStyle = BorderStyle.None;
        name.Location = new Point(160, 150 + (offset * 40));
        name.Name = nameStr + "_name";
        name.Size = new Size(100, 13);
        name.TabIndex = offset;
        name.Text = nameStr;

        value.Location = new Point(160, 165 + (offset * 40));
        value.Name = nameStr + "_value";
        value.Size = new Size(433, 20);
        value.TabIndex = offset + 1;
        value.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
        value.TextChanged += new EventHandler(PathUpdated);

        buttonAdd.Location = new Point(160, 165 + (offset * 40));
        buttonAdd.Size = new Size(20, 20);
        buttonAdd.Text = "+";
        buttonAdd.Click += new EventHandler(ButtonAddPress);

        buttonRemove.Location = new Point(616, 165 + (offset * 40));
        buttonRemove.Size = new Size(20, 20);
        buttonRemove.Text = "×";
        buttonRemove.Click += new EventHandler(ButtonRemovePress);
        buttonRemove.Anchor = AnchorStyles.Right | AnchorStyles.Top;

        buttonRevert.Location = new Point(595, 165 + (offset * 40));
        buttonRevert.Size = new Size(20, 20);
        buttonRevert.Text = "↻";
        buttonRevert.Click += new EventHandler(RevertButtonPress);
        buttonRevert.Anchor = AnchorStyles.Right | AnchorStyles.Top;
    }
    public void Update(bool hasValue)
    {
        if(hasValue)
        {
            gui.Controls.Add(value);
            gui.Controls.Add(buttonRemove);
            gui.Controls.Add(buttonRevert);
            gui.Controls.Remove(buttonAdd);
        }
        if(!hasValue)
        {
            gui.Controls.Remove(value);
            gui.Controls.Remove(buttonRemove);
            gui.Controls.Remove(buttonRevert);
            gui.Controls.Add(buttonAdd);
        }
    }
    void ButtonAddPress(object o, EventArgs e)
    {
        gui.matDat[gui.tabControl.SelectedIndex].hasValue[index] = true;
        Update(true);
    }
    void ButtonRemovePress(object o, EventArgs e)
    {
        gui.matDat[gui.tabControl.SelectedIndex].hasValue[index] = false;
        Update(false);
    }
    void RevertButtonPress(object o, EventArgs e)
    {
        value.Text = gui.matDat[gui.tabControl.SelectedIndex].pathStrings_original[index];
    }
    void PathUpdated(object o, EventArgs e)
    {
        gui.matDat[gui.tabControl.SelectedIndex].pathStrings[index] = value.Text;
    }

    public TextBox value = new TextBox();
    Label name = new Label();
    Button buttonAdd = new Button();
    Button buttonRemove = new Button();
    Button buttonRevert = new Button();
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

    public string[] pathStrings           = new string[13];
    public string[] pathStrings_original  = new string[13];
    public bool  [] hasValue              = new bool  [13];
    public bool  [] hasValue_original     = new bool  [13];
    public string[] attribValues          = new string[12];
    public string[] attribValues_original = new string[12];

}