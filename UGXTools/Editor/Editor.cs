using System;
using System.Collections.Generic;
using Assimp;
using SystemHalf;


class Editor
{
    public struct Model
    {
        public struct Mesh
        {
            public struct Vertex
        {
            public Half  vx, vy, vz,
                         tu, tv;
            public float nx, ny, nz;
            public byte  b1, b2, b3, b4,
                         w1, w2, w3, w4;
        }

            public List<Vertex> vertices;
            public List<int> indices;
            public int
                materialID,
                boneID,
                vertexSize,
                faceCount;
            public bool isSkinned;
        }
        public List<Mesh> meshes;
    }
    public Model ImportAsset(string path)
    {
        //use assimp to load a model.
        AssimpContext imp = new AssimpContext();
        Assimp.Configs.RemoveDegeneratePrimitivesConfig c = new Assimp.Configs.RemoveDegeneratePrimitivesConfig(false);
        imp.SetConfig(c);
        Scene asset = imp.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FindDegenerates);

        Model model = new Model();
        model.meshes = new List<Model.Mesh>();
        Console.WriteLine(asset.MeshCount);
        //for each mesh in the imported file, collect vertices and indices.
        for (int i = 0; i < asset.MeshCount; i++)
        {
            Model.Mesh mesh = new Model.Mesh();
            mesh.vertices = new List<Model.Mesh.Vertex>();
            mesh.indices = new List<int>();
            mesh.isSkinned = false;
            mesh.vertexSize = 24;

            //collet vertices for this mesh
            for (int ve = 0; ve < asset.Meshes[i].VertexCount; ve++)
            {
                Model.Mesh.Vertex v = new Model.Mesh.Vertex();
                v.vx = (Half)asset.Meshes[i].Vertices[ve].X;
                v.vy = (Half)asset.Meshes[i].Vertices[ve].Y;
                v.vz = (Half)asset.Meshes[i].Vertices[ve].Z;
                v.nx = (Half)asset.Meshes[i].Normals[ve].X;
                v.ny = (Half)asset.Meshes[i].Normals[ve].Y;
                v.nz = (Half)asset.Meshes[i].Normals[ve].Z;
                v.tu = (Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].X;
                v.tv = (Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].Y;
                mesh.vertices.Add(v);
            }
            //collect indices for this mesh
            for (int j = 0; j < asset.Meshes[i].FaceCount; j++)
            {
                for (int k = 0; k < asset.Meshes[i].Faces[j].IndexCount; k++)
                {
                    mesh.indices.Add(asset.Meshes[i].Faces[j].Indices[k]);
                }
                mesh.faceCount++;
            }

            model.meshes.Add(mesh);
        }

