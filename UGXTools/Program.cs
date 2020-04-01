using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MiscUtil.Conversion;

class Program
{
    public static BigEndianBitConverter bec = new BigEndianBitConverter();

    static void Main(string[] args)
    {
        Editor editor = new Editor();
        UGXFile ugx = new UGXFile();
        ugx.Load("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\assault_rifle_01.ugx");
        editor.ReplaceMesh(editor.ImportAsset("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\bp.fbx").meshes[0], ugx, 0);
        ugx.Save("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\assault_rifle_01_out.ugx");
        Console.ReadKey();
    }

    public static void LogOut(object s)
    {
        Console.WriteLine(s.ToString());
    }
}
