using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Conversion;
using CRC;

class Program
{
    public static UGXFile ugx;
    public static GUI gui = new GUI();
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        gui.InitializeComponent();
        Application.Run(gui);
    }
}

class UGXFile
{
    BigEndianBitConverter bec = new BigEndianBitConverter();

    List<byte> fileData;
    public string UGXpath;

    public UGXFile()
    {

    }
    public int Load(string path)
    {
        if (!File.Exists(path))
        {
            Program.gui.LogOut("Invalid file path. Please select a different location.");
            return -1;
        }

        using (FileStream file = File.Open(path, FileMode.Open))
        {
            byte[] b = new byte[file.Length];
            file.Read(b, 0, (int)file.Length);
            fileData = new List<byte>(b);
            file.Close();
        }
        
        if (bec.ToUInt32(fileData.GetRange(0, 4).ToArray(), 0) != 3669653303) //check for ugx signature
        {
            fileData.Clear();
            Program.gui.LogOut("File is not a valid UGX file. Please select a different file.");
            return -1;
        }
        UGXpath = path;
        return 1;
    }
    public void Save(string path)
    {
        fileData.ReplaceRange(bec.GetBytes(fileData.Count), 12, 0);
        fileData.ReplaceRange(bec.GetBytes(Utils.CalcAdler32(fileData.ToArray(), 12, 20)), 8, 0);
        File.SetAttributes(path, FileAttributes.Normal);
        File.WriteAllBytes(path, fileData.ToArray());

        Program.gui.LogOut(path);

    }
    public void Unload()
    {
        fileData.Clear();
    }

#region Texture Editing

    int materialsLoc;
    int pathBlockLoc;
    public string[] texturePaths;

    public int GetTexturePaths()
    {
        materialsLoc = bec.ToInt32(fileData.ToArray(), 136);

        byte[] arr = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x40, 0x00, 0x00, 0x00, 0x00 };
        pathBlockLoc = Utils.FindPatternInByteArray(fileData.ToArray(), arr, materialsLoc) + 16;
        byte[] paths = new byte[fileData.Count - pathBlockLoc - 1];
        Array.Copy(fileData.ToArray(), pathBlockLoc, paths, 0, paths.Length);
        texturePaths = Encoding.Default.GetString(paths).Split('\0');

        if (texturePaths.Length == 0)
        {
            Program.gui.LogOut("No texture paths found in this UGX.");
            return -1;
        }
        return 1;
    }
    public void SaveMaterialsChunk()
    {
        List<byte> newPathBlock = new List<byte>();
        foreach(string s in texturePaths)   //construct new texture path data from edited paths
        {
            newPathBlock.InsertRange(newPathBlock.Count, Encoding.Default.GetBytes(s));
            newPathBlock.Insert(newPathBlock.Count, 0x00);
        }
        fileData.RemoveRange(pathBlockLoc, fileData.Count - pathBlockLoc);      //remove old texture path data
        fileData.InsertRange(pathBlockLoc, newPathBlock.ToArray());             //insert new texture path data

        fileData.ReplaceRange(BitConverter.GetBytes(fileData.Count - materialsLoc - 28), materialsLoc + 8, 4);     //write new material data size to material chunks's header
        fileData.ReplaceRange(bec.GetBytes(fileData.Count - materialsLoc), 140, 0);                                //write new material chunk size to ugx's 0x704 header
        fileData.ReplaceRange(BitConverter.GetBytes(fileData.Count - pathBlockLoc + 32), materialsLoc + 24, 4);    //write new material "valueData" size to materials chunk's header

        Crc crc32 = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc32]);
        byte[] b32 = crc32.ComputeHash(fileData.ToArray(), materialsLoc + 28, fileData.Count - materialsLoc - 28); //calc material data crc32
        fileData.ReplaceRange(b32, materialsLoc + 4, 4);                                                           //write material data crc32

        Crc crc8 = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc16Genibus]);
        byte[] tempHeader = fileData.GetRange(materialsLoc, 28).ToArray(); tempHeader[2] = 0x00; //temp material header without crc8
        byte[] b8 = crc8.ComputeHash(tempHeader, 0, 28);                                         //calc material header crc16, gets trunicated to 8 bits
        fileData.ReplaceRange(b8, materialsLoc + 2, 1);                                          //write material header crc8
    }

#endregion
}

