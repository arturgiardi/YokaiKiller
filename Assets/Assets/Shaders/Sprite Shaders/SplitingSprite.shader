// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-8890-OUT,custl-5085-OUT,clip-2297-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:32734,y:33086,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:32734,y:32952,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:31798,y:32686,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:9684,x:31798,y:32814,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:7782,x:32010,y:32729,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-9684-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:31334,y:32421,ptovrint:False,ptlb:Main Tex,ptin:_MainTex,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1941,x:32465,y:32693,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-544-OUT,B-7782-OUT;n:type:ShaderForge.SFN_Add,id:2159,x:32734,y:32812,cmnt:Combine,varname:node_2159,prsc:2|A-1941-OUT,B-9915-OUT;n:type:ShaderForge.SFN_Multiply,id:5085,x:32979,y:32952,cmnt:Attenuate and Color,varname:node_5085,prsc:2|A-2159-OUT,B-3406-RGB,C-8068-OUT;n:type:ShaderForge.SFN_AmbientLight,id:7528,x:32684,y:32634,varname:node_7528,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2460,x:32880,y:32598,cmnt:Ambient Light,varname:node_2460,prsc:2|A-544-OUT,B-7528-RGB;n:type:ShaderForge.SFN_Multiply,id:544,x:32268,y:32448,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-2380-RGB;n:type:ShaderForge.SFN_Dot,id:3738,x:32010,y:32938,cmnt:Lambert,varname:node_3738,prsc:2,dt:1|A-6869-OUT,B-9684-OUT;n:type:ShaderForge.SFN_OneMinus,id:8214,x:32206,y:32938,varname:node_8214,prsc:2|IN-3738-OUT;n:type:ShaderForge.SFN_VertexColor,id:2380,x:32070,y:32513,varname:node_2380,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:357,x:31220,y:33237,ptovrint:False,ptlb:Split Map,ptin:_SplitMap,varname:node_357,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1da3afee7644c1047944cd990ab95700,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2297,x:32213,y:33217,varname:node_2297,prsc:2|A-851-A,B-2684-OUT,C-8676-OUT;n:type:ShaderForge.SFN_Multiply,id:9915,x:32465,y:32876,cmnt:Diffuse Contribution,varname:node_9915,prsc:2|A-544-OUT,B-8214-OUT;n:type:ShaderForge.SFN_OneMinus,id:4559,x:31372,y:33441,varname:node_4559,prsc:2|IN-357-R;n:type:ShaderForge.SFN_SwitchProperty,id:2684,x:31532,y:33277,ptovrint:False,ptlb:Flip Cut,ptin:_FlipCut,varname:node_2684,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-357-R,B-4559-OUT;n:type:ShaderForge.SFN_Slider,id:8105,x:32420,y:32080,ptovrint:False,ptlb:Emissiviness,ptin:_Emissiviness,varname:node_8105,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:100;n:type:ShaderForge.SFN_Color,id:7464,x:32458,y:32217,ptovrint:False,ptlb:node_7464,ptin:_node_7464,varname:node_7464,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:3855,x:32681,y:32201,varname:node_3855,prsc:2|A-8105-OUT,B-7464-RGB;n:type:ShaderForge.SFN_Add,id:8890,x:32898,y:32335,varname:node_8890,prsc:2|A-3855-OUT,B-2460-OUT,C-699-OUT,D-8760-OUT;n:type:ShaderForge.SFN_Color,id:2450,x:32500,y:31685,ptovrint:False,ptlb:CutColor,ptin:_CutColor,varname:node_2450,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2794118,c2:0.9105479,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:9147,x:32482,y:31885,ptovrint:False,ptlb:CutMap,ptin:_CutMap,varname:node_9147,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:23cc967d6bc02ea45be1e34082511e94,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:9398,x:32719,y:32130,ptovrint:False,ptlb:CutEmissiviness,ptin:_CutEmissiviness,varname:_Emissiviness_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:200;n:type:ShaderForge.SFN_Multiply,id:699,x:32999,y:31986,varname:node_699,prsc:2|A-2450-RGB,B-9147-RGB,C-9398-OUT;n:type:ShaderForge.SFN_Tex2d,id:1296,x:31794,y:33505,ptovrint:False,ptlb:DissolveMap,ptin:_DissolveMap,varname:node_1296,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:5806,x:32271,y:33628,varname:node_5806,prsc:2|A-1296-RGB,B-1920-OUT;n:type:ShaderForge.SFN_Slider,id:1920,x:31669,y:33723,ptovrint:False,ptlb:Dissolve,ptin:_Dissolve,varname:node_1920,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Lerp,id:1738,x:32271,y:33483,varname:node_1738,prsc:2|A-3156-OUT,B-1296-RGB,T-1920-OUT;n:type:ShaderForge.SFN_Vector3,id:3156,x:31807,y:33373,varname:node_3156,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Lerp,id:8676,x:32491,y:33483,varname:node_8676,prsc:2|A-1738-OUT,B-5806-OUT,T-1920-OUT;n:type:ShaderForge.SFN_OneMinus,id:939,x:32532,y:33630,varname:node_939,prsc:2|IN-5806-OUT;n:type:ShaderForge.SFN_Relay,id:8760,x:32869,y:33189,varname:node_8760,prsc:2|IN-3084-OUT;n:type:ShaderForge.SFN_Clamp01,id:3084,x:32999,y:33481,varname:node_3084,prsc:2|IN-9616-OUT;n:type:ShaderForge.SFN_Relay,id:4131,x:32719,y:33348,varname:node_4131,prsc:2|IN-2450-RGB;n:type:ShaderForge.SFN_Multiply,id:9616,x:32789,y:33454,varname:node_9616,prsc:2|A-4131-OUT,B-939-OUT,C-5626-OUT;n:type:ShaderForge.SFN_Slider,id:5626,x:32719,y:33758,ptovrint:False,ptlb:DissolveEmission,ptin:_DissolveEmission,varname:node_5626,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;proporder:851-357-2684-8105-7464-2450-9147-9398-1296-1920-5626;pass:END;sub:END;*/

