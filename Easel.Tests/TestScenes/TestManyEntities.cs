using System;
using System.IO;
using System.Numerics;
using Easel.Entities;
using Easel.Entities.Components;
using Easel.Graphics;
using Easel.Math;
using Easel.Scenes;

namespace Easel.Tests.TestScenes;

public class TestManyEntities : Scene
{
    private Texture2D[] _textures;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        Camera.Main.UseOrtho2D();

        string[] files = Directory.GetFiles(@"C:\Users\ollie\Pictures", "*.png");
        _textures = new Texture2D[files.Length];

        int i = 0;
        foreach (string file in files)
        {
            Console.Write($"Loading {file}... ");
            _textures[i] = new Texture2D(file);
            Console.WriteLine("Done!");
            i++;
        }
        
        Console.WriteLine(_textures.Length);

        /*Texture2D texture = Content.Load<Texture2D>("awesomeface");
        Random random = new Random();
        
        for (int i = 0; i < 13457; i++)
        {
            Entity entity = new Entity(null, new Transform()
            {
                Position = new Vector3(random.Next(0, 1280), random.Next(0, 720), 0),
                SpriteRotation = random.NextSingle() * MathF.PI * 2,
                Scale = new Vector3(random.NextSingle() * 0.5f, random.NextSingle() * 0.5f, 1.0f)
            });
            entity.AddComponent(new Sprite(texture));
            AddEntity(entity);
        }*/
    }

    protected override void Draw()
    {
        base.Draw();
        
        Graphics.SpriteRenderer.Begin();

        int i = 0;
        foreach (Texture2D texture in _textures)
        {
            Graphics.SpriteRenderer.Draw(texture, new Vector2(i * 100), null, Color.White, 0, Vector2.Zero,
                Vector2.One);

            i++;
        }

        Graphics.SpriteRenderer.End();
    }
}