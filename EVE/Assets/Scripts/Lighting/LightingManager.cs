using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField, Range(0,24)] private float timeOfDay;

    [SerializeField, Range(-10, 10)] private float speedMultiplier;  // used to adjust the cycle time. Note that values < 0 will reverse it!
    [SerializeField, Range(1, 10)] private float nightSpeed; // how much to speed up late-night hours
    [SerializeField] private float maxIntensity = 1.5f;
    private float baseIntensity = 0f;
    [SerializeField] private float maxShadowStrength = 1f;
    [SerializeField] private float minShadowStrength = 0.2f;
    private float nightSpeedUpStart = 20f;
    private float nightSpeedUpEnd = 4f;
    private float dawn = 6f;
    private float dusk = 18f;
    private float noon = 12f;

    private void Start()
    {
        // default values
        speedMultiplier = 0.1f; 
        nightSpeed = 10.0f;
        baseIntensity = maxIntensity / 2f;
    }
    
    private void Update(){
        if(Preset == null)
            return;

        if (timeOfDay > nightSpeedUpStart || timeOfDay < nightSpeedUpEnd){
                timeOfDay += Time.deltaTime * nightSpeed;
        }
        else {
            timeOfDay += Time.deltaTime * speedMultiplier;

            // adjust light intensity and shadow softness for time of day
            if (timeOfDay >= dawn && timeOfDay <= noon) {
                DirectionalLight.intensity = baseIntensity + (baseIntensity / (noon - dawn)) * (timeOfDay - dawn);
                DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (noon - dawn)) * (timeOfDay - dawn);
            }
            else if (timeOfDay > noon && timeOfDay <= dusk) {
                DirectionalLight.intensity = baseIntensity + (baseIntensity / (dusk - noon)) * (dusk - timeOfDay);
                DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (dusk - noon)) * (dusk - timeOfDay);
            }
            else {
                DirectionalLight.intensity = baseIntensity;
                DirectionalLight.shadowStrength = minShadowStrength;
            }
        }
        timeOfDay %= 24; //Modulus to ensure always between 0-24
        UpdateLighting(timeOfDay / 24f);

        // if(Application.isPlaying){
        //     timeOfDay += Time.deltaTime;
        //     timeOfDay %= 24;
        //     UpdateLighting(timeOfDay/24f);
        // }
        // else{
        //     UpdateLighting(timeOfDay/24f);
        // }
    }


    private void UpdateLighting(float timePercent){
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight != null) {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }

    private void onValidate(){
        if(DirectionalLight != null)
            return;
        
        if(RenderSettings.sun != null)
            DirectionalLight = RenderSettings.sun;
        
        else{
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights){
                if(light.type == LightType.Directional){
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}