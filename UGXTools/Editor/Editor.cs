using System;
using System.Collections.Generic;
using Assimp;
using SystemHalf;


class Editor
{
    public struct Model
    {
        public struct VertexBase
        {
            public Half  vx, vy, vz,
                         tu, tv;
            public float nx, ny, nz;
        }
        public List<List<VertexBase>> meshes;
        public List<int> indices;
    }
    public Model ImportAsset(string path)
    {
        //use assimp to load a model.
        AssimpContext imp = new AssimpContext();
        Assimp.Configs.RemoveDegeneratePrimitivesConfig c = new Assimp.Configs.RemoveDegeneratePrimitivesConfig(false);
        imp.SetConfig(c);
        Scene asset = imp.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FindDegenerates);

        Model model = new Model();
        model.meshes = new List<List<Model.VertexBase>>();
        model.indices = new List<int>();

        //for each mesh in the imported file, collect vertices and indices.
        for (int i = 0; i < asset.MeshCount; i++)
        {
            List<Model.VertexBase> thisMeshVertices = new List<Model.VertexBase>();

            //collet vertices for this mesh
            for (int ve = 0; ve < asset.Meshes[i].VertexCount; ve++)
            {
                Model.VertexBase v = new Model.VertexBase();
                v.vx = (Half)asset.Meshes[i].Vertices[ve].X;
                v.vy = (Half)asset.Meshes[i].Vertices[ve].Y;
                v.vz = (Half)asset.Meshes[i].Vertices[ve].Z;
                v.nx = (Half)asset.Meshes[i].Normals[ve].X;
                v.ny = (Half)asset.Meshes[i].Normals[ve].Y;
                v.nz = (Half)asset.Meshes[i].Normals[ve].Z;
                v.tu = (Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].X;
                v.tv = (Half)asset.Meshes[i].TextureCoordinateChannels[0][ve].Y;
                thisMeshVertices.Add(v);
            }
            //collect indices for this mesh
            for (int j = 0; j < asset.Meshes[i].FaceCount; j++)
            {
                for (int k = 0; k < asset.Meshes[i].Faces[j].IndexCount; k++)
                {
                    model.indices.Add(asset.Meshes[i].Faces[j].Indices[k]);
                }
            }

            model.meshes.Add(thisMeshVertices);
        }

        return model;
    }

    public void OverwriteUGXMeshData(Model asset, UGXFile ugx)
    {
        //write cached data's sub data

    }
}
