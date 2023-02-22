using System;
using System.Numerics;
using Easel.Entities;
using Easel.Entities.Components;
using Easel.Graphics;
using Easel.Graphics.Materials;
using Easel.Graphics.Primitives;
using Easel.Math;
using Easel.Scenes;

namespace Easel.Tests.TestScenes;

public class TestShadow : Scene
{
    protected override void Initialize()
    {
        base.Initialize();

        Camera.Main.ClearColor = Color.CornflowerBlue;
        Bitmap bitmap = new Bitmap("/home/ollie/Pictures/ball.png");
        Camera.Main.Skybox = new Skybox(bitmap, bitmap, bitmap, bitmap, bitmap, bitmap, SamplerState.PointClamp);
        Camera.Main.AddComponent(new NoClipCamera()
        {
            MoveSpeed = 5
        });

        GetEntity("Sun").GetComponent<DirectionalLight>().Direction = new Vector2<float>(0, 1);

        Entity entity = new Entity(new Transform()
        {
            Position = new Vector3(0, 0, -3),
        });
        //entity.AddComponent(new ModelRenderer(new Cube(), new StandardMaterial()));
        Model model = new Model("/home/ollie/Downloads/Fox.gltf");
        /*Material material = new StandardMaterial(new Texture2D("/home/ollie/Downloads/Cubebs/no parking.jpg"));
        for (int i = 0; i < model.Meshes.Length; i++)
        {
            ref ModelMesh mMesh = ref model.Meshes[i];
            for (int j = 0; j < mMesh.Meshes.Length; j++)
            {
                mMesh.Meshes[j].Material = material;
            }
        }*/
        entity.AddComponent(new ModelRenderer(model));
        AddEntity(entity);
    }
}