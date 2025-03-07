#include "Shapes.hlsl"

struct VSInput
{
    float2 position:  POSITION;
    float2 texCoords: TEXCOORD0;
    float4 tint:      COLOR0;
    float1 rotation:  TEXCOORD1;
    float2 origin:    TEXCOORD2;
    float2 scale:     TEXCOORD3;
    float4 meta1:     TEXCOORD4;
    float4 meta2:     TEXCOORD5;
};

struct VSOutput
{
    float4 position:  SV_Position;
    float2 texCoords: TEXCOORD0;
    float4 tint:      COLOR0;
    float4 meta1:     TEXCOORD1;
    float4 meta2:     TEXCOORD2;
};

struct PSOutput
{
    float4 color: SV_Target0;
};

cbuffer ProjView : register(b0)
{
    float4x4 projView;
};

Texture2D sprite   : register(t1);
SamplerState state : register(s1);

// Constant options
// 0 = Standard. Draw a sprite as usual.
// 1 = Blur. Blur the sprite.
// 2 = SDF Rounded Rectangle
[[vk::constant_id(0)]] const uint options = 0;

VSOutput VertexShader(in VSInput input)
{
    VSOutput output;

    float cosRot = cos(input.rotation);
    float sinRot = sin(input.rotation);

    float2 vertexPos = input.position - (input.origin * input.scale);
    float2x2 rot = float2x2(
        cosRot, sinRot,
        -sinRot, cosRot
    );

    vertexPos = mul(rot, vertexPos);
    vertexPos += input.origin * input.scale;

    output.position = mul(projView, float4(vertexPos, 0.0, 1.0));
    output.texCoords = input.texCoords;
    output.tint = input.tint;
    output.meta1 = input.meta1;
    output.meta2 = input.meta2;

    return output;
}

PSOutput PixelShader(in VSOutput input)
{
    PSOutput output;

    if (options == 1)
    {
        float4 color = 0.0;
        float2 direction = input.meta1.xy;
        float2 resolution = input.meta1.zw;
        float2 off1 = 1.3846153846 * direction;
        float2 off2 = 3.2307692308 * direction;

        color += sprite.Sample(state, input.texCoords) * 0.2270270270;
        color += sprite.Sample(state, input.texCoords + (off1 / resolution)) * 0.3162162162;
        color += sprite.Sample(state, input.texCoords - (off1 / resolution)) * 0.3162162162;
        color += sprite.Sample(state, input.texCoords + (off2 / resolution)) * 0.0702702703;
        color += sprite.Sample(state, input.texCoords - (off2 / resolution)) * 0.0702702703;

        output.color = color;
    }
    else if (options == 2)
    {
        SDFResult result = RoundedRect(input.meta1.zw, input.meta1.xy, input.texCoords);
        float4 toColor = result.dist < 0.0 ? sprite.Sample(state, input.texCoords) * input.tint : (float4) 0;
        output.color = lerp(input.meta2, toColor, result.blendAmount);
    }
    else
        output.color = sprite.Sample(state, input.texCoords) * input.tint;
    
    return output;
}