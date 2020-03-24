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

    public BDTNameValue nodeNameValue;
    public BDTNameValue[] attributeNameValues;

    public int depth;

    public void PrintFromThisNode()
    {
        for (int i = 0; i < depth; i++)
        {
            Console.Write('\t');
        }
        Console.Write("<" + nodeNameValue.decodedName);
        
        foreach(BDTNameValue b in attributeNameValues)
        {
            if (b.decodedValueType == NameValueFlags_Type.STRING)
            {
                Console.Write(" \"" + b.decodedName + "\"=");
                Console.Write(b.decodedValue);
            }
        }
        if (childNodes.Count == 0)
        {
            Console.Write(">");
        }
        else
        {
            Console.WriteLine(">");
        }

        foreach (BDTNode n in childNodes)
        {
            n.PrintFromThisNode();
        }

        if (childNodes.Count != 0)
        {
            for (int i = 0; i < depth; i++)
            {
                Console.Write('\t');
            }
        }

        Console.Write("</" + nodeNameValue.decodedName);

        Console.Write(">\n");
    }

    //raw data
    public UInt16 parentIndex;
    public UInt16 childNodeIndex;
    public UInt16 nameValueIndex;
    public byte numberOfNameValues;
    public byte numberOfChildNodes;
}

enum NameValueFlags_Type { NULL, BOOL, INT, FLOAT, STRING }
class BDTNameValue
{
    public string decodedName;
    public object decodedValue;

    public NameValueFlags_Type decodedValueType;

    public UInt32 valueOffsetOrData;
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
            //Program.gui.LogOut("Invalid file path. Please select a different location.");
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
            //Program.gui.LogOut("File is not a valid UGX file. Please select a different file.");
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
        if(fileData != null) fileData.Clear();
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
    
    public BDTNode[] nodes;

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

        nodes = new BDTNode[nodesCount];
        for (int i = 0; i < nodesCount; i++)
        {
            nodes[i] = new BDTNode();
            byte[] thisNodeData = matData.GetRange((i * 8) + 28, 8).ToArray();
            nodes[i].parentIndex = BitConverter.ToUInt16(thisNodeData, 0);
            nodes[i].childNodeIndex = BitConverter.ToUInt16(thisNodeData, 2);
            nodes[i].nameValueIndex = BitConverter.ToUInt16(thisNodeData, 4);
            nodes[i].numberOfNameValues = thisNodeData[6];
            nodes[i].numberOfChildNodes = thisNodeData[7];

            if (nodes[i].parentIndex != 0xFFFF)
            {
                nodes[nodes[i].parentIndex].childNodes.Add(nodes[i]);
                nodes[i].depth = nodes[nodes[i].parentIndex].depth + 1;
            }
        }
        DecodeNameValueData();

        //nodes[0].PrintFromThisNode();

