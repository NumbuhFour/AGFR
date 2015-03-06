Shader "Custom/StepperLight"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_IgnoreColor ("Ignore Color", Color) = (0.125,0.125,0.125,1)
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
			"LightMode" = "ForwardAdd"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Stepper vertex:vert nofog keepalpha
		//#pragma surface surf Lambert vertex:vert nofog keepalpha
		#pragma multi_compile _ PIXELSNAP_ON
		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _IgnoreColor;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color;
		}
		
		#pragma lighting Stepper
		inline half4 LightingStepper (SurfaceOutput s, half3 lightDir, half atten)
		{
			if(s.Normal.z <= 0){
				//atten = atten - fmod(atten, 0.05);
			}
			float uvX = s.Specular; //Cheaty communication
			float uvY = s.Gloss;
			
			float tileSize = 1.0/25.0;
			float distFromLight = atten; //Should be linear with cookie!
			float tileX = (uvX - fmod(uvX, tileSize));
			float tileY = (uvY - fmod(uvY, tileSize));
			//if(tileX/tileSize >= 16.0 && tileX/tileSize < 16.9) atten = 0;
			//else {
				tileX = tileX * 450+45;
				tileY = tileY * 450+125;
				
				float3 lightPos = lightDir*distFromLight + half3(uvX, uvY, 0);
				float tileAtten = sqrt((lightPos.x - tileX)*(lightPos.x - tileX)
									+ (lightPos.y - tileY)*(lightPos.y - tileY)
									+ (lightPos.z*lightPos.z))/130.0;
									
				tileAtten = max(0, min(2, tileAtten));
				
				atten = (1.0-tileAtten)/100; 
			//}
			
			
			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL;
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 1);
			c.a = s.Alpha;
			if(s.Albedo.x == 1 && s.Albedo.y == 1 && s.Albedo.z == 1)  //REQUIRED
				c.a = 0;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			if(c.x == _IgnoreColor.x && c.y == _IgnoreColor.y && c.z == _IgnoreColor.z || c.a == 0){
				c.a = 0;
			}else {
				c = fixed4(1,1,1,1);
			}
			
			o.Specular = IN.uv_MainTex.x;
			o.Gloss = IN.uv_MainTex.y;
			
			//if(IN.uv_MainTex.x >= 0.5) c.a = 0;
			
			o.Albedo = c.rgb * c.a;
			o.Alpha = c.a;
		}
		ENDCG
	}

Fallback "Transparent/VertexLit"
}
