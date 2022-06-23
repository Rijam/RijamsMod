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
	
	//float waveX = 1 - frac(coords.x + uTime * 0.75); //right to left
	//float waveY = 1 - frac((1 - frameY) + uTime * 0.75); //up to down
	
	float waveXY = 1 - frac((coords.x + (1 - frameY)) + uTime * 0.75); 
	
	//float waveX2 = 1 - frac(coords.x - uTime); //left to right
	//float waveY2 = 1 - frac((1 - frameY) - uTime); //down to up
	//float waveXo = 1 - frac(coords.x + uTime); //right to left
	//float waveYo = 1 - frac((1 - frameY )) + uTime); //up to down
	
	float luminosity = (color.r + color.g + color.b) / 3;
	
	//color.rgb = luminosity * uColor;
	//color.rgb = ((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * luminosity;
	//color.rgb *= (((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * ((frameY * uSecondaryColor) + ((1 - frameY) * uColor))) * luminosity;
	//color.rgb *= (((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * ((frameY * uSecondaryColor) + ((1 - frameY) * uColor))) * luminosity / (waveX + waveX2) / (waveY + waveY2);
	//color.rgb *= (((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * ((frameY * uSecondaryColor) + ((1 - frameY) * uColor))) * luminosity / waveX  / waveY;
	
	color.rgb *= (((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * ((frameY * uSecondaryColor) + ((1 - frameY) * uColor))) * luminosity / waveXY;
    return color * sampleColor;
}

technique Technique1
{
    pass YellaDyePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}