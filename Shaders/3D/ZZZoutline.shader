Shader "FlowKit/3D/ZZZoutline"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        
        // Use Lambert lighting (Standard)
        #pragma surface surf Lambert

        // ------------------------------ DATA STRUCTURES ------------------------------

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;

        // ------------------------------ SHADER FUNCTIONS ------------------------------

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex); // Sample the texture
            o.Albedo = c.rgb * _Color.rgb;            // Apply color to the texture
            o.Alpha = c.a;                            // Apply alpha
        }

        ENDCG
    }

    FallBack "Diffuse"
}
