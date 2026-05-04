// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33320,y:32715,varname:node_2865,prsc:2|diff-6343-OUT,spec-7845-OUT,gloss-7845-OUT,emission-3737-OUT,alpha-8032-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32509,y:32629,varname:node_6343,prsc:2|A-7736-RGB,B-6665-RGB;n:type:ShaderForge.SFN_Color,id:6665,x:32252,y:32689,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.0896554,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7736,x:31140,y:32630,ptovrint:True,ptlb:Base Color,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-9968-OUT;n:type:ShaderForge.SFN_Fresnel,id:6337,x:32049,y:33590,varname:node_6337,prsc:2|EXP-4262-OUT;n:type:ShaderForge.SFN_Slider,id:4262,x:31645,y:33611,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:node_4262,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.2,max:10;n:type:ShaderForge.SFN_Multiply,id:6774,x:32301,y:33684,varname:node_6774,prsc:2|A-6337-OUT,B-4468-OUT;n:type:ShaderForge.SFN_Vector1,id:4468,x:32049,y:33777,varname:node_4468,prsc:2,v1:3.5;n:type:ShaderForge.SFN_Slider,id:2706,x:32728,y:32972,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_2706,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3,max:10;n:type:ShaderForge.SFN_TexCoord,id:427,x:30710,y:32460,varname:node_427,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Clamp01,id:5046,x:32530,y:33684,varname:node_5046,prsc:2|IN-6774-OUT;n:type:ShaderForge.SFN_Tex2d,id:7971,x:32549,y:33230,ptovrint:False,ptlb:FadeMap,ptin:_FadeMap,varname:node_7971,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7bda051d28f15594b9cdd65ac20ef929,ntxv:0,isnm:False|UVIN-5174-OUT;n:type:ShaderForge.SFN_TexCoord,id:3299,x:31793,y:33275,varname:node_3299,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:8032,x:32967,y:33100,varname:node_8032,prsc:2|A-7971-A,B-5046-OUT;n:type:ShaderForge.SFN_Multiply,id:6109,x:32062,y:33291,varname:node_6109,prsc:2|A-4428-OUT,B-3299-U;n:type:ShaderForge.SFN_Slider,id:4428,x:31636,y:33450,ptovrint:False,ptlb:FadeScroll,ptin:_FadeScroll,varname:node_4428,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.1,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:7952,x:32154,y:33427,varname:node_7952,prsc:2|A-4428-OUT,B-3299-V;n:type:ShaderForge.SFN_Append,id:5174,x:32311,y:33212,varname:node_5174,prsc:2|A-6109-OUT,B-7952-OUT;n:type:ShaderForge.SFN_Tex2d,id:362,x:32252,y:32946,ptovrint:False,ptlb:EdgeShine,ptin:_EdgeShine,varname:node_362,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:41b4318e7e64c8642859b8f95fa8da1b,ntxv:0,isnm:False|UVIN-5089-OUT;n:type:ShaderForge.SFN_Add,id:6820,x:32787,y:32762,varname:node_6820,prsc:2|A-6343-OUT,B-1295-OUT;n:type:ShaderForge.SFN_Multiply,id:1295,x:32496,y:32892,varname:node_1295,prsc:2|A-6665-RGB,B-3921-OUT,C-362-RGB;n:type:ShaderForge.SFN_Vector1,id:3921,x:32252,y:32862,varname:node_3921,prsc:2,v1:20;n:type:ShaderForge.SFN_OneMinus,id:8094,x:31793,y:32946,varname:node_8094,prsc:2|IN-7736-R;n:type:ShaderForge.SFN_Vector1,id:7845,x:33320,y:32657,varname:node_7845,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:3737,x:33111,y:32816,varname:node_3737,prsc:2|A-6820-OUT,B-2706-OUT;n:type:ShaderForge.SFN_Time,id:7828,x:30293,y:32557,varname:node_7828,prsc:2;n:type:ShaderForge.SFN_Add,id:9968,x:30930,y:32630,varname:node_9968,prsc:2|A-427-UVOUT,B-982-OUT;n:type:ShaderForge.SFN_Multiply,id:4022,x:30533,y:32651,varname:node_4022,prsc:2|A-7828-T,B-142-OUT;n:type:ShaderForge.SFN_Slider,id:142,x:30136,y:32722,ptovrint:False,ptlb:FlowSpeed,ptin:_FlowSpeed,varname:node_142,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-3,cur:1,max:3;n:type:ShaderForge.SFN_Append,id:982,x:30710,y:32651,varname:node_982,prsc:2|A-4022-OUT,B-4022-OUT;n:type:ShaderForge.SFN_Multiply,id:5089,x:32045,y:32946,varname:node_5089,prsc:2|A-8094-OUT,B-5174-OUT;proporder:6665-7736-4262-2706-7971-4428-362-142;pass:END;sub:END;*/

