using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using SystemHalf;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MiscUtil.Conversion;
using CRC;
using static Program;
using static StumpyUGXTools.Editor;
using Mesh = StumpyUGXTools.Editor.Mesh;
using Bone = StumpyUGXTools.Editor.Bone;
using Half = SystemHalf.Half;

namespace StumpyUGXTools
{
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

            foreach (BDTNameValue b in attributeNameValues)
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
        public int typeSize = 0;
        public int totalSize = 0;
        public bool isUnsigned = false;

        public byte[] valueOffsetOrData;
        public UInt16 nameOffset;
        public UInt16 flags;

        public float[] f; //dirty hack
    }

    class UGXFile
    {
        public string fileName;
        public string filePath;
        public ImportHint importHint;

        public UGXFile()
        {

        }
        public List<byte> header; const int header_loc = 0; //header... duh      DABA7737
        public List<byte> grannyData; int grannyData_loc; //granny file info   0x703
        public List<byte> cachedData; int cachedData_loc; //cached data        0x700
        public List<byte> vbData; int vbData_loc;     //vertex buffer      0x702
        public List<byte> ibData; int ibData_loc;     //index buffer       0x701
        public List<byte> matData; int matData_loc;    //materials          0x704

        public enum ImportHint { HasBeenEdited, Fresh }
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

            if (fileData.Count == header.Count + grannyData.Count + cachedData.Count + vbData.Count + ibData.Count + matData.Count) importHint = ImportHint.HasBeenEdited;
            else importHint = ImportHint.Fresh;

            //get mesh info
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

        #region Texture Editing

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
            DecodeNameValueValues();
        }

        public void DecodeNameValueValues()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].attributeNameValues = new BDTNameValue[nodes[i].numberOfNameValues - 1];
                for (int j = 0; j < nodes[i].numberOfNameValues; j++)
                {
                    BDTNameValue nv = new BDTNameValue();
                    byte[] thisNameValueData = matData.GetRange(((nodes[i].nameValueIndex + j) * 8) + 28 + nodesSize, 8).ToArray();
                    nv.valueOffsetOrData = thisNameValueData;
                    nv.nameOffset = BitConverter.ToUInt16(thisNameValueData, 4);
                    nv.flags = BitConverter.ToUInt16(thisNameValueData, 6);

                    nv.decodedName = Utils.GetStringFromNullTerminatedByteArray(matData.ToArray(), 28 + nodesSize + nameValuesSize + (int)nv.nameOffset);
                    int valueType = (nv.flags >> 2) & ((1 << 3) - 1); //get type of data in the nameData.
                    nv.typeSize = 1 << ((nv.flags >> 5) & ((1 << 3) - 1));
                    nv.totalSize = (nv.flags >> 9) & ((1 << 7) - 1);
                    nv.isUnsigned = (nv.flags & 1) != 0;
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
                            } //BOOL
                        case 2:
                            {
                                nv.decodedValueType = NameValueFlags_Type.INT;
                                if (nv.typeSize == 4 && nv.isUnsigned) nv.decodedValue = BitConverter.ToUInt32(nv.valueOffsetOrData, 0);
                                if (nv.typeSize == 4 && !nv.isUnsigned) nv.decodedValue = BitConverter.ToInt32(nv.valueOffsetOrData, 0);
                                if (nv.typeSize == 1) nv.decodedValue = nv.valueOffsetOrData[0];
                                break;
                            } //INT
                        case 3:
                            {
                                if (nv.totalSize == 4)
                                {
                                    nv.decodedValueType = NameValueFlags_Type.FLOAT;
                                    nv.decodedValue = BitConverter.ToSingle((byte[])nv.valueOffsetOrData, 0);
                                }
                                if (nv.totalSize == 12)
                                {
                                    nv.decodedValue = BitConverter.ToUInt32((byte[])nv.valueOffsetOrData, 0);
                                    nv.f = new float[3];
                                    nv.f[0] = BitConverter.ToSingle(matData.GetRange((int)(matData.Count - valueDataSize + (UInt32)nv.decodedValue + 0), 4).ToArray(), 0);
                                    nv.f[1] = BitConverter.ToSingle(matData.GetRange((int)(matData.Count - valueDataSize + (UInt32)nv.decodedValue + 4), 4).ToArray(), 0);
                                    nv.f[2] = BitConverter.ToSingle(matData.GetRange((int)(matData.Count - valueDataSize + (UInt32)nv.decodedValue + 8), 4).ToArray(), 0);
                                }
                                break;
                            } //FLOAT
                        case 4:
                            {
                                nv.decodedValueType = NameValueFlags_Type.STRING;
                                nv.decodedValue = Utils.GetStringFromNullTerminatedByteArray(matData.ToArray(), matData.Count - valueDataSize + BitConverter.ToInt32(nv.valueOffsetOrData, 0));
                                break;
                            } //STRING
                    }

                    if (j == 0) nodes[i].nodeNameValue = nv;
                    else nodes[i].attributeNameValues[j - 1] = nv;
                }
            }
        }
        public void EncodeNameValueValueString(int nameValueIndex, string s)
        {
            matData.ReplaceRange(BitConverter.GetBytes((UInt32)valueDataSize), 28 + nodesSize + (nameValueIndex * 8), 4);

            matData.AddRange(Encoding.Default.GetBytes(s));
            matData.Add(0x00);
            valueDataSize += s.ToString().Length + 1;

            DecodeNameValueValues();
        }
        public void EncodeNameValueValueUInt(int nameValueIndex, UInt32 i)
        {
            matData.ReplaceRange(BitConverter.GetBytes(i), 28 + nodesSize + (nameValueIndex * 8), 4);
            DecodeNameValueValues();
        }
        public void EncodeNameValueValueFloat(int nameValueIndex, float f)
        {
            matData.ReplaceRange(BitConverter.GetBytes(f), 28 + nodesSize + (nameValueIndex * 8), 4);
            DecodeNameValueValues();
        }
        public void EncodeUVWVelocity(int nameValueIndex, float f1, float f2, float f3)
        {
            matData.ReplaceRange(BitConverter.GetBytes((UInt32)valueDataSize), 28 + nodesSize + (nameValueIndex * 8), 4);

            matData.AddRange(BitConverter.GetBytes(f1));
            matData.AddRange(BitConverter.GetBytes(f2));
            matData.AddRange(BitConverter.GetBytes(f3));
            valueDataSize += 12;

            DecodeNameValueValues();
        }

        public void SaveNewMaterial()
        {

            matData.ReplaceRange(BitConverter.GetBytes(matData.Count - 28), 8, 4);     //write new material data size to material chunks's header
            matData.ReplaceRange(BitConverter.GetBytes(valueDataSize), 24, 4);    //write new material "valueData" size to materials chunk's header

            Crc crc32 = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc32]);
            byte[] b32 = crc32.ComputeHash(matData.ToArray(), 28, matData.Count - 28);  //calc material data crc32
            matData.ReplaceRange(b32, 4, 4);                                                           //write material data crc32

            Crc crc8 = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc16Genibus]);
            byte[] tempHeader = matData.GetRange(0, 28).ToArray(); tempHeader[2] = 0x00; //temp material header without crc8
            byte[] b8 = crc8.ComputeHash(tempHeader, 0, 28);                             //calc material header crc16, gets trunicated to 8 bits
            matData.ReplaceRange(b8, 2, 1);                                              //write material header crc8
        }

        #endregion
        #region Mesh Editing

        public long[] subDataCount = new long[6];
        public long[] subDataOffset = new long[6];

        public void InitMeshEditing()
        {
            for (int i = 0; i < 6; i++)
            {
                subDataCount[i] = BitConverter.ToInt64(cachedData.GetRange(64 + (16 * i), 8).ToArray(), 0);
                subDataOffset[i] = BitConverter.ToInt64(cachedData.GetRange(72 + (16 * i), 8).ToArray(), 0);
            }
        }
        public int ReplaceMesh(Mesh mesh, int meshIndexToReplace)
        {
            Console.WriteLine("A");
            if (meshIndexToReplace >= subDataCount[0]) return -1;

            mesh.vertexSize = 24;
            int loc = (int)subDataOffset[0] + (meshIndexToReplace * 152);
            cachedData.ReplaceRange(BitConverter.GetBytes(mesh.materialID), loc, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(mesh.rigidBoneIndex), loc + 12, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(ibData.Count / 2), loc + 16, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(mesh.faceCount), loc + 20, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(vbData.Count), loc + 24, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(mesh.vertices.Count * mesh.vertexSize), loc + 28, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(mesh.vertexSize), loc + 32, 4);
            cachedData.ReplaceRange(BitConverter.GetBytes(mesh.vertices.Count), loc + 36, 4);
            List<byte> v = new List<byte>();
            if (mesh.vertexSize == 24)
            {
                for (int i = 0; i < mesh.vertices.Count; i++)
                {
                    Vector4 vec = new Vector4(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z, 1) * mesh.modelMatrix;
                    v.AddRange(SystemHalf.Half.GetBytes((Half)vec.X));
                    v.AddRange(SystemHalf.Half.GetBytes((Half)vec.Y));
                    v.AddRange(SystemHalf.Half.GetBytes((Half)vec.Z));
                    v.AddRange(SystemHalf.Half.GetBytes(1));
                    v.AddRange(BitConverter.GetBytes(mesh.vertices[i].nx));
                    v.AddRange(BitConverter.GetBytes(mesh.vertices[i].ny));
                    v.AddRange(BitConverter.GetBytes(mesh.vertices[i].nz));
                    v.AddRange(SystemHalf.Half.GetBytes(mesh.vertices[i].u));
                    v.AddRange(SystemHalf.Half.GetBytes(mesh.vertices[i].v));
                }
            }
            vbData.AddRange(v);

            for (int i = 0; i < mesh.indices.Count; i++)
            {
                ibData.AddRange(BitConverter.GetBytes((short)mesh.indices[i]));
            }

            Console.WriteLine("B");
            return 1;
        }
        public List<Mesh> GetMeshes()
        {
            List<Mesh> meshes = new List<Mesh>();
            Bone grannyRootBone = GetBones()[0];

            for(int i = 0; i < subDataCount[0]; i++)
            {
                Mesh m = new Mesh();
                int loc = (int)subDataOffset[0] + (i * 152);
                m.materialID = BitConverter.ToInt32(cachedData.GetRange(loc, 4).ToArray(), 0);
                m.accessoryIndex = BitConverter.ToInt32(cachedData.GetRange(loc + 4, 4).ToArray(), 0);
                m.maxBones = BitConverter.ToInt32(cachedData.GetRange(loc + 8, 4).ToArray(), 0);
                m.rigidBoneIndex = BitConverter.ToInt32(cachedData.GetRange(loc + 12, 4).ToArray(), 0);
                m.indexBufferOffsetInShorts = BitConverter.ToInt32(cachedData.GetRange(loc + 16, 4).ToArray(), 0);
                m.faceCount = BitConverter.ToInt32(cachedData.GetRange(loc + 20, 4).ToArray(), 0);
                m.vertexBufferOffsetInBytes = BitConverter.ToInt32(cachedData.GetRange(loc + 24, 4).ToArray(), 0);
                m.vertexBufferSizeInBytes = BitConverter.ToInt32(cachedData.GetRange(loc + 28, 4).ToArray(), 0);
                m.vertexSize = BitConverter.ToInt32(cachedData.GetRange(loc + 32, 4).ToArray(), 0);
                m.vertexCount = BitConverter.ToInt32(cachedData.GetRange(loc + 36, 4).ToArray(), 0);
                m.rigidOnly = BitConverter.ToBoolean(cachedData.GetRange(loc + 124, 1).ToArray(), 0);

                for (int j = 0; j < m.faceCount; j++)
                {
                    m.indices.Add((uint)BitConverter.ToInt16(ibData.GetRange((m.indexBufferOffsetInShorts * 2) + (j * 6) + 0, 2).ToArray(), 0));
                    m.indices.Add((uint)BitConverter.ToInt16(ibData.GetRange((m.indexBufferOffsetInShorts * 2) + (j * 6) + 2, 2).ToArray(), 0));
                    m.indices.Add((uint)BitConverter.ToInt16(ibData.GetRange((m.indexBufferOffsetInShorts * 2) + (j * 6) + 4, 2).ToArray(), 0));
                }
                for(int k = 0; k < m.vertexCount; k++)
                {
                    Mesh.Vertex v = new Mesh.Vertex();

                    if(m.vertexSize == 0x18)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 20, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 22, 2).ToArray(), 0);
                    }
                    if(m.vertexSize == 0x1c)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 20, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 22, 2).ToArray(), 0);
                    }
                    if(m.vertexSize == 0x20)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 28, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 30, 2).ToArray(), 0);
                    }
                    if (m.vertexSize == 0x24)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 32, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 34, 2).ToArray(), 0);
                    }
                    if (m.vertexSize == 0x28)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 32, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 34, 2).ToArray(), 0);
                    }
                    if (m.vertexSize == 0x2c)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 40, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 42, 2).ToArray(), 0);
                    }
                    if (m.vertexSize == 0x30)
                    {
                        v.x = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 0, 2).ToArray(), 0);
                        v.y = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 2, 2).ToArray(), 0);
                        v.z = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 4, 2).ToArray(), 0);
                        v.nx = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 8, 4).ToArray(), 0);
                        v.ny = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 12, 4).ToArray(), 0);
                        v.nz = BitConverter.ToSingle(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 16, 4).ToArray(), 0);
                        v.u = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 40, 2).ToArray(), 0);
                        v.v = Half.ToHalf(vbData.GetRange(m.vertexBufferOffsetInBytes + (k * m.vertexSize) + 42, 2).ToArray(), 0);
                    }
                    v.v = 1 - v.v;
                    m.vertices.Add(v);
                }
                m.rootMatrix = new Matrix4(-1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, 0, 0, 0, 0, 1) * grannyRootBone.boneMatrix;
                meshes.Add(m);
            }
            return meshes;
        }
        public List<Bone> GetBones()
        {
            List<Bone> bones = new List<Bone>();

            for(int i = 0; i < subDataCount[1]; i++)
            {
                Bone b = new Bone();
                int loc = (int)subDataOffset[1] + (i*80) + 4;
                float m11 = BitConverter.ToSingle(cachedData.GetRange(loc + 4, 4).ToArray(), 0);
                float m12 = BitConverter.ToSingle(cachedData.GetRange(loc + 8, 4).ToArray(), 0);
                float m13 = BitConverter.ToSingle(cachedData.GetRange(loc + 12, 4).ToArray(), 0);
                float m14 = BitConverter.ToSingle(cachedData.GetRange(loc + 16, 4).ToArray(), 0);

                float m21 = BitConverter.ToSingle(cachedData.GetRange(loc + 20, 4).ToArray(), 0);
                float m22 = BitConverter.ToSingle(cachedData.GetRange(loc + 24, 4).ToArray(), 0);
                float m23 = BitConverter.ToSingle(cachedData.GetRange(loc + 28, 4).ToArray(), 0);
                float m24 = BitConverter.ToSingle(cachedData.GetRange(loc + 32, 4).ToArray(), 0);

                float m31 = BitConverter.ToSingle(cachedData.GetRange(loc + 36, 4).ToArray(), 0);
                float m32 = BitConverter.ToSingle(cachedData.GetRange(loc + 40, 4).ToArray(), 0);
                float m33 = BitConverter.ToSingle(cachedData.GetRange(loc + 44, 4).ToArray(), 0);
                float m34 = BitConverter.ToSingle(cachedData.GetRange(loc + 48, 4).ToArray(), 0);

                float m41 = BitConverter.ToSingle(cachedData.GetRange(loc + 52, 4).ToArray(), 0);
                float m42 = BitConverter.ToSingle(cachedData.GetRange(loc + 56, 4).ToArray(), 0);
                float m43 = BitConverter.ToSingle(cachedData.GetRange(loc + 60, 4).ToArray(), 0);
                float m44 = BitConverter.ToSingle(cachedData.GetRange(loc + 64, 4).ToArray(), 0);

                b.parent = BitConverter.ToInt32(cachedData.GetRange(loc + 68, 4).ToArray(), 0);
                b.name = Utils.GetStringFromNullTerminatedByteArray(cachedData.ToArray(), BitConverter.ToInt32(cachedData.GetRange(loc - 4, 4).ToArray(), 0));

                Matrix4 m = new Matrix4();

                m.M11 = m11;
                m.M12 = m12;
                m.M13 = m13;
                m.M14 = m14;

                m.M21 = m21;
                m.M22 = m22;
                m.M23 = m23;
                m.M24 = m24;

                m.M31 = m31;
                m.M32 = m32;
                m.M33 = m33;
                m.M34 = m34;

                m.M41 = m41;
                m.M42 = m42;
                m.M43 = m43;
                m.M44 = m44;

                b.boneMatrix = m.Inverted();
                //if (i > 0) b.rootMatrix = bones[0].boneMatrix.Inverted();
                //else b.rootMatrix = Matrix4.Identity.Inverted();
                bones.Add(b);
            }
            return bones;
        }
        #endregion
    }
}