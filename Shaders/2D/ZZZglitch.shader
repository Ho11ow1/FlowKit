Shader "FlowKit/2D/ZZZglitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Name "Main"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // ------------------------------ DATA ------------------------------

            // Properties
            sampler2D _MainTex;
            fixed4 _Color;

            // Input from mesh
            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            // Vertex to Fragment structure
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // ------------------------------ SHADER FUNCTIONS ------------------------------

            // Vertex shader
            // Move vertex position to screen space and pass UV coordinates
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);      // Transform to screen space
                o.uv = v.uv;                                    // Pass UVs to fragment shader

                return o;
            }

            // Fragment shader
            // Sample the texture and apply color
            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv) * _Color;          // Sample texture and apply color
            }
            ENDCG
        }
    }
}
