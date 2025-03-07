using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Easel.Graphics.Primitives;
using Easel.Core;
using Pie;
using Pie.ShaderCompiler;
using Pie.Utils;

namespace Easel.Graphics;

public class Skybox : IDisposable
{
    public SamplerState SamplerState;
    
    public readonly Pie.Texture PieTexture;
    private GraphicsBuffer _vertexBuffer;
    private GraphicsBuffer _indexBuffer;
    private GraphicsBuffer _cameraBuffer;
    
    private DepthStencilState _depthState;
    private Pie.RasterizerState _rasterizerState;

    private Effect _effect;
    private InputLayout _inputLayout;

    private CameraInfo _cameraInfo;

    private GraphicsDevice _device;

    public Skybox(Bitmap right, Bitmap left, Bitmap top, Bitmap bottom, Bitmap front, Bitmap back, SamplerState samplerState = null)
    {
        _device = EaselGraphics.Instance.PieGraphics;

        byte[] data = PieUtils.Combine<byte>(right.Data, left.Data, top.Data, bottom.Data, front.Data, back.Data);

        PieTexture =
            _device.CreateTexture(
                new TextureDescription(TextureType.Cubemap, right.Size.Width, right.Size.Height, 0, right.Format, 1, 1,
                    TextureUsage.ShaderResource), data);

        Cube cube = new Cube();
        VertexPositionTextureNormalTangent[] vptnts = cube.Vertices;
        VertexPosition[] vps = new VertexPosition[vptnts.Length];
        for (int i = 0; i < vptnts.Length; i++)
        {
            vps[i] = new VertexPosition((Vector3) vptnts[i].Position);
        }

        _vertexBuffer = _device.CreateBuffer(BufferType.VertexBuffer, vps);
        _indexBuffer = _device.CreateBuffer(BufferType.IndexBuffer, cube.Indices);

        _cameraBuffer = _device.CreateBuffer(BufferType.UniformBuffer, _cameraInfo, true);
        
        _depthState = _device.CreateDepthStencilState(DepthStencilStateDescription.LessEqual);
        _rasterizerState = _device.CreateRasterizerState(RasterizerStateDescription.CullCounterClockwise);

        _effect = Effect.FromPath("Easel.Graphics.Shaders.Skybox_vert.spv", "Easel.Graphics.Shaders.Skybox_frag.spv");

        _inputLayout =
            _device.CreateInputLayout(new InputLayoutDescription(Format.R32G32B32_Float, 0, 0, InputType.PerVertex));

        SamplerState = samplerState ?? SamplerState.LinearClamp;
    }

    internal void Draw(Matrix4x4 projection, Matrix4x4 view)
    {
        _cameraInfo.Projection = projection;
        _cameraInfo.View = view.To3x3Matrix();
        
        _device.UpdateBuffer(_cameraBuffer, 0, _cameraInfo);
        
        _device.SetShader(_effect.PieShader);
        _device.SetPrimitiveType(PrimitiveType.TriangleList);
        _device.SetUniformBuffer(0, _cameraBuffer);
        _device.SetTexture(1, PieTexture, SamplerState.PieSamplerState);
        _device.SetDepthStencilState(_depthState);
        _device.SetRasterizerState(_rasterizerState);
        _device.SetVertexBuffer(0, _vertexBuffer, VertexPosition.SizeInBytes, _inputLayout);
        _device.SetIndexBuffer(_indexBuffer, IndexType.UInt);
        // 36 because cube
        _device.DrawIndexed(36);
    }
    
    [StructLayout(LayoutKind.Sequential)]
    private struct CameraInfo
    {
        public Matrix4x4 Projection;
        public Matrix4x4 View;
    }

    public void Dispose()
    {
        PieTexture.Dispose();
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _cameraBuffer.Dispose();
        _depthState.Dispose();
        _rasterizerState.Dispose();
        _effect.Dispose();
        _inputLayout.Dispose();
    }
}