
Shader "DC_Shaders/DC_Standard_VolumeClip"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetallicGloss ("MetallicGloss", 2D) = "black" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Cull Back
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MetallicGloss;
		sampler2D _BumpMap;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
			float3 clipPos;
		};

		fixed4 _Color;
		float4x4 _TransformMatrix;

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.clipPos = mul(_TransformMatrix, mul(unity_ObjectToWorld, v.vertex));
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 mg = tex2D(_MetallicGloss, IN.uv_MainTex);

			if (IN.clipPos.x < -0.5 || IN.clipPos.x > 0.5 ||
				IN.clipPos.y < -0.5 || IN.clipPos.y > 0.5 ||
				IN.clipPos.z < -0.5 || IN.clipPos.z > 0.5)
				discard;

			o.Albedo = c.rgb;
			o.Metallic = mg.r;
			o.Smoothness = mg.a;
			o.Alpha = c.a;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
