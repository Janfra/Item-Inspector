using MyMathsComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private DegreeRotation degreeRotation;
    [SerializeField]
    private QuaternionRotation quaternionRotation;
    private IItemRotator currentRotator;
    private MyTransform selectedObjectTransform;

    private void Awake()
    {
        currentRotator = degreeRotation;
    }

    private void Start()
    {
        SettingsManager.OnRotationTypeUpdated += UpdateRotator;
    }

    private void OnDisable()
    {
        SettingsManager.OnRotationTypeUpdated -= UpdateRotator;
    }

    private void Update()
    {
        if (selectedObjectTransform == null) return;
        if (!quaternionRotation.IsSlerping)
        {
            currentRotator.OnRotateUpdate();
        }
        else
        {
            quaternionRotation.SimpleSlerp();
        }
    }

    private void UpdateRotator(SettingsManager.RotationType rotationType)
    {
        switch (rotationType)
        {
            case SettingsManager.RotationType.Degrees:
                currentRotator = degreeRotation;
                break;

            case SettingsManager.RotationType.Quaternion:
                currentRotator = quaternionRotation;
                break;

            default:
                Debug.LogError("No rotation type set in item rotator");
                break;
        }
    }

    public void SetRotationTarget(MyTransform transform)
    {
        selectedObjectTransform = transform;
        degreeRotation.SetRotationTarget(transform);
        quaternionRotation.SetRotationTarget(transform);
    }

    #region Button Functions

    public void SetItemOrientationToTop()
    {
        // quaternionRotation.SetSlerping((MyVector3.Up * 180) * -1);

        quaternionRotation.SetSlerp();
    }

    #endregion
}

public interface IItemRotator
{
    void OnRotateUpdate();
    void SetRotationTarget(MyTransform transform);
}