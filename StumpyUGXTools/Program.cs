using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Conversion;
using CRC;
using static Program;

class Program
{
    public static UGXFile ugx;
    public static GUI gui = new GUI();
    [STAThread]
    static void Main()
    {
        ugx = new UGXFile();
        ugx.Load("F:\\HaloWarsModding\\HaloWarsDE\\mod\\debug\\model.ug");
        ugx.InitTextureEditing();
        ugx.Save("F:\\HaloWarsModding\\HaloWarsDE\\mod\\debug\\model.ugx");
        ugx.Unload();
        Console.ReadKey();
    }
}

