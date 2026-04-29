Shader "Custom/FullScreenDarkness_Fixed"
{
    Properties
    {
        _Color ("Darkness Color", Color) = (0,0,0,1) // Alpha 1 yaparsan zifiri karanlık olur
    }
    SubShader
    {
        // Çizim sırasını en sona (Overlay) alıyoruz ki her şeyin üstüne binsin
        Tags { "Queue"="Overlay+10" "RenderType"="Transparent" "IgnoreProjector"="True" }
        
        // Standart şeffaflık ayarı
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Stencil
            {
                Ref 1
                Comp NotEqual // Stencil 1 (FOV) OLMAYAN yerlere siyah bas
                Pass Keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Burada doğrudan Properties'den gelen rengi döndürüyoruz
                return _Color; 
            }
            ENDCG
        }
    }
}