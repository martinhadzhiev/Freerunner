Shader "Custom/Curved"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert addshadow

        uniform float2 _BendAmount;
        uniform float3 _BendOrigin;
        uniform float _BendFalloff;

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
              float2 uv_MainTex;
        };

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

        void vert(inout appdata_full v)
        {
              v.vertex = Curve(v.vertex);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
              fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
              o.Albedo = c.rgb;
              o.Alpha = c.a;
        }

        ENDCG
    }
 
      Fallback "Mobile/Diffuse"
}