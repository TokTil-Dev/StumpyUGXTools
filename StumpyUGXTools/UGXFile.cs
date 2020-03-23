using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using MiscUtil.Conversion;
using CRC;
using static Program;


class BDTNode
{
    public List<BDTNode> childNodes = new List<BDTNode>();

    public BDTNameValue[] nameValue;
    string name;
    object value;
    string[] attribNames;
    object[] attribData;
    public int layer = 0;
    
    public void FindChildrenInArrayRecursive(BDTNode[] nodes)
    {
        if(parentIndex != 0xFFFF)
        {
            layer = nodes[parentIndex].layer + 1;
        }
        for (int i = 0; i < numberOfChildNodes; i++)
        {
            childNodes.Add(nodes[i + childNodeIndex]);
        }
        foreach (BDTNode n in childNodes) { n.FindChildrenInArrayRecursive(nodes); }
    }

    //raw data
    public UInt16 parentIndex;
    public UInt16 childNodeIndex;
    public UInt16 nameValueIndex;
    public byte numberOfNameValues;
    public byte numberOfChildNodes;
}
class BDTNameValue
{
    public UInt32 value;
    public UInt16 nameOffset;
    public UInt16 flags;
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
        File.WriteAllBytes(path, fileData.ToArray());
    }
    public void Unload()
    {
        fileData.Clear();
    }

#region Texture Editing

    List<byte> matData;
    int matChunkLoc;

    //header info
    const int crc8Loc = 2;
    const int crc32Loc = 4;
    const int dataSizeLoc = 8;

    const int nodesSizeLoc = 12;
    const int nameValuesSizeLoc = 16;
    const int nameDataSizeLoc = 20;
    const int valueDataSizeLoc = 24;

    //material section info
    int nodesSize, nodesCount;
    int nameValuesSize, nameValuesCount;
    int nameDataSize;
    int valueDataSize;

    public BDTNode rootNode;
    public BDTNode[] materialTreeNodes;

    public void InitTextureEditing()
    {
        matChunkLoc = bec.ToInt32(fileData.ToArray(), 136);
        matData = fileData.GetRange(matChunkLoc, bec.ToInt32(fileData.ToArray(), 140));

        nodesSize = BitConverter.ToInt32(matData.ToArray(), nodesSizeLoc);
        nodesCount = nodesSize / 8;
        nameValuesSize = BitConverter.ToInt32(matData.ToArray(), nameValuesSizeLoc);
        nameValuesCount = nameValuesSize / 8;
        nameDataSize = BitConverter.ToInt32(matData.ToArray(), nameDataSizeLoc);
        valueDataSize = BitConverter.ToInt32(matData.ToArray(), valueDataSizeLoc);

        BDTNode[] nodes = new BDTNode[nodesCount];
        for (int i = 0; i < nodesCount; i++)
        {
            nodes[i] = new BDTNode();
            byte[] thisNodeData = matData.GetRange((i * 8) + 28, 8).ToArray();
            nodes[i].parentIndex = BitConverter.ToUInt16(thisNodeData, 0);
            nodes[i].childNodeIndex = BitConverter.ToUInt16(thisNodeData, 2);
            nodes[i].nameValueIndex = BitConverter.ToUInt16(thisNodeData, 4);
            nodes[i].numberOfNameValues = thisNodeData[6];
            nodes[i].numberOfChildNodes = thisNodeData[7];
            if (nodes[i].parentIndex == 0xFFFF) { rootNode = nodes[i]; }
        }
        rootNode.FindChildrenInArrayRecursive(nodes);
        
    }

#endregion
}