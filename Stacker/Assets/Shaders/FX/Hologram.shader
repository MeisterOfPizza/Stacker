Shader "Custom/FX/Hologram"
{
    Properties
    {
        _HologramTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
		[HDR] _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		[PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 4)) = 2.7
		_HologramSpeed("Speed", float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Unlit alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _HologramTex;
		float4 _HologramTex_ST;
		fixed4 _Color;
		float3 _FresnelColor;
		float _FresnelExponent;
		float _HologramSpeed;

        struct Input
        {
			float3 worldNormal;
			float3 viewDir;
			float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
			// Create the 2D (orthographic) screen pos uv coordinates.
			float2 screenPos2D = IN.screenPos.xy / IN.screenPos.w;

			// Create the final uv coordinates.
			float speed = _Time[0] * _HologramSpeed;
			float2 uv = TRANSFORM_TEX(screenPos2D * _HologramTex_ST.xy + float2(speed, speed), _HologramTex);

            // Albedo comes from a texture tinted by color.
            fixed4 col = tex2D(_HologramTex, uv) * _Color;
            o.Albedo = col.rgb;
			o.Alpha = (col.r + col.g + col.b + col.a) / 4;

			// Taken from https://www.ronja-tutorials.com/2018/05/26/fresnel.html.
			// Get the dot product between the normal and the view direction.
			float fresnel = dot(IN.worldNormal, IN.viewDir);
			// Invert the fresnel so the big values are on the outside.
			fresnel = saturate(1 - fresnel);
			// Raise the fresnel value to the exponents power to be able to adjust it.
			fresnel = pow(fresnel, _FresnelExponent);
			// Combine the fresnel value with a color.
			float3 fresnelColor = fresnel * _FresnelColor;
			// Apply the fresnel value to the emission.
			o.Emission = fresnelColor;
        }

		// Remove lighting.
		half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
		{
			return half4(s.Albedo, s.Alpha);
		}
        ENDCG
    }
    FallBack "Diffuse"
}
