using UnityEngine;
using MyMathsComponents;

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
        transform.SetQuatRotation(eulerAngles);
    }

    public void SetRotationTarget(MyTransform transform)
    {
        this.transform = transform;
    }

    public void SlerpObject()
    {
        slerpTime += SettingsManager.Instance.GetTime();
        float progress = SettingsManager.Instance.GetInterpolationAlpha(slerpTime, SLERP_DURATION);
        transform.Slerp(MyMathsLibrary.VectorDegreeValuesToRadians(slerpStart), MyMathsLibrary.VectorDegreeValuesToRadians(slerpTarget), progress);

        if (progress == 1)
        {
            isSlerping = false;
            slerpTime = 0.0f;
        }
    }

    // QUATERNION NOT SELECTED FREEZES ROTATION
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
