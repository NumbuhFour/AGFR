Shader "Sprites/Swap"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Swap ("Swap", Color) = (1,1,1,1)
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert keepalpha
		#pragma multi_compile DUMMY PIXELSNAP_ON

		sampler2D _MainTex;
		fixed4 _Swap;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			v.normal = float3(0,0,-1);
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			if(c.rgb.r == 1 && c.rgb.g == 1 && c.rgb.b == 1 && c.a == 1){
				o.Albedo = _Swap * c.a;
			}else{
				o.Albedo = c.rgb * c.a * _Color;
			}
			o.Alpha = c.a;
		}
		ENDCG
	}

Fallback "Legacy Shaders/Transparent/VertexLit"
}
