Shader "Custom/MirrorShader"
{
    Properties
    {
        _ScreenTint ("_ScreenTint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend One OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD1;
            };

            sampler2D _MainTex;  // Use the main texture in the Blit call
            float4 _ScreenTint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPosition = ComputeScreenPos(o.vertex);
                

                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture using the screen coordinates
              //  float2 screenUV = i.screenPosition.xy / i.screenPosition.w;
               // fixed4 col = tex2D(_MainTex, screenUV + float2(0, sin( i.vertex.x/50 + _Time[1] * 3 ) /50 ));

                // Sample the texture using _MainTex, which should be the source texture passed to Blit
                   fixed4 col = tex2D(_MainTex, i.uv);
                   col *= _ScreenTint;
                

               return col;
            }
            ENDCG
        }
    }
}
