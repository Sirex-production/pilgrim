//--------------------------------------------
//Stylized Environment Kit
//LittleMarsh CG ART
//version 1.2.0
//--------------------------------------------

#ifndef LMArtShader_INCLUDED
#define LMArtShader_INCLUDED

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

/////////////Nature Leaves//////////////////////////

struct appdata_lf
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
	fixed4 color : COLOR;
	float4 tangent : TANGENT;
};


sampler2D _MainTex, _BumpMap, _SpecularTex, _TransTex;
fixed _Cutoff, _TransArea, _BumpScale, _Shininess, _SpecularPower, _ShadowAttenuation, _TransPower;
half _AnimationScale;
fixed3 _TransColor, _SpecularColor;

float rand(float2 st)
{
	return frac(sin(dot(st, float2(12.9898, 78.233))) * 43758.5453);
}

inline float3 vertexAnimation(float3 worldPos, float4 vertex, fixed4 color)
{
	float t1 = _Time.y + (worldPos.x * 0.6) - (worldPos.y * 0.6) - (worldPos.z * 0.8);
	float t2 = _Time.y + (worldPos.x * 2) + (worldPos.z * 3);

	float randNoise = rand(float2((worldPos.x * 0.8), (worldPos.z * 0.8)));

	float x = 1.44* pow(cos(t1 / 2), 2) + sin(t1 / 2)* randNoise;
	float y = (2 * sin(3 * x) + sin(10 * x) - cos(5 * x)) / 10 * randNoise;

	float3 move = float3(0, 0, 0);
	move.x = lerp(0, y, color.r) / 2 * _AnimationScale;
	move.y = lerp(0, y, color.r) / 4 * _AnimationScale;
	move.z = lerp(0, sin(t2) * cos(2 * t2) * _AnimationScale / 10, color.r);

	return move;
}

inline float3x3 tspace(float3 normal, float4 tangent)
{
	half3 worldNormal = UnityObjectToWorldNormal(normal);
	//half3 worldNormal = normal;
	half3 worldTangent = UnityObjectToWorldDir(tangent.xyz);
	//half3 worldTangent = tangent.xyz;
	half tangentSign = tangent.w * unity_WorldTransformParams.w;
	half3 wBitangent = cross(worldNormal, worldTangent) * tangentSign;

	float3x3 tspaceC;
	tspaceC[0] = half3(worldTangent.x, wBitangent.x, worldNormal.x);
	tspaceC[1] = half3(worldTangent.y, wBitangent.y, worldNormal.y);
	tspaceC[2] = half3(worldTangent.z, wBitangent.z, worldNormal.z);

	return tspaceC;
}

inline half3 worldNormal(float3 tspace0, float3 tspace1, float3 tspace2, half3 tNormal, fixed facing)
{
	half3 wNormal;
	wNormal.x = dot(tspace0, tNormal);
	wNormal.y = dot(tspace1, tNormal);
	wNormal.z = dot(tspace2, tNormal);
	wNormal = lerp(-wNormal, wNormal, step(0, facing));
	wNormal = normalize(wNormal);

	return wNormal;
}

inline fixed4 computeLFCol(fixed4 col, half3 viewDir, half3 lightDir, half3 wNormal, fixed shadow, fixed atten,
	fixed4 specularTex, fixed4 transTex)
{
	shadow = lerp(1.0, shadow, _ShadowAttenuation);
	fixed lightCal = lerp(shadow, atten, _WorldSpaceLightPos0.w);

	half3 ambient = ShadeSH9(half4(wNormal, 1));
	ambient = lerp(ambient, 0, _WorldSpaceLightPos0.w);

	half3 nl = max(0, dot(wNormal, lightDir));
	half3 nlr = max(0, dot(-wNormal, lightDir));
	nlr = smoothstep(_TransArea, 1, nlr);

	//Specular
	half3 R = reflect(-lightDir, wNormal);
	half specularReflection = clamp(dot(viewDir, R), 0.0, 1.0);
	half3 specular = pow(specularReflection, _Shininess) * _SpecularPower * specularTex * _LightColor0.rgb * _SpecularColor.rgb;
	specular = lerp(fixed3(0, 0, 0), specular, lightCal);

	half3 diffuse = nl * _LightColor0.rgb * lightCal + ambient;
	half3 trans = lerp(fixed3(0, 0, 0), _LightColor0.rgb * transTex * _TransColor.rgb, nlr * lightCal);
	half3 Translucency = _TransPower * _LightColor0.rgb * transTex * _TransColor.rgb
		* pow(max(0.0, dot(lightDir, -wNormal)), _TransArea);
	Translucency = lerp(fixed3(0, 0, 0), Translucency, lightCal);

	col.rgb *= diffuse;
	col.rgb += specular;
	col.rgb += trans;
	col.rgb += Translucency;

	return col;
}

