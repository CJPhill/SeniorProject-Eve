using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("Lighting Settings")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    public bool timeStopped = false;

    [SerializeField, Range(0, 24)] public float TimeOfDay;
    
    [Header("Time Settings")]
    [SerializeField] private float dayDurationInMinutes = 10f; // How long a full day should last in real time
    private float timeMultiplier;

    private void Start()
    {
        TimeOfDay = 7.50f;
        CalculateTimeMultiplier();
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying && !timeStopped)
        {
            TimeOfDay += (Time.deltaTime / timeMultiplier);
            TimeOfDay %= 24; // Keep TimeOfDay within 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        CalculateTimeMultiplier();
        
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
            DirectionalLight = RenderSettings.sun;
    }

    private void CalculateTimeMultiplier()
    {
        // Converts real-time duration in minutes to a multiplier for a 24-hour cycle
        timeMultiplier = (dayDurationInMinutes * 60f) / 24f;
    }
}