Shader "Shader Forge/SplitingSprite" {
    Properties {
        _MainTex ("Main Tex", 2D) = "white" {}
        _SplitMap ("Split Map", 2D) = "white" {}
        [MaterialToggle] _FlipCut ("Flip Cut", Float ) = 0
        _Emissiviness ("Emissiviness", Range(0, 100)) = 0
        _node_7464 ("node_7464", Color) = (1,0,0,1)
        _CutColor ("CutColor", Color) = (0.2794118,0.9105479,1,1)
        _CutMap ("CutMap", 2D) = "white" {}
        _CutEmissiviness ("CutEmissiviness", Range(0, 200)) = 2
        _DissolveMap ("DissolveMap", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0, 1)) = 1
        _DissolveEmission ("DissolveEmission", Range(0, 10)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _SplitMap; uniform float4 _SplitMap_ST;
            uniform fixed _FlipCut;
            uniform float _Emissiviness;
            uniform float4 _node_7464;
            uniform float4 _CutColor;
            uniform sampler2D _CutMap; uniform float4 _CutMap_ST;
            uniform float _CutEmissiviness;
            uniform sampler2D _DissolveMap; uniform float4 _DissolveMap_ST;
            uniform float _Dissolve;
            uniform float _DissolveEmission;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _SplitMap_var = tex2D(_SplitMap,TRANSFORM_TEX(i.uv0, _SplitMap));
                float4 _DissolveMap_var = tex2D(_DissolveMap,TRANSFORM_TEX(i.uv0, _DissolveMap));
                float3 node_5806 = (_DissolveMap_var.rgb+_Dissolve);
                clip((_MainTex_var.a*lerp( _SplitMap_var.r, (1.0 - _SplitMap_var.r), _FlipCut )*lerp(lerp(float3(0,0,0),_DissolveMap_var.rgb,_Dissolve),node_5806,_Dissolve)) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float3 node_544 = (_MainTex_var.rgb*i.vertexColor.rgb); // Diffuse Color
                float4 _CutMap_var = tex2D(_CutMap,TRANSFORM_TEX(i.uv0, _CutMap));
                float3 emissive = ((_Emissiviness*_node_7464.rgb)+(node_544*UNITY_LIGHTMODEL_AMBIENT.rgb)+(_CutColor.rgb*_CutMap_var.rgb*_CutEmissiviness)+saturate((_CutColor.rgb*(1.0 - node_5806)*_DissolveEmission)));
                float3 finalColor = emissive + (((node_544*max(0,dot(lightDirection,normalDirection)))+(node_544*(1.0 - max(0,dot(lightDirection,normalDirection)))))*_LightColor0.rgb*attenuation);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _SplitMap; uniform float4 _SplitMap_ST;
            uniform fixed _FlipCut;
            uniform float _Emissiviness;
            uniform float4 _node_7464;
            uniform float4 _CutColor;
            uniform sampler2D _CutMap; uniform float4 _CutMap_ST;
            uniform float _CutEmissiviness;
            uniform sampler2D _DissolveMap; uniform float4 _DissolveMap_ST;
            uniform float _Dissolve;
            uniform float _DissolveEmission;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _SplitMap_var = tex2D(_SplitMap,TRANSFORM_TEX(i.uv0, _SplitMap));
                float4 _DissolveMap_var = tex2D(_DissolveMap,TRANSFORM_TEX(i.uv0, _DissolveMap));
                float3 node_5806 = (_DissolveMap_var.rgb+_Dissolve);
                clip((_MainTex_var.a*lerp( _SplitMap_var.r, (1.0 - _SplitMap_var.r), _FlipCut )*lerp(lerp(float3(0,0,0),_DissolveMap_var.rgb,_Dissolve),node_5806,_Dissolve)) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 node_544 = (_MainTex_var.rgb*i.vertexColor.rgb); // Diffuse Color
                float3 finalColor = (((node_544*max(0,dot(lightDirection,normalDirection)))+(node_544*(1.0 - max(0,dot(lightDirection,normalDirection)))))*_LightColor0.rgb*attenuation);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
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
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _SplitMap; uniform float4 _SplitMap_ST;
            uniform fixed _FlipCut;
            uniform sampler2D _DissolveMap; uniform float4 _DissolveMap_ST;
            uniform float _Dissolve;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _SplitMap_var = tex2D(_SplitMap,TRANSFORM_TEX(i.uv0, _SplitMap));
                float4 _DissolveMap_var = tex2D(_DissolveMap,TRANSFORM_TEX(i.uv0, _DissolveMap));
                float3 node_5806 = (_DissolveMap_var.rgb+_Dissolve);
                clip((_MainTex_var.a*lerp( _SplitMap_var.r, (1.0 - _SplitMap_var.r), _FlipCut )*lerp(lerp(float3(0,0,0),_DissolveMap_var.rgb,_Dissolve),node_5806,_Dissolve)) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