struct appdata_shd
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
	fixed4 color : COLOR;
};


/////////////VertexBlending//////////////////////////

sampler2D _TopTex, _TopBumpMap, _MidTex, _MidBumpMap, _BtmTex, _BtmBumpMap, _TopReflectMap, _MidReflectMap, _BtmReflectMap;
fixed _TopBumpScale, _MidBumpScale, _BtmBumpScale, _BlendFactor, _TopUVScale, _MidUVScale, _BtmUVScale, _TopReflectScale, _MidReflectScale, _BtmReflectScale;
float4 _TopTex_ST, _MidTex_ST, _BtmTex_ST;
float _TopWorldUV, _MidWorldUV, _BtmWorldUV;
fixed3 _ReflectionColor;

struct appdata_VB
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
	fixed4 color : COLOR;
	float4 tangent : TANGENT;
};

inline half2 maskBlending(float4 color, fixed4 topcol, fixed4 midcol, fixed4 btmcol)
{
	half vertexRw = smoothstep(0.3, 1, color.r);
	half vertexGw = smoothstep(0.3, 1, color.g);
	half vertexBw = smoothstep(0.3, 1, color.b);
	half vertexRb = smoothstep(0, 0.7, color.r);
	half vertexGb = smoothstep(0, 0.7, color.g);
	half vertexBb = smoothstep(0, 0.7, color.b);

	half maskR = lerp(0, topcol.a, smoothstep(0, 0.5, color.r));
	half maskG = lerp(0, midcol.a, smoothstep(0, 0.5, color.g));
	half maskB = lerp(0, btmcol.a, smoothstep(0, 0.5, max(0, 1 - color.r - color.g)));

	//RGBmask
	half maskRGB = smoothstep(0, (0.5 - _BlendFactor), max(0, maskR - maskB - maskG));
	maskRGB = saturate(maskRGB * vertexRb + vertexRw);
	maskRGB = smoothstep(0, 0.4, maskRGB);

	//GBmask
	half maskGB = smoothstep(0, (0.5 - _BlendFactor), max(0, maskG - maskB - maskR));
	maskGB = saturate(maskGB * vertexGb + vertexGw);
	maskGB = smoothstep(0, 0.4, maskGB);

	return half2(maskGB, maskRGB);
}

inline half3 worldReflect(float3 worldPos, half3 wNormal)
{
	half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
	half3 worldRefl = reflect(-worldViewDir, wNormal);
	half4 reflData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
	half3 reflColor = DecodeHDR(reflData, unity_SpecCube0_HDR);

	return reflColor;
}

inline fixed4 computeVBCol(fixed4 col, half3 wNormal, half3 lightDir, float3 worldPos, fixed shadow, fixed atten, half3 reflColor)
{
	fixed lightCal = lerp(shadow, atten, _WorldSpaceLightPos0.w);

	half3 ambient = ShadeSH9(half4(wNormal, 1));
	ambient = lerp(ambient, 0, _WorldSpaceLightPos0.w);

	half3 nl = max(0, dot(wNormal, lightDir));
	half3 nll = max(0, dot(wNormal, -lightDir));

	half3 diffuse = nl * _LightColor0.rgb * lightCal + ambient;
	half3 reflectLight = nll * reflColor * (1 - lightCal) * _ReflectionColor;

	col.rgb *= diffuse;
	col.rgb += reflectLight;
	col.a = 1.0;

	return col;
}
#endif