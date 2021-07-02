Shader "Custom/FenceCurved"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex("Albedo and alpha (RGBA)", 2D) = "white" {}
        _Cutoff("Base Alpha cutoff", Range(0,.9)) = .5
    }

    SubShader
    {
        Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert alphatest:_Cutoff addshadow

        uniform float2 _BendAmount;
        uniform float3 _BendOrigin;
        uniform float _BendFalloff;

        sampler2D _MainTex;

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

        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
 
      Fallback "Mobile/Diffuse"
}