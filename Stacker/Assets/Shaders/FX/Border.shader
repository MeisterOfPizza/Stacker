Shader "Custom/FX/Border"
{
    Properties
    {
        _BorderTex ("Albedo (RGB)", 2D) = "white" {}
		_AlphaTex ("Alpha (A)", 2D) = "white" {}
        [HDR] _BorderColor ("Border Color", Color) = (1,1,1,1)
		[HDR] _BorderGlow ("Glow Color", Color) = (1, 1, 1, 1)
		_BorderSpeed ("Speed", float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "ForceNoShadowCasting"="True" }
        LOD 100
		Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Unlit alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _BorderTex;
		sampler2D _AlphaTex;
		fixed4 _BorderColor;
		fixed4 _BorderGlow;
		float _BorderSpeed;

        struct Input
        {
			float2 uv_BorderTex;
			float2 uv_AlphaTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
			float2 offset = float2(_BorderSpeed * _Time[0], _BorderSpeed * _Time[0]);

            // Albedo comes from a texture tinted by color
            fixed4 col = tex2D (_BorderTex, IN.uv_BorderTex + offset) * _BorderColor;
            o.Albedo = col.rgb;
            o.Alpha = tex2D(_AlphaTex, IN.uv_AlphaTex).a;
			o.Emission = _BorderGlow;
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
