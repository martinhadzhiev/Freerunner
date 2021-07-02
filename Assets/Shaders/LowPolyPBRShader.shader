Shader "LowPolyShaders/LowPolyPBRShader" {
	Properties {
		_MainTex ("Color Scheme", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert

		#pragma target 3.0

		uniform float2 _BendAmount;
        uniform float3 _BendOrigin;
        uniform float _BendFalloff;

		struct Input {
			float3 color : COLOR;
		};		

		sampler2D _MainTex;
		fixed4 _Color;

		float4 Curve(float4 v)
        {
              _BendAmount *= .0001;

              float4 world = mul(unity_ObjectToWorld, v);

              float dist = length(world.xz-_BendOrigin.xz);

              dist = max(0, dist-_BendFalloff);
              dist = dist*dist;

              world.xy += dist*_BendAmount;
              return mul(unity_WorldToObject, world);
        }

		void vert (inout appdata_full v) {
			v.vertex = Curve(v.vertex);
			v.color = tex2Dlod(_MainTex, v.texcoord) * _Color;
        }

		half _Glossiness;
		half _Metallic;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = IN.color;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
