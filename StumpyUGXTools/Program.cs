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
    public static string version = "0.3.0";

    public static UGXFile ugx = new UGXFile();
    public static bool ugxLoaded = false;
    public static GUI gui = new GUI();

    [STAThread]
    static void Main()
    {
        gui.InitializeComponent();
        gui.pathBox.Text = "F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\albatross_01.ugx";
        Application.EnableVisualStyles();
        Application.Run(gui);
    }

    //functions for gui consumption
    public static int LoadUGX(string path)
    {
        if (File.Exists(path))
        {
            if (ugx.Load(path) == -1) { return - 1; }
            ugx.InitTextureEditing();
            gui.LogOut("UGX Loaded.");
            ugxLoaded = true;
            return 1;
        }
        else
        {
            gui.LogOut("Could not find file specified.");
            return -1;
        }
    }
}

