using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;

public class Pedestal : MonoBehaviour, IMyColliderUpdate
{
    [Header("Config")]
    [SerializeField]
    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private MyTransform myTransform;

    private BoundingCapsule myCapsule;
    private const float ITEM_DISTANCE = 1f;

    private void Awake()
    {
        if(TryGetComponent(out CapsuleCollider unityCapsule))
        {
            Debug.Log("Capsule created");
            myCapsule = new BoundingCapsule(new MyVector3(transform.position), unityCapsule.radius, unityCapsule.height);
            capsuleCollider = unityCapsule;
            myTransform.OnTranslated += UpdateColliderCentre;
        }
    }

    public MyVector3 GetPlacingPosition()
    {
        MyVector3 pedestalTop = myCapsule.GetBoundHighestPosition();
        pedestalTop.y += ITEM_DISTANCE;
        return pedestalTop;
    }

    private void OnDrawGizmos()
    {
        if(myCapsule != null)
        {
            myCapsule.OnGizmos();
        }
    }

    public void UpdateColliderCentre(MyVector3 transformPosition)
    {
        capsuleCollider.center = transformPosition.ConvertToUnityVector();
        myCapsule.SetHeight(transformPosition, capsuleCollider.height);
    }
}
