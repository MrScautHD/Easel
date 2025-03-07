using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Easel.Core;
using Easel.Graphics.Renderers.Structs;
using Easel.Math;
using Pie;
using Pie.ShaderCompiler;

namespace Easel.Graphics.Materials;

/// <summary>
/// A material represents a set of parameters that tells Easel how to render an object.
/// </summary>
public abstract class Material : IDisposable
{
    public const int TextureBindingLoc = 3;

    private static Dictionary<int, MaterialCache> _cache;
    private int _hash;
    
    public bool IsDisposed { get; private set; }

    static Material()
    {
        _cache = new Dictionary<int, MaterialCache>();
    }

    /// <summary>
    /// The <see cref="Easel.Graphics.EffectLayout"/> of this material.
    /// Each material will contain its own <see cref="Easel.Graphics.EffectLayout"/>.
    /// </summary>
    public EffectLayout EffectLayout { get; protected set; }
    
    /// <summary>
    /// Is <see langword="true" /> when using a translucent material, such as <see cref="TranslucentStandardMaterial"/>.
    /// </summary>
    public bool IsTranslucent { get; protected set; }
    
    public abstract ShaderMaterial ShaderMaterial { get; }

    /// <summary>
    /// How much the texture will tile. (Default: 1)
    /// </summary>
    public Vector2 Tiling;

    /// <summary>
    /// The rasterizer state of this material. (Default: CullClockwise)
    /// </summary>
    public RasterizerState RasterizerState;
    
    // TODO: Depth states
    public BlendState BlendState;

    protected Material()
    {
        Tiling = Vector2.One;
        RasterizerState = RasterizerState.CullCounterClockwise;
        BlendState = BlendState.NonPremultiplied;
        DisposeManager.AddItem(this);
    }

    protected internal abstract void ApplyTextures(GraphicsDevice device);

    protected EffectLayout GetEffectLayout(byte[] vShader, byte[] fShader, SpecializationConstant[] constants, InputLayoutDescription[] descriptions, uint stride)
    {
        // TODO: Potentially a better way of getting a hash code?

        _hash = unchecked(Convert.ToBase64String(vShader).GetHashCode() +
                          Convert.ToBase64String(fShader).GetHashCode() +
                          constants.Sum(constant => constant.GetHashCode()));
        if (!_cache.TryGetValue(_hash, out MaterialCache cache))
        {
            Logger.Debug($"Creating new material cache. (ID: {_hash})");
            InputLayout layout = EaselGraphics.Instance.PieGraphics.CreateInputLayout(descriptions);

            cache = new MaterialCache()
            {
                EffectLayout = new EffectLayout(new Effect(vShader, fShader, constants), layout, stride),
                NumReferences = 0
            };
            
            _cache.Add(_hash, cache);
        }

        cache.NumReferences++;
        return cache.EffectLayout;
    }

    public virtual void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        
        MaterialCache cache = _cache[_hash];
        cache.NumReferences--;
        if (cache.NumReferences <= 0)
        {
            Logger.Debug($"Disposing of material cache. (ID: {_hash})");
            cache.EffectLayout.Dispose();
            _cache.Remove(_hash);
        }
        
        Logger.Debug("Material disposed.");
    }
    
    private class MaterialCache
    {
        public EffectLayout EffectLayout;
        public int NumReferences;
    }
}