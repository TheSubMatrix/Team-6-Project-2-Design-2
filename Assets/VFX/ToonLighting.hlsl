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
    float metalness;
    float4 shadowCoord;
    float3 positionWS;
    float3 ambientOcclusion;
    float3 bakedGI;
};
#ifndef SHADERGRAPH_PREVIEW
float Quantize(float valueToQuantize, float bandCount)
{
    return (floor(valueToQuantize * (bandCount - 1) + 0.5) / (bandCount - 1));
}
float Remap(float v, float minOld, float maxOld, float minNew, float maxNew) {
    return minNew + (v-minOld) * (maxNew - minNew) / (maxOld-minOld);
}
float3 ToonGlobalIllumination(ToonLightingData toonLightingData)
{
    float3 indirectDiffuse = toonLightingData.albedo * toonLightingData.bakedGI * toonLightingData.ambientOcclusion;
    float3 reflectionVector = reflect(-toonLightingData.viewDirectionWS, toonLightingData.normalWS);
    float fresnel = Pow4(1- saturate(dot(normalize(toonLightingData.viewDirectionWS), normalize(toonLightingData.normalWS))));
    float rampedFresnel = Quantize(fresnel, 15);
    float3 indirectSpecular = (GlossyEnvironmentReflection(reflectionVector, RoughnessToPerceptualRoughness(1-toonLightingData.smoothness), toonLightingData.ambientOcclusion) * rampedFresnel);
    return indirectDiffuse + indirectSpecular;
}
float GetSmoothnessPower(float rawSmoothness)
{
    return exp2(10* rawSmoothness + 1);
}

float3 ToonLightHandling(ToonLightingData toonLightingData, Light light)
{
    float diffuse = saturate(dot(toonLightingData.normalWS, light.direction));
    float lightIntensity = Quantize(diffuse, 5);
    float3 radiance = light.color * (light.shadowAttenuation * light.distanceAttenuation);
    float specularDot = saturate(dot(toonLightingData.normalWS, normalize(light.direction + toonLightingData.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(toonLightingData.smoothness)) * diffuse;
    float specularIntensity = Quantize(specular, Remap(toonLightingData.metalness, 0, 1, 7, 2));
    float3 totalSpecular = specularIntensity * light.shadowAttenuation * toonLightingData.specularColor;
    return (toonLightingData.albedo * radiance * (lightIntensity + totalSpecular));
    
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
        Light mainLight = GetMainLight(toonLightingData.shadowCoord, toonLightingData.positionWS, 1);
        MixRealtimeAndBakedGI(mainLight, toonLightingData.normalWS, toonLightingData.bakedGI);
        float3 color = ToonGlobalIllumination(toonLightingData);
        color += ToonLightHandling(toonLightingData, mainLight);
        
        #ifdef _ADDITIONAL_LIGHTS
            uint additionalLightCount = GetAdditionalLightsCount();
            for(uint i = 0; i < additionalLightCount; i++)
            {
                Light light = GetAdditionalLight(i, toonLightingData.positionWS, 1);
                color += ToonLightHandling(toonLightingData, light);
            }
        #endif
        return color;
    #endif

}

//Wrapper for use in shader graph
void CalculateToonLighting_float(
    float3 position, 
    float3 albedo, 
    float smoothness,
    float metalness,
    float4 specularColor, 
    float3 worldspaceNormal,
    float3 viewDirection, 
    UnityTexture2D rampTexture,
    UnitySamplerState sampler_rampTexture,
    float ambientOcclusion,
    float2 lightmapUV,
    out float3 finalCol)
{
    ToonLightingData toonLightingData;
    toonLightingData.positionWS = position;
    toonLightingData.albedo = albedo;
    toonLightingData.specularColor = specularColor;
    toonLightingData.normalWS = worldspaceNormal;
    toonLightingData.viewDirectionWS = viewDirection;
    toonLightingData.rampTexture = rampTexture;
    toonLightingData.sampler_rampTexture = sampler_rampTexture;
    toonLightingData.smoothness = smoothness;
    toonLightingData.metalness = metalness;
    toonLightingData.ambientOcclusion = ambientOcclusion;
    #ifdef SHADERGRAPH_PREVIEW
        toonLightingData.shadowCoord = 0;
        toonLightingData.bakedGI = 0;
    #else
        float4 positionCS = TransformWorldToHClip(position);
        #if SHADOWS_SCREEN
            toonLightingData.shadowCoord = ComputeScreenPos(positionCS);
        #else
            toonLightingData.shadowCoord = TransformWorldToShadowCoord(position);
        #endif
        float calculatedLightmapUV;
        OUTPUT_LIGHTMAP_UV(lightmapUV, unity_LightmapST, lightmapUV)
        float3 vertexSH;
        OUTPUT_SH(worldspaceNormal, vertexSH);
        toonLightingData.bakedGI = SAMPLE_GI(calculatedLightmapUV, vertexSH, worldspaceNormal);
    #endif
    finalCol = CalculateToonLighting(toonLightingData);
}
#endif