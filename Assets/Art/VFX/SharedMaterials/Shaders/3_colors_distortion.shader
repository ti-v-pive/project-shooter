Shader "PerfectHeroes/3_colors_distortion"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Power ("Power", Range (0,10)) = 1
        [Header(BASE COLOR)]
        _BaseColor (" ", Color) = (1,1,1,1)
        _BaseCutout ("Base Cutout", Range (0.001,1)) = 0.001
        _BaseCutoutSmooth ("Base Smooth", Range (0.001,1)) = 0.001

        [Header(MIDDLE COLOR)]
        [Toggle] _MiddleColorUse (" ", float) = 0
        _MiddleColor (" ", COlor) = (1,1,1,1)
        _MiddleCutout ("Middle Cutout", Range (0.001,2)) = 0.001
        _MiddleCutoutSmooth ("Middle Smooth", Range (0.001,1)) = 0.001

        [Header(TOP COLOR)]
        [Toggle] _TopColorUse (" ", float) = 0
        _TopColor (" ", Color) = (1,1,1,1)
        _TopCutout ("Top Cutout", Range (0.001,2)) = 0.001
        _TopCutoutSmooth ("Top Smooth", Range (0.001,1)) = 0.001

        [Header(UV TRANSFORMS)]
        [Space(10)]
        _TilingOffset ("Tiling Offset", Vector) = (1,1,0,0)
        _ScrollSkew ("Scroll Skew", Vector) = (0,0,0,0)
        _EdgeCut ("Edge Cut", Vector) = (0,0,0,0)
        _EdgeCutOffset ("Edge Cut Offset", Vector) = (0,0,0,0)

        [Header(DISTORTION)]
        [Toggle] _DistortionUse (" ", float) = 0
        [NoScaleOffset] _DistortTex ("Texture", 2D) = "white" {}
        _DistortTex_TilingScroll ("Tiling Scroll", Vector) = (1,1,0,0)
        _DistortTex_Power ("Distortion power", Vector) = (0,0,0,0)

        [Header(SHADER PROPERTIES)]
        [Space(10)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendFactor ("Src", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlendFactor ("Dst", Float) = 10
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend [_SrcBlendFactor] [_DstBlendFactor]
        Ztest [_ZTest]
        Cull [_CullMode]
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _MIDDLECOLORUSE_ON
            #pragma shader_feature _TOPCOLORUSE_ON
            #pragma shader_feature _DISTORTIONUSE_ON
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR0;
                float4 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1; //custom vertex streams
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 uv1 : TEXCOORD1; //edges cut
                fixed4 color : COLOR0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _BaseColor;
            float4 _TilingOffset;
            float4 _ScrollSkew;
            float4 _EdgeCut;
            float4 _EdgeCutOffset;
            float _Power;
            float _BaseCutout, _BaseCutoutSmooth;

            #if _MIDDLECOLORUSE_ON
            fixed4  _MiddleColor;
            float _MiddleCutout, _MiddleCutoutSmooth;
            #endif

            #if _TOPCOLORUSE_ON
            fixed4 _TopColor;
            float _TopCutout, _TopCutoutSmooth;
            #endif

            #if _DISTORTIONUSE_ON
            sampler2D _DistortTex;
            float4 _DistortTex_TilingScroll, _DistortTex_Power;
            #endif

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                //edges cut
                fixed4 diap = step(_EdgeCut, 0);
                _EdgeCut = diap + (1-diap) * _EdgeCut;
                o.uv1 = diap + (1- diap) * float4(v.uv.x + _EdgeCutOffset.x, 1-v.uv.x + _EdgeCutOffset.y, v.uv.y + _EdgeCutOffset.z, 1-v.uv.y + _EdgeCutOffset.w)/_EdgeCut;

                //                    skew
                v.uv.xy += v.uv.yx * _ScrollSkew.zw;

                //                                [    tiling   ]   [              scroll        ]    [    offset    ]
                o.uv.xy = (v.uv.xy + v.uv1.xy) * _TilingOffset.xy + frac(_Time.y * _ScrollSkew.xy) + _TilingOffset.zw; 
                
                #if _DISTORTIONUSE_ON
                o.uv.zw = v.uv.xy * _DistortTex_TilingScroll.xy + frac(_DistortTex_TilingScroll.zw * _Time.y);
                #else
                o.uv.zw = v.uv.xy;
                #endif


                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                i.uv1 = saturate(i.uv1);
                fixed mask = pow(i.uv1.x * i.uv1.y * i.uv1.z * i.uv1.w, 1); //edges cut
                _BaseCutout = lerp(1, _BaseCutout + (1-i.color.a), mask);

                #if _DISTORTIONUSE_ON
                fixed2 distortion = tex2D(_DistortTex, i.uv.zw).rg - 0.5;
                i.uv.xy += distortion * lerp(_DistortTex_Power.zw, _DistortTex_Power.xy, mask);
                #endif

                fixed cut_Tex = tex2D(_MainTex, i.uv);
                _BaseCutout = cut_Tex - lerp(-1,1+_BaseCutoutSmooth, _BaseCutout);
                fixed BaseMask = 1-saturate(_BaseCutout/-_BaseCutoutSmooth);    //base color
                
                
                #if _MIDDLECOLORUSE_ON // middle color
                fixed MiddleMask = saturate((_BaseCutout-_MiddleCutout)/_MiddleCutoutSmooth);
                #endif
                
                #if _TOPCOLORUSE_ON  //top color
                fixed TopMask = saturate((_BaseCutout - _TopCutout)/-_TopCutoutSmooth); 
                #endif

                col = _BaseColor * _Power;

                #if _MIDDLECOLORUSE_ON // middle color
                col = lerp(_BaseColor, _MiddleColor, MiddleMask) * _Power;
                #endif

                #if _TOPCOLORUSE_ON //top color
                col = lerp(_BaseColor, (lerp(_TopColor, _MiddleColor, TopMask)), MiddleMask) * _Power;
                #endif

                col.a *= saturate(BaseMask * mask);
                return col;
            }
            ENDCG
        }
    }
}