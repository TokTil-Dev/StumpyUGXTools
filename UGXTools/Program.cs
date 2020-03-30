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
        UGXFile f = new UGXFile();
        //f.Load("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\assault_rifle_01.ugx");
        foreach(string s in Directory.GetFiles("F:\\HaloWarsModding\\HaloWarsDE\\Extract\\art", ".", SearchOption.AllDirectories))
        {
            if (s.Contains(".ugx")) f.Load(s);
        }
        //f.Save("F:\\HaloWarsModding\\HaloWarsDE\\mod\\assault_rifle_01_out.ugx");
        //f.Save("F:\\HaloWarsModding\\HaloWarsDE\\mod\\art\\assault_rifle_01_out.ugx");
        Console.ReadKey();
    }

    public static void LogOut(object s)
    {
        Console.WriteLine(s.ToString());
    }
}
