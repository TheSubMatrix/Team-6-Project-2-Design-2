#ifndef TOON_LIGHTING_INCLUDED
#define TOON_LIGHTING_INCLUDED

struct ToonLightingData
{
    float3 normalWS;
    UnityTexture2D rampTexture;
    UnitySamplerState sampler_rampTexture;
    float3 albedo;
};
#ifndef SHADERGRAPH_PREVIEW
float3 ToonLightHandling(ToonLightingData toonLightingData, Light light)
{
    float3 radiance = light.color;
    float diffuse = dot(toonLightingData.normalWS, light.direction);
    float2 rampUV = float2(1 - (diffuse * 0.5 + 0.5), 0.5);
    float4 rampValue = SAMPLE_TEXTURE2D(toonLightingData.rampTexture, toonLightingData.sampler_rampTexture, rampUV);
    return toonLightingData.albedo * radiance * rampValue.r;
    
}
#endif
float3 CalculateToonLighting(ToonLightingData toonLightingData)
{
    //Fix for urp lighting not being included in shader graph preview
    #ifdef SHADERGRAPH_PREVIEW
    float lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(toonLightingData.normalWS, lightDir));
    return toonLightingData.albedo * intensity;
    #else
    Light mainLight = GetMainLight();
    float3 color = 0;
    color += ToonLightHandling(toonLightingData, mainLight);
    return color;
    #endif

}

//Wrapper for use in shader graph
void CalculateToonLighting_float(float3 albedo, float3 worldspaceNormal, UnityTexture2D rampTexture, UnitySamplerState sampler_rampTexture, out float3 finalCol)
{
    ToonLightingData toonLightingData;
    toonLightingData.albedo = albedo;
    toonLightingData.normalWS = worldspaceNormal;
    toonLightingData.rampTexture = rampTexture;
    toonLightingData.sampler_rampTexture = sampler_rampTexture;
    finalCol = CalculateToonLighting(toonLightingData);
}
#endif