using System;
using System.Numerics;
using Easel.Core;
using Easel.Graphics.Renderers;
using Easel.Math;
using Pie;
using Pie.ShaderCompiler;

namespace Easel.Graphics.Effects;

public class GaussianBlur : IDisposable
{
    private static Effect _effect;
    
    private RenderTarget _target;
    private RenderTarget _target2;
    
    public Texture Texture;

    public float Radius;

    public int Iterations;

    public GaussianBlur(Texture texture, float radius, int iterations, bool autoDispose = true)
    {
        Texture = texture;
        Radius = radius;
        Iterations = iterations;

        _target = new RenderTarget(texture.Size, autoDispose: false);
        _target2 = new RenderTarget(texture.Size, autoDispose: false);

        _effect ??= Effect.FromPath("Easel.Graphics.Shaders.SpriteRenderer.Sprite_vert.spv",
            "Easel.Graphics.Shaders.SpriteRenderer.Sprite_frag.spv",
            constants: new[] { new SpecializationConstant(0, 1u) });
        
        if (autoDispose)
            DisposeManager.AddItem(this);
    }

    public Texture Blur()
    {
        EaselGraphics graphics = EaselGraphics.Instance;
        SpriteRenderer renderer = graphics.SpriteRenderer;
        
        graphics.SetRenderTarget(_target);
        graphics.Clear(Color.Black);
        //graphics.Viewport = new Rectangle<int>(Vector2T<int>.Zero, graphics.MainTarget.Size);

        renderer.Begin(blendState: BlendState.DisabledRgbMask);

        renderer.Draw(Texture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One);
        
        renderer.End();

        for (int i = 0; i < Iterations; i++)
        {
            float radius = (Iterations - i - 1) * Radius;
            Vector2 direction = i % 2 == 0 ? new Vector2(radius, 0) : new Vector2(0, radius);
            
            graphics.SetRenderTarget(_target2);
            renderer.Begin(effect: _effect, blendState: BlendState.Disabled);

            renderer.Draw(_target, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One,
                meta1: new Vector4((System.Numerics.Vector2) direction, _target.Size.Width, _target.Size.Height));
            
            renderer.End();
            graphics.SetRenderTarget(null);
            (_target, _target2) = (_target2, _target);
        }

        return _target;
    }

    public void Dispose()
    {
        _target.Dispose();
        _target2.Dispose();
    }
}