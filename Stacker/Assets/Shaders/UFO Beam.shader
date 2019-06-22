Shader "Custom/UFO Beam"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[HDR] _EmissionColor ("Emission color", Color) = (1, 1, 1, 1)

		[Space(10)]
		_VerticalCutout ("Vertical cutout", Range(0, 1)) = 0

		[Header(Edge)]
		[Space(10)]
        _EdgeNoiseTex ("Noise (R)", 2D) = "white" {}
		[PowerSlider(2.0)] _EdgeThickness ("Edge thickness", Range(0, 1)) = 0.1
		_EdgeSpinSpeed ("Edge spin speed", float) = 3.3
		_EdgeClipThreshold ("Edge clip threshold", Range(0, 1)) = 0.15
    }
    SubShader
    {
        Tags { "Queue"="Transparent+100" "RenderType"="Transparent+100" "IgnoreProjector"="True" "ForceNoShadowCasting"="True" }
        LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

        CGPROGRAM
        // Physically based Standard lighting model
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _EdgeNoiseTex;
		fixed4 _Color;
		half4 _EmissionColor;
		half _VerticalCutout;
		half _EdgeThickness;
		half _EdgeSpinSpeed;
		half _EdgeClipThreshold;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_EdgeNoiseTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			clip(IN.uv_MainTex.y - _VerticalCutout);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			fixed noise = tex2D(_EdgeNoiseTex, IN.uv_EdgeNoiseTex + float2(_EdgeSpinSpeed * _Time[0], _EdgeSpinSpeed * _Time[0])).r;
			fixed edge = IN.uv_MainTex.y - _EdgeThickness < _VerticalCutout ? 1 : 0;
			clip(noise - _EdgeClipThreshold * edge);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
			o.Emission = _EmissionColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
