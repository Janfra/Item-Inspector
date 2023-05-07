using UnityEngine;
using MyMathsComponents;
using System.Runtime.CompilerServices;
using System;

[System.Serializable]
public class QuaternionRotation : IItemRotator
{
    MyTransform transform;

    [SerializeField]
    private MyVector3 radiansRotation;
    private MyVector3 rotationOffset = new MyVector3();

    [SerializeField]
    private MyVector3 axis;

    #region Slerp

    private float slerpTime = 0.0f;

    [SerializeField]
    private bool isSlerping = false;
    public bool IsSlerping => isSlerping;

    [SerializeField]
    private MyVector3 slerpStart = new MyVector3();
    [SerializeField]
    private MyVector3 slerpTarget = new MyVector3();
    private const float SLERP_DURATION = 3.0f;

    #endregion

    public void OnRotateUpdate(MyVector3 eulerAngles)
    {
        UpdateEulerAngles(eulerAngles);
        transform.SetQuatRotation(radiansRotation, true);
    }

    public void SetRotationTarget(MyTransform transform)
    {
        this.transform = transform;
    }

    public void UpdateEulerAngles(MyVector3 eulerAngles)
    {
        //MyVector3 offsetMultiplier = eulerAngles.NormalizeVector();
        //rotationOffset.x = ROTATION_OFFSET * MyMathsLibrary.RoundUp(offsetMultiplier.x);
        //rotationOffset.y = ROTATION_OFFSET * MyMathsLibrary.RoundUp(offsetMultiplier.y);
        //rotationOffset.z = ROTATION_OFFSET * MyMathsLibrary.RoundUp(offsetMultiplier.z);

        radiansRotation = MyMathsLibrary.VectorDegreeValuesToRadians(eulerAngles);
    }

    public MyVector3 GetEulers()
    {
        return radiansRotation;
    }

    public void SetSlerp(MyVector3 eulerAngles, MyVector3 currentRotation)
    {
        // If no changes are going to be made, return
        if(eulerAngles == currentRotation || transform == null)
        {
            return;
        }

        UpdateEulerAngles(currentRotation);
        MyVector3 targetRotation = MyMathsLibrary.VectorDegreeValuesToRadians(eulerAngles);

        Quat start = Quat.EulerToQuaternion(radiansRotation, false);
        Quat target = Quat.EulerToQuaternion(targetRotation, false);

        slerpStart = transform.Rotation;
        slerpTarget = eulerAngles;

        transform.SetSlerp(start, target, eulerAngles);
        radiansRotation = eulerAngles;
        isSlerping = true;
        slerpTime = 0.0f;
    }

    public void SlerpToTarget()
    {
        slerpTime += SettingsManager.Instance.GetTime();
        slerpTime = Mathf.Clamp01(slerpTime);

        transform.SlerpToTarget(slerpTime);
        if(slerpTime == 1)
        {
            isSlerping = false;
            slerpTime = 0.0f;
        }
    }
}

#region OUTDATED

//private const float ROTATION_OFFSET = 280f;

//public void SlerpObject()
//{
//    slerpTime += SettingsManager.Instance.GetTime();
//    float progress = SettingsManager.Instance.GetInterpolationAlpha(slerpTime, SLERP_DURATION);
//    transform.Slerp(MyMathsLibrary.VectorDegreeValuesToRadians(slerpStart), MyMathsLibrary.VectorDegreeValuesToRadians(slerpTarget), progress);

//    if (progress == 1)
//    {
//        isSlerping = false;
//        slerpTime = 0.0f;
//    }
//}


//public void SetSlerping(MyVector3 slerpTarget, MyVector3 slerpStart, bool isSlerping = true)
//{
//    if (transform == null)
//    {
//        return;
//    }

//    this.slerpTarget = slerpTarget;
//    this.slerpStart = slerpStart;
//    this.isSlerping = isSlerping;
//    slerpTime = 0.0f;
//}

//public void SetSlerping(MyVector3 slerpTarget, bool isSlerping = true)
//{
//    if (transform == null)
//    {
//        return;
//    }

//    slerpStart = transform.Rotation;
//    this.slerpTarget = slerpTarget;
//    this.isSlerping = isSlerping;
//    slerpTime = 0.0f;
//}

#endregion
