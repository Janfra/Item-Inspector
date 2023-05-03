using UnityEngine;
using MyMathsComponents;
using System.Runtime.CompilerServices;

[System.Serializable]
public class QuaternionRotation : IItemRotator
{
    MyTransform transform;

    [SerializeField]
    private MyVector3 eulerAngles;
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

    public void OnRotateUpdate()
    {
        transform.SetQuatRotation(eulerAngles, true);
    }

    public void SetRotationTarget(MyTransform transform)
    {
        this.transform = transform;
    }

    public void UpdateEulerAngles(MyVector3 eulerAngles)
    {
        this.eulerAngles = MyMathsLibrary.VectorDegreeValuesToRadians(eulerAngles);
    }

    public MyVector3 GetEulers()
    {
        return eulerAngles;
    }

    public void SetSlerp(MyVector3 eulerAngles)
    {
        Quat start = Quat.EulerToQuaternion(transform.Rotation, false);
        Quat target = Quat.EulerToQuaternion(eulerAngles, false);

        slerpStart = transform.Rotation;
        slerpTarget = eulerAngles;

        transform.SetSlerp(start, target, eulerAngles);
        this.eulerAngles = eulerAngles;
        isSlerping = true;
    }

    public void SimpleSlerp()
    {
        slerpTime += SettingsManager.Instance.GetTime();
        slerpTime = Mathf.Clamp01(slerpTime);

        transform.SimpleSlerp(slerpTime);
        if(slerpTime == 1)
        {
            isSlerping = false;
            slerpTime = 0.0f;
        }
    }

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

    public void SetSlerping(MyVector3 slerpTarget, MyVector3 slerpStart, bool isSlerping = true)
    {
        if(transform == null)
        {
            return;
        }

        this.slerpTarget = slerpTarget;
        this.slerpStart = slerpStart;
        this.isSlerping = isSlerping;
        slerpTime = 0.0f;
    }

    public void SetSlerping(MyVector3 slerpTarget, bool isSlerping = true)
    {
        if(transform == null)
        {
            return;
        }

        slerpStart = transform.Rotation;
        this.slerpTarget = slerpTarget;
        this.isSlerping = isSlerping;
        slerpTime = 0.0f;
    }
}
