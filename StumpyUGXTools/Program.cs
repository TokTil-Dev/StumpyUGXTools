using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Conversion;
using CRC;


static class Program
{
    public static string version = "0.5.0";

    public static UGXFile ugx = new UGXFile();
    public static GUI gui = new GUI();
    public static UGXTools.Form1 f1 = new UGXTools.Form1();

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        gui.InitializeComponent();
        gui.version.Text = version;
        //gui.pathBox.Text = "F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\assault_rifle_01_tester.ugx";
        Application.Run(gui);
    }

    //function for gui consumption
    public static int LoadUGX(string path)
    {
        if (File.Exists(path))
        {
            if (ugx.Load(path) == -1) { return - 1; }
            ugx.InitTextureEditing();
            ugx.InitMeshEditing();
            gui.LogOut("UGX Loaded.");
            return 1;
        }
        else
        {
            gui.LogOut("Could not find file specified.");
            return -1;
        }
    }
}

