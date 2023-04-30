using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;
using TMPro;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public static Action<RotationType> OnRotationTypeUpdated;

    [Header("Private Access")]
    private float timeScalar = 1f;
    private EasingFunctions easingType = EasingFunctions.EaseIn;
    private RotationType rotationType = RotationType.Degrees;

    [Header("UI References")]
    [SerializeField]
    private TMP_Dropdown easingOptionsDropdown;

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

    private void Start()
    {
        if(easingOptionsDropdown != null && Instance == this)
        {
            ItemSelection.OnTransitionCompleted += SwitchDropdown;
            ItemSelection.OnTransition += SwitchDropdown;
        }
    }

    private void OnDisable()
    {
        if(Instance == this)
        {
            ItemSelection.OnTransition -= SwitchDropdown;
            ItemSelection.OnTransitionCompleted -= SwitchDropdown;
            OnRotationTypeUpdated = null;
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
    
    public void OnEasingTypeChanged(int value)
    {
        value = Mathf.Clamp(value, (int)EasingFunctions.EaseIn, (int)EasingFunctions.EaseInOut);
        easingType = (EasingFunctions)value;
    }

    private void SwitchDropdown()
    {
        easingOptionsDropdown.interactable = !easingOptionsDropdown.interactable;
    }

    private enum SpeedSettings
    {
        slow,
        normal,
        fast
    }
    
    public void SetRotationType(int value)
    {
        value = Mathf.Clamp(value, (int)RotationType.Degrees, (int)RotationType.Quaternion);
        rotationType = (RotationType)value;
        Debug.Log($"Set to {rotationType}");
    }

    public enum RotationType
    {
        Degrees,
        Quaternion,
    }

    #endregion
}