//--------------------------------------------
//Stylized Environment Kit
//LittleMarsh CG ART
//version 1.1.0
//--------------------------------------------

Shader "LMArtShader/NatureLeaves"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Albedo Tex", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5

		[NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpScale("Normal Scale", Range(0,1)) = 1.0
		_AnimationScale("Animation Scale", Range(0,1)) = 1.0

		[Space(16)][Header(Back Leaf)]
		[Space(7)]
		[HDR]_TransColor("BackLeaf Color", Color) = (1, 1, 1, 1)
		[NoScaleOffset]_TransTex("BackLeaf ColorTex", 2D) = "white" {}
		_TransArea("BackLeaf Range", Range(0.01,1)) = 0.5
		_TransPower("Translucent Scale", Range(0,1)) = 1.0

		[Space(16)][Header(Specular)]
		[Space(7)]
		[HDR]_SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
		[NoScaleOffset]_SpecularTex("Specular ColorTex", 2D) = "white" {}
		_Shininess("Shininess", Range(1,96)) = 12
		_SpecularPower("Specular Power", Range(0,3)) = 1.0

		[Space(16)][Header(Shadow)]
		[Space(7)]
		_ShadowAttenuation("Shadow Intensity", Range(0,1)) = 1.0
	}
		SubShader
		{


		Pass
		{
				Tags {
				"LightMode" = "ForwardBase"
				"Queue" = "AlphaTest"
				"RenderType" = "TransparentCutout"
				}

				Cull Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_fwdbase_fullshadows
				#pragma multi_compile_fog
				#include "LMArtShader.cginc"


			struct v2f
			{
				float2 uv : TEXCOORD0;
				SHADOW_COORDS(1)
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
				float3 tspace0 : TEXCOORD3;
				float3 tspace1 : TEXCOORD4;
				float3 tspace2 : TEXCOORD5;
				float3 viewDir : TEXCOORD6;
			};


			v2f vert(appdata_lf v)
			{
				v2f o;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				v.vertex.x += vertexAnimation(worldPos, v.vertex, v.color).x;
				v.vertex.y += vertexAnimation(worldPos, v.vertex, v.color).y;
				v.vertex.z += vertexAnimation(worldPos, v.vertex, v.color).z;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				o.tspace0 = tspace(v.normal, v.tangent)[0];
				o.tspace1 = tspace(v.normal, v.tangent)[1];
				o.tspace2 = tspace(v.normal, v.tangent)[2];

				o.viewDir = WorldSpaceViewDir(v.vertex);

				TRANSFER_SHADOW(o)
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}


			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				half3 tNormal = UnpackScaleNormal(tex2D(_BumpMap, i.uv), _BumpScale);
				half3 wNormal = worldNormal(i.tspace0, i.tspace1, i.tspace2, tNormal, facing);

				half3 viewDir = normalize(i.viewDir);
				half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

				fixed shadow = SHADOW_ATTENUATION(i);
				fixed atten = 0;
				fixed4 specularTex = tex2D(_SpecularTex, i.uv);
				fixed4 transTex = tex2D(_TransTex, i.uv);

				col = computeLFCol(col, viewDir, lightDir, wNormal, shadow, atten, specularTex, transTex);

				clip(col.a - _Cutoff);
				col.a = 1;

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG

		}

				Pass
		{
				Tags {
				"LightMode" = "ForwardAdd"
				"Queue" = "AlphaTest"
				"RenderType" = "TransparentCutout"
				}

				Cull Off
				Blend One One

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_fwdadd_fullshadows
				#pragma multi_compile_fog
				#include "LMArtShader.cginc"


			struct v2f
			{
				float2 uv : TEXCOORD0;
				//SHADOW_COORDS(1)
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
				float3 tspace0 : TEXCOORD3; // tangent.x, bitangent.x, normal.x
				float3 tspace1 : TEXCOORD4; // tangent.y, bitangent.y, normal.y
				float3 tspace2 : TEXCOORD5; // tangent.z, bitangent.z, normal.z
				float3 viewDir : TEXCOORD6;
				float3 worldPos : TEXCOORD7;
				LIGHTING_COORDS(8, 9)

			};

			v2f vert(appdata_lf v)
			{
				v2f o;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				v.vertex.x += vertexAnimation(worldPos, v.vertex, v.color).x;
				v.vertex.y += vertexAnimation(worldPos, v.vertex, v.color).y;
				v.vertex.z += vertexAnimation(worldPos, v.vertex, v.color).z;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				o.tspace0 = tspace(v.normal, v.tangent)[0];
				o.tspace1 = tspace(v.normal, v.tangent)[1];
				o.tspace2 = tspace(v.normal, v.tangent)[2];

				o.viewDir = WorldSpaceViewDir(v.vertex);

				o.worldPos = worldPos;

				UNITY_TRANSFER_FOG(o, o.pos);
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}


			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				half3 tNormal = UnpackScaleNormal(tex2D(_BumpMap, i.uv), _BumpScale);
				half3 wNormal = worldNormal(i.tspace0, i.tspace1, i.tspace2, tNormal, facing);

				half3 viewDir = normalize(i.viewDir);

				half3 lightDir = lerp(_WorldSpaceLightPos0.xyz, (_WorldSpaceLightPos0.xyz - i.worldPos.xyz), _WorldSpaceLightPos0.w);
				lightDir = normalize(lightDir);

				fixed shadow = 1.0;
				fixed atten = LIGHT_ATTENUATION(i);
				fixed4 specularTex = tex2D(_SpecularTex, i.uv);
				fixed4 transTex = tex2D(_TransTex, i.uv);

				col = computeLFCol(col, viewDir, lightDir, wNormal, shadow, atten, specularTex, transTex * 0.6);
				clip(col.a - _Cutoff);
				col.a = 1;

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG

		}


			Pass
			{
				Tags {
				"LightMode" = "ShadowCaster"
				}

				ZWrite On ZTest LEqual
				Cull Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#include "LMArtShader.cginc"


			struct v2f
			{

				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};


			v2f vert(appdata_shd v)
			{
				v2f o;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.uv = v.texcoord;

				v.vertex.x += vertexAnimation(worldPos, v.vertex, v.color).x;
				v.vertex.y += vertexAnimation(worldPos, v.vertex, v.color).y;
				v.vertex.z += vertexAnimation(worldPos, v.vertex, v.color).z;

				o.pos = UnityObjectToClipPos(v.vertex);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);
				clip(texcol.a - _Cutoff);
				texcol.a = 1;

				return texcol;

			}
			ENDCG
		}
	}
}
