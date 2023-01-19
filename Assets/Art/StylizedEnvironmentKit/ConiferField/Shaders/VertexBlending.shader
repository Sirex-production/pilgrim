//--------------------------------------------
//Stylized Environment Kit
//LittleMarsh CG ART
//version 1.2.0
//--------------------------------------------

Shader "LMArtShader/VertexBlending"
{
	Properties
	{
		[Header(Red Channel)]
		[Space(7)][NoScaleOffset]_TopTex("Top Tex(Alpha used for Blending)", 2D) = "white" {}
		[MaterialToggle]_TopWorldUV("Use World UV", Float) = 0
		_TopUVScale("World UV Scale", Range(0,1)) = 0.5

		[NoScaleOffset]_TopBumpMap("Top Normal Map", 2D) = "bump" {}
		_TopBumpScale("Top Normal Scale", Range(0,1)) = 1.0

		[Space(24)][Header(Green Channel)]
		[Space(7)][NoScaleOffset]_MidTex("Middle Tex(Alpha used for Blending)", 2D) = "white" {}
		[MaterialToggle]_MidWorldUV("Use World UV", Float) = 0
		_MidUVScale("World UV Scale", Range(0,1)) = 0.5

		[NoScaleOffset]_MidBumpMap("Middle Normal Map", 2D) = "bump" {}
		_MidBumpScale("Middle Normal Scale", Range(0,1)) = 1.0

		[Space(24)][Header(Blue Channel)]
		[Space(7)][NoScaleOffset]_BtmTex("Bottom Tex(Alpha used for Blending)", 2D) = "white" {}
		[MaterialToggle]_BtmWorldUV("Use World UV", Float) = 0
		_BtmUVScale("World UV Scale", Range(0,1)) = 0.5

		[NoScaleOffset]_BtmBumpMap("Bottom Normal Map", 2D) = "bump" {}
		_BtmBumpScale("Buttom Normal Scale", Range(0,1)) = 1.0

		[Space(24)][Header(Others)]
		[Space(7)]_BlendFactor("BlendFactor", Range(0, 0.5)) = 0.2
		_ReflectionColor("Reflection Light Color", Color) = (0.5,0.5,0.5,1)

	}

		SubShader
		{

			Pass
			{
				Tags {
				"LightMode" = "ForwardBase"
				"Queue" = "Geometry"
				"RenderType" = "Opaque"
				}

				LOD 200

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
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
					float4 color : COLOR;
					float3 worldPos : TEXCOORD7;
				};


				v2f vert(appdata_VB v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;

					o.tspace0 = tspace(v.normal, v.tangent)[0];
					o.tspace1 = tspace(v.normal, v.tangent)[1];
					o.tspace2 = tspace(v.normal, v.tangent)[2];

					TRANSFER_SHADOW(o)

					o.viewDir = WorldSpaceViewDir(v.vertex);
					o.color = v.color;
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

					UNITY_TRANSFER_FOG(o,o.pos);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{

					half2 worldUV = i.worldPos.xz;
					half2 topUV = lerp(i.uv, worldUV * _TopUVScale, _TopWorldUV);
					half2 midUV = lerp(i.uv, worldUV * _MidUVScale, _MidWorldUV);
					half2 btmUV = lerp(i.uv, worldUV * _BtmUVScale, _BtmWorldUV);

					fixed4 topcol = tex2D(_TopTex, topUV);
					fixed4 midcol = tex2D(_MidTex, midUV);
					fixed4 btmcol = tex2D(_BtmTex, btmUV);

					half2 maskBD = maskBlending(i.color, topcol, midcol, btmcol);

					//Color Blending
					fixed4 col = btmcol;
					col = lerp(col, midcol, maskBD.x);
					col = lerp(col, topcol, maskBD.y);


					half3 topNormal = UnpackScaleNormal(tex2D(_TopBumpMap, topUV), _TopBumpScale);
					half3 midNormal = UnpackScaleNormal(tex2D(_MidBumpMap, midUV), _MidBumpScale);
					half3 btmNormal = UnpackScaleNormal(tex2D(_BtmBumpMap, btmUV), _BtmBumpScale);

					//Normal Blending
					half3 tNormal = btmNormal;
					tNormal = lerp(tNormal, midNormal, maskBD.x);
					tNormal = lerp(tNormal, topNormal, maskBD.y);

					half3 wNormal;
					wNormal.x = dot(i.tspace0, tNormal);
					wNormal.y = dot(i.tspace1, tNormal);
					wNormal.z = dot(i.tspace2, tNormal);
					wNormal = normalize(wNormal);

					half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

					fixed shadow = SHADOW_ATTENUATION(i);
					fixed atten = 0;

					half3 reflColor = worldReflect(i.worldPos, wNormal);

					col = computeVBCol(col, wNormal, lightDir, i.worldPos, shadow, atten, reflColor);

					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}


			Pass
			{
				Tags {
				"LightMode" = "ForwardAdd"
				"Queue" = "Geometry"
				"RenderType" = "Opaque"
				}

				LOD 200
				Blend One One

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
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
					float4 color : COLOR;
					float3 worldPos : TEXCOORD7;
					LIGHTING_COORDS(8, 9)
				};


				v2f vert(appdata_VB v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;

					o.tspace0 = tspace(v.normal, v.tangent)[0];
					o.tspace1 = tspace(v.normal, v.tangent)[1];
					o.tspace2 = tspace(v.normal, v.tangent)[2];

					o.viewDir = WorldSpaceViewDir(v.vertex);
					o.color = v.color;
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

					UNITY_TRANSFER_FOG(o,o.pos);
					TRANSFER_VERTEX_TO_FRAGMENT(o);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{

					half2 worldUV = i.worldPos.xz;
					half2 topUV = lerp(i.uv, worldUV * _TopUVScale, _TopWorldUV);
					half2 midUV = lerp(i.uv, worldUV * _MidUVScale, _MidWorldUV);
					half2 btmUV = lerp(i.uv, worldUV * _BtmUVScale, _BtmWorldUV);

					/////////////////////////Base
					fixed4 topcol = tex2D(_TopTex, topUV);
					fixed4 midcol = tex2D(_MidTex, midUV);
					fixed4 btmcol = tex2D(_BtmTex, btmUV);

					half2 maskBD = maskBlending(i.color, topcol, midcol, btmcol);

					//Color Blending
					fixed4 col = btmcol;
					col = lerp(col, midcol, maskBD.x);
					col = lerp(col, topcol, maskBD.y);


					/////////////////////////Normal
					half3 topNormal = UnpackScaleNormal(tex2D(_TopBumpMap, topUV), _TopBumpScale);
					half3 midNormal = UnpackScaleNormal(tex2D(_MidBumpMap, midUV), _MidBumpScale);
					half3 btmNormal = UnpackScaleNormal(tex2D(_BtmBumpMap, btmUV), _BtmBumpScale);

					//Normal Blending
					half3 tNormal = btmNormal;
					tNormal = lerp(tNormal, midNormal, maskBD.x);
					tNormal = lerp(tNormal, topNormal, maskBD.y);

					half3 wNormal;
					wNormal.x = dot(i.tspace0, tNormal);
					wNormal.y = dot(i.tspace1, tNormal);
					wNormal.z = dot(i.tspace2, tNormal);
					wNormal = normalize(wNormal);

					half3 lightDir = lerp(_WorldSpaceLightPos0.xyz, (_WorldSpaceLightPos0.xyz - i.worldPos.xyz), _WorldSpaceLightPos0.w);
					lightDir = normalize(lightDir);

					fixed shadow = 1.0;
					fixed atten = LIGHT_ATTENUATION(i);

					half3 reflColor = fixed3(0, 0, 0);

					col = computeVBCol(col, wNormal, lightDir, i.worldPos, shadow, atten, reflColor);

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

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					V2F_SHADOW_CASTER;
				};


				v2f vert(appdata v)
				{
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG
			}
		}
}