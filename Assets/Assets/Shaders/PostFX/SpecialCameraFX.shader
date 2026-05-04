// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:6,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32973,y:33234,varname:node_2865,prsc:2|emission-1550-OUT;n:type:ShaderForge.SFN_TexCoord,id:4219,x:31845,y:33171,cmnt:Default coordinates,varname:node_4219,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Relay,id:8397,x:32101,y:33171,cmnt:Refract here,varname:node_8397,prsc:2|IN-4219-UVOUT;n:type:ShaderForge.SFN_Tex2dAsset,id:4430,x:31811,y:33326,ptovrint:False,ptlb:MainTex,ptin:_MainTex,cmnt:MainTex contains the color of the scene,varname:node_9933,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7542,x:32314,y:33237,varname:node_1672,prsc:2,ntxv:0,isnm:False|UVIN-8397-OUT,TEX-4430-TEX;n:type:ShaderForge.SFN_Slider,id:2009,x:31826,y:33680,ptovrint:False,ptlb:Depth,ptin:_Depth,varname:node_2009,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:999;n:type:ShaderForge.SFN_DepthBlend,id:4306,x:32243,y:33578,varname:node_4306,prsc:2|DIST-2009-OUT;n:type:ShaderForge.SFN_OneMinus,id:8339,x:32545,y:33411,varname:node_8339,prsc:2|IN-4306-OUT;n:type:ShaderForge.SFN_Tex2d,id:2381,x:32162,y:33384,ptovrint:False,ptlb:Background,ptin:_Background,varname:node_2381,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1019-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4470,x:32345,y:33441,varname:node_4470,prsc:2|A-2381-RGB,B-4306-OUT;n:type:ShaderForge.SFN_Add,id:1550,x:32742,y:33374,varname:node_1550,prsc:2|A-8339-OUT,B-4470-OUT;n:type:ShaderForge.SFN_ScreenPos,id:7189,x:31370,y:33171,varname:node_7189,prsc:2,sctp:1;n:type:ShaderForge.SFN_Panner,id:1019,x:31636,y:33504,varname:node_1019,prsc:2,spu:0,spv:2|UVIN-7189-UVOUT,DIST-979-T;n:type:ShaderForge.SFN_Time,id:979,x:31383,y:33582,varname:node_979,prsc:2;proporder:4430-2009-2381;pass:END;sub:END;*/

Shader "Shader Forge/SpecialCameraFX" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Depth ("Depth", Range(0, 999)) = 0
        _Background ("Background", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest Always
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float _Depth;
            uniform sampler2D _Background; uniform float4 _Background_ST;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
////// Lighting:
////// Emissive:
                float node_4306 = saturate((sceneZ-partZ)/_Depth);
                float node_8339 = (1.0 - node_4306);
                float4 node_979 = _Time;
                float2 node_1019 = (float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg+node_979.g*float2(0,2));
                float4 _Background_var = tex2D(_Background,TRANSFORM_TEX(node_1019, _Background));
                float3 emissive = (node_8339+(_Background_var.rgb*node_4306));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
