// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32737,y:32716,varname:node_3138,prsc:2|emission-4641-OUT,alpha-132-OUT,clip-7429-OUT;n:type:ShaderForge.SFN_Fresnel,id:132,x:32202,y:32994,varname:node_132,prsc:2|EXP-5972-OUT;n:type:ShaderForge.SFN_Color,id:1472,x:32202,y:32840,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1472,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:5972,x:31845,y:33052,ptovrint:False,ptlb:Softness,ptin:_Softness,varname:node_5972,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.5,max:5;n:type:ShaderForge.SFN_Multiply,id:4641,x:32433,y:32828,varname:node_4641,prsc:2|A-1472-RGB,B-132-OUT;n:type:ShaderForge.SFN_Slider,id:5597,x:31660,y:33209,ptovrint:False,ptlb:Dissolve Amount,ptin:_DissolveAmount,varname:node_5597,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:1612,x:32205,y:33192,varname:node_1612,prsc:2,frmn:0,frmx:1,tomn:-0.6,tomx:0.6|IN-2183-OUT;n:type:ShaderForge.SFN_Tex2d,id:4661,x:32205,y:33405,ptovrint:False,ptlb:Dissolve Pattern,ptin:_DissolvePattern,varname:node_4661,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:7429,x:32416,y:33210,varname:node_7429,prsc:2|A-1612-OUT,B-4661-B;n:type:ShaderForge.SFN_OneMinus,id:2183,x:32009,y:33192,varname:node_2183,prsc:2|IN-5597-OUT;proporder:1472-5972-5597-4661;pass:END;sub:END;*/

Shader "Shader Forge/Outline" {
    Properties {
        _Color ("Color", Color) = (1,0,0,1)
        _Softness ("Softness", Range(0, 5)) = 1.5
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolvePattern ("Dissolve Pattern", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Softness;
            uniform float _DissolveAmount;
            uniform sampler2D _DissolvePattern; uniform float4 _DissolvePattern_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _DissolvePattern_var = tex2D(_DissolvePattern,TRANSFORM_TEX(i.uv0, _DissolvePattern));
                clip((((1.0 - _DissolveAmount)*1.2+-0.6)+_DissolvePattern_var.b) - 0.5);
////// Lighting:
////// Emissive:
                float node_132 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_Softness);
                float3 emissive = (_Color.rgb*node_132);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_132);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _DissolveAmount;
            uniform sampler2D _DissolvePattern; uniform float4 _DissolvePattern_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _DissolvePattern_var = tex2D(_DissolvePattern,TRANSFORM_TEX(i.uv0, _DissolvePattern));
                clip((((1.0 - _DissolveAmount)*1.2+-0.6)+_DissolvePattern_var.b) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
