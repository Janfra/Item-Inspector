using MyMathsComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public MyVector3 eulerAngles;

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
                degreeRotation.UpdateEulerAngles(currentRotator.GetEulers());
                currentRotator = degreeRotation;
                break;

            case SettingsManager.RotationType.Quaternion:
                quaternionRotation.UpdateEulerAngles(currentRotator.GetEulers());
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
        MyVector3 topOrientation = new MyVector3(MyMathsLibrary.DegreesToRadians(90), 0, 0);
        quaternionRotation.SetSlerp(topOrientation);
    }

    #endregion
}

public interface IItemRotator
{
    void OnRotateUpdate();
    void SetRotationTarget(MyTransform transform);
    void UpdateEulerAngles(MyVector3 eulerAngles);
    MyVector3 GetEulers();
}