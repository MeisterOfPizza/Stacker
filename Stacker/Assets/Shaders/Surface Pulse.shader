Shader "Custom/Surface Pulse"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

		[Space(10)]
		[Header(Pulse Parameters)]
		[HDR] _EmissionColor ("Pulse Color", Color) = (1, 1, 1, 1)
		_PulseSpeed ("Pulse Speed", Range(1, 100)) = 25
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		fixed4 _Color;
		fixed4 _EmissionColor;
		float _PulseSpeed;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float t = sin(_Time[0] * _PulseSpeed);
			t = t > 0 ? t : 0;
			o.Albedo = _Color.rgb * (1 - t);
			o.Emission = (o.Albedo + _EmissionColor) * t;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
