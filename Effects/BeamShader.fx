sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float2 uTargetPosition;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;
    
float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
	float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
	
	float waveY = 1 - frac((1 - frameY) + uTime * 0.5); //up to down
	
	color.rgb *= color.rgb + (uColor / (waveY * 5));
	
    return color * sampleColor;
}

technique Technique1
{
    pass BeamDyePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}