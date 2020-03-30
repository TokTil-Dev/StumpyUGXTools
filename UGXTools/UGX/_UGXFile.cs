using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MiscUtil.Conversion;
using SystemHalf;
using static Program;

class UGXFile
{
    List<byte> header;  const int header_loc = 0; //header... duh      DABA7737
    List<byte> grannyData;    int grannyData_loc; //granny file info   0x703
    List<byte> cachedData;    int cachedData_loc; //cached data        0x700
    List<byte> vbData;        int vbData_loc;     //vertex buffer      0x702
    List<byte> ibData;        int ibData_loc;     //index buffer       0x701
    List<byte> matData;       int matData_loc;    //materials          0x704

    public int Load(string path)
    {
        if (!File.Exists(path)) return -1;
        List<byte> fileData = File.ReadAllBytes(path).ToList<byte>();
        if (bec.ToUInt32(fileData.GetRange(0, 4).ToArray(), 0) != 3669653303) { fileData.Clear(); return -1; }

        grannyData_loc  = bec.ToInt32(fileData.ToArray(), 40);
        cachedData_loc  = bec.ToInt32(fileData.ToArray(), 64);
        vbData_loc      = bec.ToInt32(fileData.ToArray(), 88);
        ibData_loc      = bec.ToInt32(fileData.ToArray(), 112);
        matData_loc     = bec.ToInt32(fileData.ToArray(), 136);

        header =     fileData.GetRange(0, 152);
        grannyData = fileData.GetRange(grannyData_loc, bec.ToInt32(fileData.ToArray(), 44));
        cachedData = fileData.GetRange(cachedData_loc, bec.ToInt32(fileData.ToArray(), 68));
        vbData     = fileData.GetRange(vbData_loc,     bec.ToInt32(fileData.ToArray(), 92));
        ibData     = fileData.GetRange(ibData_loc,     bec.ToInt32(fileData.ToArray(), 116));
        matData    = fileData.GetRange(matData_loc,    bec.ToInt32(fileData.ToArray(), 140));

        InitMeshEditing();

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
        header.ReplaceRange(bec.GetBytes(vbData.Count),     92, 4);
        header.ReplaceRange(bec.GetBytes(ibData.Count),     116, 4);
        header.ReplaceRange(bec.GetBytes(matData.Count),    140, 4);
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

    #region Mesh Editing
    long[] subDataCount = new long[6];
    long[] subDataOffset = new long[6];
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
    };
    MeshInfo[] meshInfo;
    struct BoneInfo
    {
        public int parentID;
        public float[] matrix4x4;
        public string boneName;
    }
    BoneInfo[] boneInfo;
    struct Vertex24b
    {
        public Half  vx, vy, vz,
                     p1, tu, tv;
        public float nx, ny, nz;
    } //0x18
    struct Vertex28b
    {

    } //0x1c
    struct Vertex32b
    {

    } //0x20
    struct Vertex36b
    {

    } //0x24
    struct Vertex40b
    {

    } //0x28
    struct Vertex44b
    {

    } //0x2c
    struct Vertex48b
    {

    } //0x30

    void InitMeshEditing()
    {
        //cached data read
        for(int i = 0; i < 6; i ++)
        {
            subDataCount[i]  = BitConverter.ToInt64(cachedData.GetRange(64 + (16 * i), 8).ToArray(), 0);
            subDataOffset[i] = BitConverter.ToInt64(cachedData.GetRange(72 + (16 * i), 8).ToArray(), 0);
        }

        //mesh data - ("if y == 1" in the .ms).
        meshInfo = new MeshInfo[subDataCount[0]];
        for(int i = 0; i < subDataCount[0]; i ++) //("for z=1 to subDataCount[y] do").
        {
            meshInfo[i] = new MeshInfo();
            meshInfo[i].matID      = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 0 + (152 * i), 4).ToArray(),  0);
            meshInfo[i].polyID     = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 4 + (152 * i), 4).ToArray(),  0);
            /*unknown ID variable*/  BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 8 + (152 * i), 4).ToArray(),  0);
            meshInfo[i].boneID     = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 12 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].faceOffset = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 16 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].faceCount  = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 20 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertOffset = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 24 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertLength = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 28 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertSize   = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 32 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].vertCount  = BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 36 + (152 * i), 4).ToArray(), 0);
            meshInfo[i].polyName   = Utils.GetStringFromNullTerminatedByteArray(cachedData.ToArray(), 
                BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[0] + 56 + (152 * i), 4).ToArray(), 0));
            if (meshInfo[i].vertSize != 0x18 && meshInfo[i].vertSize != 0x1c && meshInfo[i].vertSize != 0x20 && meshInfo[i].vertSize != 0x24 && meshInfo[i].vertSize != 0x28 && meshInfo[i].vertSize != 0x2c && meshInfo[i].vertSize != 0x30)
            {
                Console.WriteLine(meshInfo[i].vertSize);
            }
        }

        //bone data - ("if y == 2" in the .ms).
        boneInfo = new BoneInfo[subDataCount[1]];
        for (int i = 0; i < subDataCount[1]; i++)
        {
            boneInfo[i] = new BoneInfo();
            boneInfo[i].boneName = Utils.GetStringFromNullTerminatedByteArray(cachedData.ToArray(),
                BitConverter.ToInt32(cachedData.GetRange((int)subDataOffset[1] + (0 * i), 4).ToArray(), 0));

            boneInfo[i].matrix4x4 = new float[16];
            #region get mat4x4 floats
            byte[] mat4data = cachedData.GetRange((int)subDataOffset[1] + 8 + (0 * i), sizeof(float) * 16).ToArray();
            boneInfo[i].matrix4x4[0] = BitConverter.ToSingle(mat4data, 0);   //0,0
            boneInfo[i].matrix4x4[1] = BitConverter.ToSingle(mat4data, 4);   //0,1
            boneInfo[i].matrix4x4[2] = BitConverter.ToSingle(mat4data, 8);   //0,2
            boneInfo[i].matrix4x4[3] = BitConverter.ToSingle(mat4data, 12);  //0,3
            boneInfo[i].matrix4x4[4] = BitConverter.ToSingle(mat4data, 16);  //1,0
            boneInfo[i].matrix4x4[5] = BitConverter.ToSingle(mat4data, 20);  //1,1
            boneInfo[i].matrix4x4[6] = BitConverter.ToSingle(mat4data, 24);  //1,2
            boneInfo[i].matrix4x4[7] = BitConverter.ToSingle(mat4data, 28);  //1,3
            boneInfo[i].matrix4x4[8] = BitConverter.ToSingle(mat4data, 32);  //2,0
            boneInfo[i].matrix4x4[9] = BitConverter.ToSingle(mat4data, 36);  //2,1
            boneInfo[i].matrix4x4[10] = BitConverter.ToSingle(mat4data, 40); //2,2
            boneInfo[i].matrix4x4[11] = BitConverter.ToSingle(mat4data, 44); //2,3
            boneInfo[i].matrix4x4[12] = BitConverter.ToSingle(mat4data, 48); //3,0
            boneInfo[i].matrix4x4[13] = BitConverter.ToSingle(mat4data, 52); //3,1
            boneInfo[i].matrix4x4[14] = BitConverter.ToSingle(mat4data, 56); //3,2
            boneInfo[i].matrix4x4[15] = BitConverter.ToSingle(mat4data, 60); //3,3
            #endregion

            boneInfo[i].parentID = BitConverter.ToInt32(cachedData.ToArray(), (int)subDataOffset[1] + 64 + (0 * i));
        }
    }

    #endregion
}