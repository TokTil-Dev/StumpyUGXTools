using System;
using System.Collections.Generic;
using System.Linq;
using Assimp;
using System.IO;
using MiscUtil.Conversion;
using SystemHalf;
using static Program;


class UGXFile
{
    public List<byte> header; const int header_loc = 0; //header... duh      DABA7737
    public List<byte> grannyData;   int grannyData_loc; //granny file info   0x703
    public List<byte> cachedData;   int cachedData_loc; //cached data        0x700
    public List<byte> vbData;       int vbData_loc;     //vertex buffer      0x702
    public List<byte> ibData;       int ibData_loc;     //index buffer       0x701
    public List<byte> matData;      int matData_loc;    //materials          0x704

    public long[] subDataCount = new long[6];
    public long[] subDataOffset = new long[6];

    public int Load(string path)
    {
        if (!File.Exists(path)) return -1;
        List<byte> fileData = File.ReadAllBytes(path).ToList<byte>();
        if (bec.ToUInt32(fileData.GetRange(0, 4).ToArray(), 0) != 3669653303) { fileData.Clear(); return -1; }

        //get chunk locations
        grannyData_loc = bec.ToInt32(fileData.ToArray(), 40);
        cachedData_loc = bec.ToInt32(fileData.ToArray(), 64);
        vbData_loc = bec.ToInt32(fileData.ToArray(), 88);
        ibData_loc = bec.ToInt32(fileData.ToArray(), 112);
        matData_loc = bec.ToInt32(fileData.ToArray(), 136);

        //separate file into it's chunks
        header = fileData.GetRange(0, 152);
        grannyData = fileData.GetRange(grannyData_loc, bec.ToInt32(fileData.ToArray(), 44));
        cachedData = fileData.GetRange(cachedData_loc, bec.ToInt32(fileData.ToArray(), 68));
        vbData = fileData.GetRange(vbData_loc, bec.ToInt32(fileData.ToArray(), 92));
        ibData = fileData.GetRange(ibData_loc, bec.ToInt32(fileData.ToArray(), 116));
        matData = fileData.GetRange(matData_loc, bec.ToInt32(fileData.ToArray(), 140));

        for (int i = 0; i < 6; i++)
        {
            subDataCount[i] = BitConverter.ToInt64(cachedData.GetRange(64 + (16 * i), 8).ToArray(), 0);
            subDataOffset[i] = BitConverter.ToInt64(cachedData.GetRange(72 + (16 * i), 8).ToArray(), 0);
        }


        return 1;
    }
    public void Save(string path)
    {
        List<byte> file = new List<byte>();
        //write offsets of chunks to file header.
        header.ReplaceRange(bec.GetBytes(header.Count), 40, 4);
        header.ReplaceRange(bec.GetBytes(header.Count + grannyData.Count), 64, 4);
        header.ReplaceRange(bec.GetBytes(header.Count + grannyData.Count + cachedData.Count), 88, 4);
        header.ReplaceRange(bec.GetBytes(header.Count + grannyData.Count + cachedData.Count + vbData.Count), 112, 4);
        header.ReplaceRange(bec.GetBytes(header.Count + grannyData.Count + cachedData.Count + vbData.Count + ibData.Count), 136, 4);
        //write sizes of chunks to file header.
        header.ReplaceRange(bec.GetBytes(grannyData.Count), 44, 4);
        header.ReplaceRange(bec.GetBytes(cachedData.Count), 68, 4);
        header.ReplaceRange(bec.GetBytes(vbData.Count), 92, 4);
        header.ReplaceRange(bec.GetBytes(ibData.Count), 116, 4);
        header.ReplaceRange(bec.GetBytes(matData.Count), 140, 4);
        //write size of file and adler32 to file header
        header.ReplaceRange(bec.GetBytes(header.Count + grannyData.Count + cachedData.Count + vbData.Count + ibData.Count + matData.Count), 12, 4);
        header.ReplaceRange(bec.GetBytes(Utils.CalcAdler32(header.ToArray(), 12, 20)), 8, 4);
        //constuct file from chunks.
        file.AddRange(header);
        file.AddRange(grannyData);
        file.AddRange(cachedData);
        file.AddRange(vbData);
        file.AddRange(ibData);
        file.AddRange(matData);
        //write file.
        File.WriteAllBytes(path, file.ToArray());
    }

    struct MeshInfo
    {
        public int vertCount;
        public int vertOffset;
        public int vertSize;
        public int vertLength;
        public int faceCount;
        public int faceOffset;
        public int matID;
        public int polyID;
        public int boneID;
        public string polyName;

        public void Print()
        {
            Console.WriteLine("vertCount: " + vertCount);
            Console.WriteLine("vertOffset: " + vertOffset);
            Console.WriteLine("vertSize: " + vertSize);
            Console.WriteLine("faceCount: " + faceCount);
            Console.WriteLine("faceOffset: " + faceOffset);
        }
    };
    MeshInfo[] meshInfo;
    public void PrintFileData()
    {
        //cached data read

        //mesh data - ("if y == 1" in the .ms).
        meshInfo = new MeshInfo[subDataCount[0]];
        for (int i = 0; i < subDataCount[0]; i++) //("for z=1 to subDataCount[y] do").
        {
            meshInfo[i] = new MeshInfo();
            meshInfo[i].matID = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 0 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].polyID = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 4 + (152 * i), 4).ToArray(), 0);
            /*unknown ID variable*/
            BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 8 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].boneID = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 12 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].faceOffset = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 16 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].faceCount = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 20 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertOffset = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 24 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertLength = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 28 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertSize = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 32 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertCount = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 36 + (152 * i), 4).ToArray(), 0);
            //meshInfo[i].polyName = Utils.GetStringFromNullTerminatedByteArray(cachedData.ToArray(),
            //    BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + ((int)subDataCount[0]*152), 4).ToArray(), 0));

            //Console.WriteLine(meshInfo[i].vertCount);
        }
    }
}


       