Shader "Custom/CamShader"
{
    Properties
    {
        _ScreenTint ("_ScreenTint", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "_ScreenTint" {}
        _UnscaledTime ("UnscaledTime", Float) = 0
        _IsForcingBack ("IsForcingBack", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float UnscaledTime;
            float IsForcingBack;

            sampler2D _MainTex;
            float4 _ScreenTint;
             fixed4 col;

            fixed4 frag (v2f i) : SV_Target
            {
              //fixed4 col = tex2D(_MainTex, i.uv + float2(0, sin( i.vertex.x/50 + _Time[1] * 3 ) /50 ) );
              
            
              if(IsForcingBack == 0)
              {
                   col = tex2D(_MainTex, i.uv);
              }
              else if (IsForcingBack == 1)
              {
                    col = tex2D(_MainTex, i.uv + float2(tan( i.vertex.y/90 + UnscaledTime * 0.5f)/ 1500, 0));
              }
              else if (IsForcingBack == 2)
              {
                  col = tex2D(_MainTex, i.uv + float2(cos(i.vertex.x + UnscaledTime * 10) / 500, sin( i.vertex.y + UnscaledTime * 10 ) / 500));
              }

              // _ScreenTint = float4(sin(_ScreenTint.x + _Time[1] ), sin(_ScreenTint.y + _Time[1] * 2), sin(_ScreenTint.z + _Time[1] * 3), 0);
              // _ScreenTint = float4(_ScreenTint.x, _ScreenTint.y, _ScreenTint.z, 0);

              //Glass looking weird pixel effect
              // col = tex2D(_MainTex, i.uv + float2(cos(i.vertex.x + UnscaledTime * 10) / 500, sin( i.vertex.y + UnscaledTime * 10 ) / 500));

              return col * _ScreenTint;
            }
            ENDCG
        }
    }
}
