using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Private Access")]
    private float timeScalar = 1f;
    private EasingFunctions easingType = EasingFunctions.EaseInOut;

    // Constant speed values
    private const float slowSpeed = 0.5f;
    private const float normalSpeed = 1.0f;
    private const float fastSpeed = 2.0f;

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
        return Time.deltaTime * timeScalar;
    }

    public float GetInterpolationAlpha(float currentTime, float duration)
    {
        float alpha = 0.0f;

        switch (easingType)
        {
            case EasingFunctions.EaseIn:
                alpha = MyMathsLibrary.EaseInDecimal(currentTime, duration);
                break;
            case EasingFunctions.EaseOut:
                alpha = MyMathsLibrary.EaseOutDecimal(currentTime, duration);
                break;
            case EasingFunctions.EaseInOut:
                alpha = MyMathsLibrary.EaseInOutDecimal(currentTime, duration);
                break;
            default:
                Debug.LogError("No valid easing type selected in the Settings Manager");
                break;
        }

        // Debug.Log($"Current interpolation progression is: {alpha}");
        return alpha;
    }

    #region Settings Button Edit

    private void SetToSlowSpeed()
    {
        timeScalar = slowSpeed;
    }
    private void SetToNormalSpeed()
    {
        timeScalar = normalSpeed;
    }
    private void SetToFastSpeed()
    {
        timeScalar = fastSpeed;
    }

    public void SetSpeed(float value)
    {
        int speedSelected = Mathf.RoundToInt(value);
        speedSelected = Mathf.Clamp(speedSelected, (int)SpeedSettings.slow, (int)SpeedSettings.fast);
        SpeedSettings speedSetting = (SpeedSettings)speedSelected;

        switch (speedSetting)
        {
            case SpeedSettings.slow:
                SetToSlowSpeed();
                break;
            case SpeedSettings.normal:
                SetToNormalSpeed();
                break;
            case SpeedSettings.fast:
                SetToFastSpeed();
                break;
            default:
                Debug.LogError("Speed setting not set in SetSpeed in Settings Manager");
                break;
        }
    }
    
    private enum SpeedSettings
    {
        slow,
        normal,
        fast
    }

    #endregion
}