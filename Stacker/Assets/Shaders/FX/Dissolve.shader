Shader "Custom/FX/Dissolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		
		[Header(Dissolve Parameters)]
		[Space(10)]
		_NoiseTex ("Noise (R)", 2D) = "white" {}
		[HDR] _OutlineColor ("Dissolve outline color", Color) = (1, 1, 1, 1)
		[PowerSlider(3.0)] _OutlineThickness ("Dissolve outline thickness", Range(0, 1)) = 0.01
		
		[Space(10)]
		_DissolveSpeed ("Dissolve speed", float) = 1
		
		[Space(10)]
		[Toggle] _CanDissolve ("Can dissolve?", float) = 1
		[Toggle] _TimeDissolve ("Use Time as dissolve amount?", float) = 0
		_DissolveAmount ("Dissolve amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Name "Draw"
        LOD 200
		Cull Off
		
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		
        sampler2D _MainTex;
		sampler2D _NoiseTex;
		fixed4 _Color;
		fixed4 _OutlineColor;
		half _OutlineThickness;
		half _DissolveSpeed;
		half _CanDissolve;
		half _TimeDissolve;
		half _DissolveAmount;
		
        struct Input
        {
            float2 uv_MainTex;
			float2 uv_NoiseTex;
        };
		
		float map(float value, float min1, float max1, float min2, float max2)
		{
			return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
		}
		
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r;
			half amount = _TimeDissolve == 1 ? map(_SinTime[3], -1, 1, 0, 1) : _DissolveAmount;
			half t = pow(amount, _DissolveSpeed);
			clip(_CanDissolve == 1 ? noise - t : 1); // Discard this pixel if (noise - t) is less than 0.
			
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half outline = step(noise - t, _OutlineThickness);
			
            o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = outline * _OutlineColor * _CanDissolve;
        }
        ENDCG
    }
	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "Queue"="Geometry" "LightMode"="ShadowCaster" }
		Name "Shadow Caster"
        ZWrite On
        Cull Off
		
        CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		
		sampler2D _NoiseTex;
		half _OutlineThickness;
		half _DissolveSpeed;
		half _CanDissolve;
		half _TimeDissolve;
		half _DissolveAmount;
		
        struct Input
        {
			float2 uv_NoiseTex;
        };
		
		float map(float value, float min1, float max1, float min2, float max2)
		{
			return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
		}
		
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r;
			half amount = _TimeDissolve == 1 ? map(_SinTime[3], -1, 1, 0, 1) : _DissolveAmount;
			half t = pow(amount, _DissolveSpeed);
			clip(_CanDissolve == 1 ? noise - t : 1); // Discard this pixel if (noise - t) is less than 0.
        }
        ENDCG
    }
    FallBack "Diffuse"
}