        //EditNameValueValueDataFloat(nodes[0].childNodes[2].childNodes[1].childNodes[0], 1, .99f);
        //EditNameValueValueDataString(nodes[0].childNodes[2].childNodes[1].childNodes[0].childNodes[0], 1, "\\unsc\\infantry\\odst_01\\odst_01_df");
    }
    public void DecodeNameValueData() //populate nodeNameValue and attributeNameValues.
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].attributeNameValues = new BDTNameValue[nodes[i].numberOfNameValues - 1];
            for (int j = 0; j < nodes[i].numberOfNameValues; j++)
            {
                BDTNameValue nv = new BDTNameValue();
                byte[] thisNameValueData = matData.GetRange(((nodes[i].nameValueIndex + j) * 8) + 28 + nodesSize, 8).ToArray();
                nv.valueOffsetOrData = BitConverter.ToUInt32(thisNameValueData, 0);
                nv.nameOffset = BitConverter.ToUInt16(thisNameValueData, 4);
                nv.flags = BitConverter.ToUInt16(thisNameValueData, 6);

                nv.decodedName = Utils.GetStringFromNullTerminatedByteArray(matData.ToArray(), 28 + nodesSize + nameValuesSize + (int)nv.nameOffset);
                int valueType = (nv.flags >> 2) & ((1 << 3) - 1); //get type of data in the nameData.
                switch (valueType)
                {
                    case 0:
                        {
                            nv.decodedValueType = NameValueFlags_Type.NULL;
                            break;
                        } //NULL
                    case 1:
                        {
                            nv.decodedValueType = NameValueFlags_Type.BOOL;
                            break;
                        } //BOOL;
                    case 2:
                        {
                            nv.decodedValueType = NameValueFlags_Type.INT;
                            break;
                        } //INT;
                    case 3:
                        {
                            nv.decodedValueType = NameValueFlags_Type.FLOAT;
                            break;
                        } //FLOAT;
                    case 4:
                        {
                            nv.decodedValueType = NameValueFlags_Type.STRING;
                            nv.decodedValue = Utils.GetStringFromNullTerminatedByteArray(matData.ToArray(), matData.Count - valueDataSize + (int)nv.valueOffsetOrData);
                            break;
                        } //STRING;
                }

                if (j == 0) nodes[i].nodeNameValue = nv;
                else nodes[i].attributeNameValues[j - 1] = nv;
            }
        }
    }

    //value editing
    //public void EditNameValueValueDataString(BDTNode node, int nameValueIndex, string s)
    //{
    //    BDTNameValue nv;
    //    if (nameValueIndex == 0) nv = node.nodeNameValue;
    //    else nv = node.attributeNameValues[nameValueIndex - 1];

    //    Console.WriteLine(nv.decodedName);
    //    nv.valueOffsetOrData = (UInt32)valueDataSize;
        

    //    //temp
    //    matData.AddRange(Encoding.Default.GetBytes(s));
    //    matData.Add(0x00);
    //    valueDataSize += s.ToString().Length + 1;
    //    //\temp

    //    matData.ReplaceRange(BitConverter.GetBytes(nv.valueOffsetOrData), 28 + nodesSize + ((node.nameValueIndex + nameValueIndex) * 8), 4);
    //    DecodeNameValueData();
    //}
    public void EditNameValueValueDataString(int nameValueIndex, string s)
    {
        matData.ReplaceRange(BitConverter.GetBytes((UInt32)valueDataSize), 28 + nodesSize + (nameValueIndex * 8), 4);

        //temp
        matData.AddRange(Encoding.Default.GetBytes(s));
        matData.Add(0x00);
        valueDataSize += s.ToString().Length + 1;
        //\temp

        DecodeNameValueData();
    }
    //public void EditNameValueValueDataFloat(BDTNode node, int nameValueOffset, float f)
    //{
    //    BDTNameValue nv;
    //    if (nameValueOffset == 0) nv = node.nodeNameValue;
    //    else nv = node.attributeNameValues[nameValueOffset - 1];

    //    nv.decodedValue = f;

    //    matData.ReplaceRange(BitConverter.GetBytes(f), 28 + nodesSize + ((node.nameValueIndex + nameValueOffset) * 8), 4);
    //    DecodeNameValueData();
    //}

    public void SaveNewMaterial()
    {
        fileData.ReplaceRange(matData.ToArray(), matChunkLoc, matData.Count);

        fileData.ReplaceRange(BitConverter.GetBytes(matData.Count - 28), matChunkLoc + 8, 4);     //write new material data size to material chunks's header
        fileData.ReplaceRange(bec.GetBytes(matData.Count), 140, 0);                               //write new material chunk size to ugx's 0x704 header
        fileData.ReplaceRange(BitConverter.GetBytes(valueDataSize), matChunkLoc + 24, 4);         //write new material "valueData" size to materials chunk's header

        Crc crc32 = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc32]);
        byte[] b32 = crc32.ComputeHash(fileData.ToArray(), matChunkLoc + 28, fileData.Count - matChunkLoc - 28);  //calc material data crc32
        fileData.ReplaceRange(b32, matChunkLoc + 4, 4);                                                           //write material data crc32

        Crc crc8 = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc16Genibus]);
        byte[] tempHeader = fileData.GetRange(matChunkLoc, 28).ToArray(); tempHeader[2] = 0x00; //temp material header without crc8
        byte[] b8 = crc8.ComputeHash(tempHeader, 0, 28);                                        //calc material header crc16, gets trunicated to 8 bits
        fileData.ReplaceRange(b8, matChunkLoc + 2, 1);                                          //write material header crc8
    }

#endregion
}