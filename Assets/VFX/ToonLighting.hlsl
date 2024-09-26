#ifndef TOON_LIGHTING_INCLUDED
#define TOON_LIGHTING_INCLUDED

struct ToonLightingData
{
    float3 normalWS;
    float3 viewDirectionWS;
    float4 specularColor;
    float4 ambientColor;
    UnityTexture2D rampTexture;
    UnitySamplerState sampler_rampTexture;
    float3 albedo;
    float smoothness;
};
#ifndef SHADERGRAPH_PREVIEW
float GetSmoothnessPower(float rawSmoothness)
{
    return exp2(10* rawSmoothness + 1);
}

float3 ToonLightHandling(ToonLightingData toonLightingData, Light light)
{
    float diffuse = dot(toonLightingData.normalWS, light.direction);
    float2 rampUV = float2(1 - (diffuse * 0.5 + 0.5), 0.5);
    float lightIntensity = SAMPLE_TEXTURE2D(toonLightingData.rampTexture, toonLightingData.sampler_rampTexture, rampUV).r;
    float3 radiance = light.color * lightIntensity;
    float specularDot = dot(toonLightingData.normalWS, normalize(light.direction + toonLightingData.viewDirectionWS));
    float specular = smoothstep(0.0005, 0.001, pow(specularDot, GetSmoothnessPower(toonLightingData.smoothness)) * lightIntensity);
    return toonLightingData.albedo * (toonLightingData.ambientColor + radiance + lerp(0, (specular * toonLightingData.specularColor), (toonLightingData.smoothness)));
    
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
void CalculateToonLighting_float(float3 albedo, float smoothness, float4 specularColor, float4 ambientColor, float3 worldspaceNormal, float3 viewDirection, UnityTexture2D rampTexture, UnitySamplerState sampler_rampTexture, out float3 finalCol)
{
    ToonLightingData toonLightingData;
    toonLightingData.albedo = albedo;
    toonLightingData.specularColor = specularColor;
    toonLightingData.normalWS = worldspaceNormal;
    toonLightingData.viewDirectionWS = viewDirection;
    toonLightingData.rampTexture = rampTexture;
    toonLightingData.sampler_rampTexture = sampler_rampTexture;
    toonLightingData.smoothness = smoothness;
    toonLightingData.ambientColor = ambientColor;
    finalCol = CalculateToonLighting(toonLightingData);
}
#endif