using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;
using UnityEngine.EventSystems;

[System.Serializable]
public class DegreeRotation : IItemRotator
{
    #region Euler Rotation
    [Header("Rotation degrees")]
    [SerializeField]
    float pitchRotation;
    [SerializeField]
    float yawRotation;
    [SerializeField]
    float rollRotation;

    #endregion
    // NOTE: Y: Yaw, X: Pitch, Z: Roll

    [Header("Tranform")]
    [SerializeField]
    MyTransform transform;
    MyVector3 degreeRotation = new MyVector3();

    /// <summary>
    /// Sets the rotation of the object as well as updating to match input
    /// </summary>
    public void OnRotateUpdate(MyVector3 eulerAngles)
    {
        UpdateEulerAngles(eulerAngles);
        transform.SetRotation(MyMathsLibrary.VectorDegreeValuesToRadians(degreeRotation));
    }

    /// <summary>
    /// Sets the target to rotate
    /// </summary>
    /// <param name="transform"></param>
    public void SetRotationTarget(MyTransform transform)
    {
        this.transform = transform;
    }

    public void UpdateEulerAngles(MyVector3 eulerAngles)
    {
        pitchRotation = eulerAngles.x;
        yawRotation = eulerAngles.y;
        rollRotation = eulerAngles.z;

        degreeRotation.y = yawRotation;
        degreeRotation.x = pitchRotation;
        degreeRotation.z = rollRotation;
    }

    public MyVector3 GetEulers()
    {
        return new MyVector3(pitchRotation, yawRotation);
    }
}
