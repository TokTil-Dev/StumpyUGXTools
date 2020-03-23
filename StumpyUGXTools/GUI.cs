using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GUI : Form
{
    private TextBox pathBox;
    private OpenFileDialog fbd;
    private TextBox logBox;
    private Button buttonSave;
    private TextBox version;
    private Button findButton;

    public void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.findButton = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.fbd = new System.Windows.Forms.OpenFileDialog();
            this.logBox = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
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
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(11, 86);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(478, 27);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save File";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
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
            this.ClientSize = new System.Drawing.Size(500, 590);
            this.Controls.Add(this.version);
            this.Controls.Add(this.buttonSave);
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
        buttonLoad_Click();
    }
    private void buttonPaste_Click(object sender, EventArgs e)
    {
        pathBox.Text = Clipboard.GetText();
    }

    private void pathBox_textChanged(object sender, EventArgs e)
    {
        buttonLoad_Click();
    }

    bool fileLoaded = false;
    int cnt;
    PathBox[] pb= new PathBox[0];

    private void buttonLoad_Click()
    {
        foreach(BDTNode n in Program.ugx.rootNode.childNodes)
        {
            TextBox tb = new TextBox();
            Controls.Add(tb);
            //tb.Location = new Point()
        }

        {
            //int w = ClientSize.Width;

            //if (pb.Length != 0)
            //{
            //    for (int i = 0; i < pb.Length; i++)
            //    {
            //        pb[i].Remove(this);
            //    }
            //    ClientSize = new Size(w, 125);
            //}


            //Program.ugx = new UGXFile();
            //if(Program.ugx.Load(pathBox.Text) == -1) return;
            //if(Program.ugx.GetTexturePaths() == -1) return;
            //cnt = Program.ugx.texturePaths.Length;
            //if(cnt == 0)
            //{
            //    logBox.Text = "No texture paths found within this UGX.";
            //    Program.ugx.Unload();
            //    return;
            //}


            //int h = ClientSize.Height;

            //logBox.Text = "Found " + cnt.ToString() + " texture paths in loaded UGX.";
            //ClientSize = new Size(w, h + (25 * cnt) + 10);

            //pb = new PathBox[cnt];
            //for (int i = 0; i < cnt; i++)
            //{
            //    pb[i] = new PathBox(this);
            //    pb[i].tb.Location = new Point(12, h + (25 * i));
            //    pb[i].tb.Text = Program.ugx.texturePaths[i];
            //    pb[i].tb.Size = new Size(w -24,h);
            //    pb[i].b.Location = new Point(w - 32, h + (25 * i));
            //}
            //fileLoaded = true;
        }
    }
    private void buttonSave_Click(object sender, EventArgs e)
    {
        //if(!fileLoaded)
        //{
        //    LogOut("There is no file loaded.");
        //    return;
        //}
        
        //for (int i = 0; i < cnt; i++)
        //{
        //    Program.ugx.texturePaths[i] = pb[i].tb.Text;
        //}
        //Program.ugx.SaveMaterialsChunk();
        //Program.ugx.Save(Program.ugx.UGXpath);
        //LogOut("UGX saved to path.");
    }

    public void LogOut(string s)
    {
        logBox.Text = s;
    }
}

class PathBox
{
    public TextBox tb = new TextBox();
    public Button b = new Button();
    public PathBox(Form c)
    {
        tb.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
        tb.Name = "button2";
        tb.TabIndex = 2;
        c.Controls.Add(tb);

        //b.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
        //b.Image = Bitmap.FromFile("copy.png");
        //b.Size = new Size(20, 20);
        //b.Click += new EventHandler(click_paste);
        //c.Controls.Add(b);
    }
    public void Remove(Form c)
    {
        c.Controls.Remove(tb);
        c.Controls.Remove(b);
    }

    private void click_paste(object sender, EventArgs e)
    {
        tb.Text = Clipboard.GetText();
    }
}
