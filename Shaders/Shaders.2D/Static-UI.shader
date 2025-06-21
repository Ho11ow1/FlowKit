Shader "FlowKit/2D/Static-UI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
        
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // ------------------------------ DATA STRUCTURES ------------------------------

            struct appData
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            // ------------------------------ SHADER FUNCTIONS ------------------------------

            v2f vert(appData v)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);     // Transform to screen space
                output.uv = v.uv;                                   // Pass UVs to fragment shader

                return output;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv) * _Color;              // Sample texture and apply color
            }

            ENDCG
        }
    }
}
