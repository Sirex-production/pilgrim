Shader "Custom/2DVignette"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _Radius("Radius", Range(0, 1)) = 0
        _Strength("Strength", Float) = 5
        _CenterX("CenterX", Range(0, 1)) = .5
        _CenterY("CenterY", Range(0, 1)) = .5
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Libraries/InterpolationF.cginc"
            
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

            fixed4 _Color;
            float _Radius;
            float _Strength;
            half _CenterX;
            half _CenterY;
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 targetColor = _Color;
                const half2 centerCoordinates = half2(_CenterX, _CenterY);
                const half2 uv = i.uv.xy;
                half distanceToTheCenter = distance(uv, centerCoordinates);
                fixed targetAlpha = Lerp(0, _Radius * _Strength, distanceToTheCenter); 
                
                targetColor.a = targetAlpha;
                
                return targetColor;
            }
            ENDCG
        }
    }
}
