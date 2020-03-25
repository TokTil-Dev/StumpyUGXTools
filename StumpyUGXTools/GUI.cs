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
            this.tabControl.Location = new System.Drawing.Point(12, 119);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(626, 23);
            this.tabControl.TabIndex = 7;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
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

    public void LogOut(string s)
    {
        logBox.Text = s;
    }


    public List<MaterialGUIElement> matTabs = new List<MaterialGUIElement>();
    public void SetupPathView()
    {
        SuspendLayout();
        foreach(MaterialGUIElement m in matTabs) m.Destroy();
        matTabs.Clear();
        Console.WriteLine("A");

        for (int i = 0; i < ugx.nodes[0].childNodes.Count; i++)
        {
            MaterialGUIElement t = new MaterialGUIElement();
            t.SetupGUI(ugx.nodes[0].childNodes[i], i);
            matTabs.Add(t);
        }

        LogOut("Found " + matTabs.Count + " materials.");

        matTabs[tabControl.SelectedIndex].Select();

        ResumeLayout();
    }
    public void Save()
    {
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        SuspendLayout();
        foreach (MaterialGUIElement m in matTabs)
        {
            m.Deselect();
        }
        if (tabControl.SelectedIndex >= 0) matTabs[tabControl.SelectedIndex].Select();
        ResumeLayout();
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
    public TextBox tb = new TextBox();
    public string originalString;
    public int linkedNameValueIndex;
}


class MapGUINode
{
    public BDTNode mapNode;
    public TextBox tb = new TextBox();
    public TextBox name = new TextBox();
    public Button button = new Button();
    public bool tbEnabled = false, buttonEnabled = false;

    public MapGUINode()
    {
        button.Click += AddPathBox;
    }

    public void AddPathBox(object sender, EventArgs e)
    {
        gui.Controls.Add(tb);
        gui.Controls.Remove(button);
        tbEnabled = true;
        buttonEnabled = false;
    }

    public void Enable()
    {
        if (tbEnabled) gui.Controls.Add(tb);
        if (buttonEnabled) gui.Controls.Add(button);
        gui.Controls.Add(name);
    }
    public void Disable()
    {
        gui.Controls.Remove(tb);
        gui.Controls.Remove(button);
        gui.Controls.Remove(name);
    }
}
enum Type { uint8, Uint32, Float }
class AttributeGUINode
{
    public TextBox valueTB = new TextBox();
    public TextBox attribName = new TextBox();
    public TextBox typeDescription = new TextBox();
    private Type type;

    public void Enable()
    {
        gui.Controls.Add(attribName);
        gui.Controls.Add(valueTB);
    }
    public void Disable()
    {
        gui.Controls.Remove(attribName);
        gui.Controls.Remove(typeDescription);
        gui.Controls.Remove(valueTB);
    }
}

class MaterialGUIElement
{
    BDTNode node;
    TabPage tab = new TabPage();
    //material data
    AttributeGUINode[] attribs = new AttributeGUINode[12];
    //path data
    MapGUINode[] maps = new MapGUINode[13];

    public void SetupGUI(BDTNode materialRootNode, int index)
    {
        gui.tabControl.Controls.Add(tab);
        tab.Text = "Material " + (index + 1).ToString();
        node = materialRootNode;
        for(int i = 0; i < materialRootNode.childNodes[0].childNodes.Count; i++)
        {
            attribs[i] = new AttributeGUINode();

            attribs[i].attribName.Size = new Size(100, 0);
            attribs[i].attribName.Location = new Point(12, 145 + (40 * i));
            attribs[i].attribName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            attribs[i].attribName.BorderStyle = BorderStyle.None;
            attribs[i].attribName.ReadOnly = true;
            attribs[i].attribName.Text = (string)materialRootNode.childNodes[0].childNodes[i].nodeNameValue.decodedName;

            attribs[i].valueTB.Size = new Size(100, 0);
            attribs[i].valueTB.Location = new Point(12, 160 + (40 * i));
            attribs[i].valueTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            if (materialRootNode.childNodes[0].childNodes[i].nodeNameValue.decodedValueType == NameValueFlags_Type.FLOAT)
            {
                attribs[i].valueTB.Text = ((float)materialRootNode.childNodes[0].childNodes[i].nodeNameValue.decodedValue).ToString();
            }

            attribs[i].Disable();
        }
        for(int i = 0; i < materialRootNode.childNodes[1].childNodes.Count; i++)
        {
            maps[i] = new MapGUINode();

            if(materialRootNode.childNodes[1].childNodes[i].childNodes.Count > 0)
            {
                maps[i].tb.Text = (string)materialRootNode.childNodes[1].childNodes[i].childNodes[0].attributeNameValues[0].decodedValue;
                maps[i].tbEnabled = true;
                maps[i].buttonEnabled = false;
                gui.Controls.Add(maps[i].tb);
            }
            else
            {
                maps[i].tbEnabled = false;
                maps[i].buttonEnabled = true;
            }

            maps[i].name.Location = new Point(150, 145 + (40 * i));
            maps[i].name.Size = new Size(350, 0);
            maps[i].name.Text = materialRootNode.childNodes[1].childNodes[i].nodeNameValue.decodedName;
            maps[i].name.BorderStyle = BorderStyle.None;
            maps[i].name.ReadOnly = true;

            maps[i].tb.Location = new Point(150, 160 + (40 * i));
            maps[i].tb.Size = new Size(488, 0);
            maps[i].tb.Font = new Font("Microsoft Sans Serif", 8F);
            maps[i].tb.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;

            maps[i].button.Text = "+";
            maps[i].button.Location = new Point(150, 160 + (40 * i));
            maps[i].button.Size = new Size(20, 20);
            
            maps[i].Disable();
        }
    }
    public void Select()
    {
        foreach (AttributeGUINode a in attribs) a.Enable();
        foreach (MapGUINode mn in maps)
        {
            mn.Enable();
            mn.tb.Size = new Size(gui.ClientSize.Width - 162, 20);
        }
    }
    public void Deselect()
    {
        foreach (AttributeGUINode a in attribs) a.Disable();
        foreach (MapGUINode mn in maps) mn.Disable();
    }
    public void Destroy()
    {
        node = null;
        gui.Controls.Remove(tab);
        gui.tabControl.Controls.Remove(tab);
        for(int i = 0; i < 13; i++)
        {
            if (i < 12) attribs[i].Disable();
            maps[i].Disable();
        }
    }
}