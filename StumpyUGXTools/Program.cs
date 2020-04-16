using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Conversion;
using CRC;
using StumpyUGXTools;


static class Program
{
    public static Random random = new Random();
    public static BigEndianBitConverter bec = new BigEndianBitConverter();
    public static string version = "1.0.0-pre0.1";
    public static Editor editor = new Editor();

    [STAThread]
    static void Main()
    {
        //init user settings
        if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StumpyUGXTools\\config.cfg"))
        {
            editor.needsSetup = true;
            Directory.CreateDirectory((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StumpyUGXTools"));
        }
        else
        {
            editor.textureReferencePath = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StumpyUGXTools\\config.cfg")[0];
        }


        Application.EnableVisualStyles();
        editor.InitializeComponent();
        editor.version.Text = version;
        Application.Run(editor);
    }
}

