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

float4 OutlineShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	// Help from https://blog.febucci.com/2019/06/sprite-outline-shader/
	
	// Get the color at the selected coordinate.
	float4 color = tex2D(uImage0, coords);
	
	// Set what is up and what is right.
	float2 up = float2(0, 1 / uImageSize0.y);
	float2 right = float2(1 / uImageSize0.x, 0);
	
	// Get the pixel at the coord + up/right * 2.
	// * 2 because Terraria pixels are actually 2x2.
	float pixelUp2 = tex2D(uImage0, coords + up * 2).a;
	float pixelRight2 = tex2D(uImage0, coords + right * 2).a;
	float pixelDown2 = tex2D(uImage0, coords - up * 2).a;
	float pixelLeft2 = tex2D(uImage0, coords - right * 2).a;
	
	// Inline will draw on the inside of the texture.
	// float inline = (1 - pixelLeft2 * pixelUp2 * pixelRight2 * pixelDown2) * color.a;
	
	// Figure out if it is the outside of the texture.
	float outline = max(max(pixelLeft2, pixelUp2), max(pixelRight2, pixelDown2)) - color.a;

	// Create a new color for the outline color.
	float4 outlineColor = float4(1, 1, 1, 1);
	outlineColor.rgb *= uColor; // multiply it by the color passed through the dye color argument.

	// Either the pixel will be the normal color (color * sampleColor) affected by lighting or it will be the outline color.
	// outline will be:
	//   0 -> Original color with lighting
	//   1 -> outlineColor
	return lerp(color * sampleColor, outlineColor, outline);
	
	// For the inline
	// return lerp(color * sampleColor, color, outline); Inline is full bright original color, rest is affected by lighting. Looks pretty cool in the dark.
	// return lerp(float4(0,0,0,0), color, outline); // Only the original color as the inline, rest is invisible.
	// return lerp(float4(0,0,0,color.a), color, outline); // Only the original color as the inline, rest is black.
}

technique Technique1
{
	pass OutlinePass
	{
		PixelShader = compile ps_2_0 OutlineShader();
	}
}