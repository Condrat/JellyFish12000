float4x4 View;
float4x4 Projection;
float2 ViewportScale;
float ParticleSize;
texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);    
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Point;    
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
    float2 Corner : POSITION0;
    float3 Position : POSITION1;
    float4 Color: COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : COLOR1;
};

VertexShaderOutput ParticleVertexShader(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(mul(float4(input.Position, 1), View), Projection);
	float size = ParticleSize * Projection._m11;
    output.Position.xy += input.Corner * size * ViewportScale;
    output.Color = input.Color;
    output.TextureCoordinate = (input.Corner + 1) / 2;
    
    return output;
}

float4 ParticlePixelShader(VertexShaderOutput input) : COLOR0
{
    return tex2D(Sampler, input.TextureCoordinate) * input.Color;
}

technique Particles
{
    pass P0
    {
        VertexShader = compile vs_2_0 ParticleVertexShader();
        PixelShader = compile ps_2_0 ParticlePixelShader();
    }
}