        return model;
    }

    public void AddMesh(Model.Mesh mesh, UGXFile ugx)
    {
        int q = (int)ugx.subDataOffset[0] + ((int)ugx.subDataCount[0] * 152);
        List<byte> allDataAfterMeshSubData = ugx.cachedData.GetRange(q, ugx.cachedData.Count - q);

        List<byte> newMeshSubData = new List<byte>();
        //populate new subData.
        {
            newMeshSubData.AddRange(BitConverter.GetBytes(mesh.materialID));                                        //4   materialID
            newMeshSubData.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 });                 //12  polyID, ukwID
            newMeshSubData.AddRange(BitConverter.GetBytes(mesh.boneID));                                            //16  boneID
            newMeshSubData.AddRange(BitConverter.GetBytes(0));                                   //20  faceOffset/indexOffset
            newMeshSubData.AddRange(BitConverter.GetBytes((mesh.indices.Count - (mesh.indices.Count % 3)) / 3));    //24  faceCount
            newMeshSubData.AddRange(BitConverter.GetBytes(0));                                       //28  vertexOffset
            newMeshSubData.AddRange(BitConverter.GetBytes(mesh.vertices.Count * mesh.vertexSize));                  //32  vertexDataLength
            newMeshSubData.AddRange(BitConverter.GetBytes(mesh.vertexSize));                                        //36  vertexSize
            newMeshSubData.AddRange(BitConverter.GetBytes(mesh.vertices.Count));                                    //40  vertexCount
            newMeshSubData.AddRange(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,                     
                                    0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00 });                                                                 //56  16 unknown bytes.
            newMeshSubData.AddRange(ugx.cachedData.GetRange((int)ugx.subDataOffset[0] + 56, 4).ToArray());          //60  name value. pretty much useless afaik. uses meshSubData[0]'s value.
            newMeshSubData.AddRange(new byte[] {
                0x00, 0x00, 0x00, 0x00,
                0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00,
                0x09, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});                                                                 //152 92 unknown bytes.
        }

        ugx.cachedData.ReplaceRange(newMeshSubData.ToArray(), (int)ugx.subDataOffset[0] + ((int)ugx.subDataCount[0] * 152), 152); //add in new meshSubData.
        
        //write cached data's sub data
        ugx.subDataCount[0]++;
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(ugx.subDataCount[0]), 64, 8);  //write the number of meshes to the meshSubData header.

        for (int i = 1; i < 6; i++)
        {
            ugx.subDataOffset[i] += 152;
            ugx.cachedData.ReplaceRange(BitConverter.GetBytes(ugx.subDataOffset[i]), 72 + (i * 16), 8); //fix offsets of data after mesh data.
        }

        ugx.cachedData.ReplaceRange(allDataAfterMeshSubData.ToArray(), (int)ugx.subDataOffset[0] + ((int)ugx.subDataCount[0] * 152), 0);

        //calc new name offsets for meshSubDatas
        int newOffset = 152 + BitConverter.ToInt32(ugx.cachedData.GetRange((int)ugx.subDataOffset[0] + 56, 4).ToArray(), 0);
        Console.WriteLine(newOffset);
        for (int i = 0; i < ugx.subDataCount[0]; i++)
        {
            //TODO
        }

        //calc new name offsets for boneSubData
        int newBoneNameOffset = 152 + BitConverter.ToInt32(ugx.cachedData.GetRange((int)ugx.subDataOffset[1], 4).ToArray(), 0);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(newBoneNameOffset), (int)ugx.subDataOffset[1], 4);
        Console.WriteLine(newBoneNameOffset);
    } //not currently working
    public int ReplaceMesh(Model.Mesh mesh, UGXFile ugx, int meshIndexToReplace)
    {
        if (meshIndexToReplace >= ugx.subDataCount[0]) return -1;

        int loc = (int)ugx.subDataOffset[0] + (meshIndexToReplace * 152);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(mesh.materialID), loc, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(mesh.boneID), loc + 12, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(ugx.ibData.Count / 2), loc + 16, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(mesh.faceCount), loc + 20, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(ugx.vbData.Count), loc + 24, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(mesh.vertices.Count * mesh.vertexSize), loc + 28, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(mesh.vertexSize), loc + 32, 4);
        ugx.cachedData.ReplaceRange(BitConverter.GetBytes(mesh.vertices.Count), loc + 36, 4);

        List<byte> v = new List<byte>();
        if(mesh.vertexSize == 24)
        {
            for (int i = 0; i < mesh.vertices.Count; i++) 
            {
                v.AddRange(Half.GetBytes(mesh.vertices[i].vx));
                v.AddRange(Half.GetBytes(mesh.vertices[i].vy));
                v.AddRange(Half.GetBytes(mesh.vertices[i].vz));
                v.AddRange(Half.GetBytes(1));
                v.AddRange(BitConverter.GetBytes(mesh.vertices[i].nx));
                v.AddRange(BitConverter.GetBytes(mesh.vertices[i].ny));
                v.AddRange(BitConverter.GetBytes(mesh.vertices[i].nz));
                v.AddRange(Half.GetBytes(mesh.vertices[i].tu));
                v.AddRange(Half.GetBytes(mesh.vertices[i].tv));
            }
        }
        ugx.vbData.AddRange(v);

        for (int i = 0; i < mesh.indices.Count; i++)
        {
            ugx.ibData.AddRange(BitConverter.GetBytes((short)mesh.indices[i]));
        }
        
        return 1;
    }
}
