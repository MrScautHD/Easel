using System;
using System.Numerics;
using Easel.Entities.Components;
using Easel.Graphics.Renderers.Structs;
using Easel.Math;

namespace Easel.Graphics.Lighting;

public class DirectionalLight : Component
{
    private Vector2 _direction;
    private Vector3 _position;

    public Vector2 Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            float theta = -value.Y;
            float phi = value.X;
            _position = new Vector3(MathF.Cos(phi) * MathF.Cos(theta), MathF.Cos(phi) * MathF.Sin(theta),
                MathF.Sin(phi));
            
            Console.WriteLine(_position);
        }
    }
    
    public Color Color;

    public bool CastShadows;

    public DirectionalLight(Vector2 direction, Color color, bool castShadows = true)
    {
        Direction = direction;
        Color = color;
        CastShadows = castShadows;
    }

    public ShaderDirLight ShaderDirLight => new ShaderDirLight()
    {
        Direction = new Vector4(_position, 0),
        Color = Color
    };
}