Shader "Hidden/S_CRTEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SyncVer ("Vertical syncronisation fx", 2D) = "black" {}
        _CurvatureX("Curvature X", Range(0,5)) = 0.1
        _CurvatureY("Curvature Y", Range(0,5)) = 0.1
        _CurvatureScanX("Curvature scan X", Range(0,5)) = 0.1
        _CurvatureScanY("Curvature scan Y", Range(0,5)) = 0.1
        _OpacityX("Opacity X", Range(0,1)) = 0.1
        _OpacityY("Opacity Y", Range(0,1)) = 0.1
        roundness("Roundness", Range(0, 1)) = 1
        vignetteOpacity("Vignette opacity", Range(0, 1)) = 1
        _Brightness("brightness", Range(0, 10)) = 1
        _SyncVerSpeed("sync ver speed", Range(0, 1)) = .1
        _SyncVerStrength("sync ver strength", Range(0, .2)) = 0
        _ChromStr("chromatic aberration strength", Range(0, .2)) = 0
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

            float _CurvatureX;
            float _CurvatureY;
            float _CurvatureScanX;
            float _CurvatureScanY;
            float _OpacityX;
            float _OpacityY;
            float roundness;
            float vignetteOpacity;
            float _Brightness;
            float _SyncVerSpeed;
            float _SyncVerStrength;
            float _ChromStr;

            sampler2D _MainTex;
            sampler2D _SyncVer;

            float2 GetCurvedUvs(float2 uv, float2 curvature)
            {
                uv = uv * 2.0 - 1.0;
                float2 offset = abs(uv.yx) / curvature;
                uv = uv + uv * offset * offset;
                uv = uv * 0.5 + 0.5;

                return uv;
            }

            float4 scanLineIntensity(float uv, float resolution, float opacity)
            {
                float intensity = sin(uv * resolution * 3.14 * 2.0);
                intensity = ((0.5 * intensity) + 0.5) * 0.9 + 0.1;
                intensity = pow(intensity, opacity);
                return float4(float3(intensity, intensity, intensity), 1.0);
            }

            float4 vignette(float2 uv, float2 screenRes)
            {
                float intensity = uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
                intensity = clamp(pow((screenRes.x / 4.0) * intensity, vignetteOpacity), 0.0, 1.0);
                return float4(float3(intensity, intensity, intensity), 1.0);
            }
           
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float2 remappedUV = GetCurvedUvs(i.uv, float2(_CurvatureX, _CurvatureY));
                float2 remappedUVScan = GetCurvedUvs(i.uv, float2(_CurvatureScanX, _CurvatureScanY));
                float2 syncVerUv = remappedUV + float2(0, 1) * _Time.y * _SyncVerSpeed;
                float syncVer = tex2D(_SyncVer, syncVerUv);
                float2 mainTexUvs = remappedUV + float2(1, 0) * syncVer * _SyncVerStrength;
                fixed colR = tex2D(_MainTex, mainTexUvs + float2(1, 1) * _ChromStr).r;
                fixed colG = tex2D(_MainTex, mainTexUvs + float2(1, -1) * _ChromStr).g;
                fixed colB = tex2D(_MainTex, mainTexUvs + float2(-1, 1) * _ChromStr).b;
                fixed4 col = fixed4(colR, colG, colB, 1);


                col *= vignette(remappedUV.xy, _ScreenParams.xy);
                
                col *= scanLineIntensity(remappedUVScan.x, _ScreenParams.y, _OpacityX);
                col *= scanLineIntensity(remappedUVScan.y, _ScreenParams.x, _OpacityY);

                if (remappedUV.x < 0.0 || remappedUV.y < 0.0 || remappedUV.x > 1.0 || remappedUV.y > 1.0){
                    col = float4(0.0, 0.0, 0.0, 1.0);
                } else {
                    col = col;
                }
                col *= _Brightness;
                return col;
            }
            ENDCG
        }
    }
}
