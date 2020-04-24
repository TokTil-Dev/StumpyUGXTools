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
    public static string version = "1.0.0-pre5";
    public static Editor editor = new Editor();

    [STAThread]
    static void Main()
    {
#if DEBUG
        if (!File.Exists("I:\\StumpyUGXTools\\StumpyUGXTools\\devkey")) //this is just so that you avoid headaches when accidentally trying to run the build with my absolute system paths.
        {                                                               //this can be removed if you REALLY want to, but there is literally no point, and it probably will crash.
            Console.WriteLine("Please run in \'Release\' configuration.");
            while (true) { }
        }
#endif

        LoadCFG();

        Application.EnableVisualStyles();
        editor.InitializeComponent();
        editor.versionLabel.Text = version;
        Application.Run(editor);
    }

    static void LoadCFG()
    {
#if DEBUG
        editor.textureReferencePaths.Add("F:\\HaloWarsModding\\HaloWarsDE\\Extract\\art");
        editor.textureReferencePaths.Add("C:\\Users\\Jake\\Desktop");
#endif

        if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StumpyUGXTools\\config.cfg"))
        {
            Directory.CreateDirectory((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StumpyUGXTools"));
        }
        else
        {

        }
    }
}

