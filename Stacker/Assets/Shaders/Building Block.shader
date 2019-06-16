Shader "Custom/Building Block"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_OutlineScale ("Outline Scale", Range(0, 2)) = 1.03
		_OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Geometry+100" "RenderType"="Opaque" }
        LOD 200

		Cull Back
		ZWrite On
		ZTest Less

		Pass // Render outline pass
		{
			Name "Outline"
			Tags { "Queue" = "Transparent" }
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _OutlineScale;
			fixed4 _OutlineColor;

			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 pos : POSITION;
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				v.vertex.xyz *= _OutlineScale;
				o.pos = UnityObjectToClipPos(v.vertex.xyz);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}

		// Normal render pass

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

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
