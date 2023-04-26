using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Public Access")]
    private float time;

    [Header("Private Access")]
    private float timeScalar = 1f;

    float value = 0f;
    float oldValue;
    float currentTime;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public float GetTime()
    {
        return time;
    }

    public float GetInterpolationAlpha()
    {
        float alpha = 0.0f;

        return alpha;
    }

    private void Update()
    {
        time = Time.deltaTime * timeScalar;
        currentTime += time;
        float duration = 5;

        value = MyMathsComponents.MyMathsLibrary.EaseInValueWithinRange(currentTime, 0, 1, duration);
        Debug.Log(value);
        oldValue = value;
    }
}

public enum SettingOptions
{

}