Shader "Shader Forge/BozuShield" {
    Properties {
        _Color ("Color", Color) = (0,0.0896554,1,1)
        _MainTex ("Base Color", 2D) = "white" {}
        _Fresnel ("Fresnel", Range(0, 10)) = 2.2
        _Emission ("Emission", Range(0, 10)) = 3
        _FadeMap ("FadeMap", 2D) = "white" {}
        _FadeScroll ("FadeScroll", Range(0.1, 1)) = 1
        _EdgeShine ("EdgeShine", 2D) = "white" {}
        _FlowSpeed ("FlowSpeed", Range(-3, 3)) = 1
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Fresnel;
            uniform float _Emission;
            uniform sampler2D _FadeMap; uniform float4 _FadeMap_ST;
            uniform float _FadeScroll;
            uniform sampler2D _EdgeShine; uniform float4 _EdgeShine_ST;
            uniform float _FlowSpeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float node_7845 = 0.0;
                float gloss = node_7845;
                float perceptualRoughness = 1.0 - node_7845;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = node_7845;
                float specularMonochrome;
                float4 node_7828 = _Time;
                float node_4022 = (node_7828.g*_FlowSpeed);
                float2 node_9968 = (i.uv0+float2(node_4022,node_4022));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9968, _MainTex));
                float3 node_6343 = (_MainTex_var.rgb*_Color.rgb);
                float3 diffuseColor = node_6343; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_5174 = float2((_FadeScroll*i.uv0.r),(_FadeScroll*i.uv0.g));
                float2 node_5089 = ((1.0 - _MainTex_var.r)*node_5174);
                float4 _EdgeShine_var = tex2D(_EdgeShine,TRANSFORM_TEX(node_5089, _EdgeShine));
                float3 emissive = ((node_6343+(_Color.rgb*20.0*_EdgeShine_var.rgb))*_Emission);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                float4 _FadeMap_var = tex2D(_FadeMap,TRANSFORM_TEX(node_5174, _FadeMap));
                fixed4 finalRGBA = fixed4(finalColor,(_FadeMap_var.a*saturate((pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel)*3.5))));
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Fresnel;
            uniform float _Emission;
            uniform sampler2D _FadeMap; uniform float4 _FadeMap_ST;
            uniform float _FadeScroll;
            uniform sampler2D _EdgeShine; uniform float4 _EdgeShine_ST;
            uniform float _FlowSpeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float node_7845 = 0.0;
                float gloss = node_7845;
                float perceptualRoughness = 1.0 - node_7845;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = node_7845;
                float specularMonochrome;
                float4 node_7828 = _Time;
                float node_4022 = (node_7828.g*_FlowSpeed);
                float2 node_9968 = (i.uv0+float2(node_4022,node_4022));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9968, _MainTex));
                float3 node_6343 = (_MainTex_var.rgb*_Color.rgb);
                float3 diffuseColor = node_6343; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                float2 node_5174 = float2((_FadeScroll*i.uv0.r),(_FadeScroll*i.uv0.g));
                float4 _FadeMap_var = tex2D(_FadeMap,TRANSFORM_TEX(node_5174, _FadeMap));
                fixed4 finalRGBA = fixed4(finalColor * (_FadeMap_var.a*saturate((pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel)*3.5))),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Emission;
            uniform float _FadeScroll;
            uniform sampler2D _EdgeShine; uniform float4 _EdgeShine_ST;
            uniform float _FlowSpeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_7828 = _Time;
                float node_4022 = (node_7828.g*_FlowSpeed);
                float2 node_9968 = (i.uv0+float2(node_4022,node_4022));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9968, _MainTex));
                float3 node_6343 = (_MainTex_var.rgb*_Color.rgb);
                float2 node_5174 = float2((_FadeScroll*i.uv0.r),(_FadeScroll*i.uv0.g));
                float2 node_5089 = ((1.0 - _MainTex_var.r)*node_5174);
                float4 _EdgeShine_var = tex2D(_EdgeShine,TRANSFORM_TEX(node_5089, _EdgeShine));
                o.Emission = ((node_6343+(_Color.rgb*20.0*_EdgeShine_var.rgb))*_Emission);
                
                float3 diffColor = node_6343;
                float specularMonochrome;
                float3 specColor;
                float node_7845 = 0.0;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, node_7845, specColor, specularMonochrome );
                float roughness = 1.0 - node_7845;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
