using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;

public class Pedestal : MonoBehaviour
{
    private BoundingCapsule myCapsule;
    private const float ITEM_DISTANCE = 1f;

    private void Awake()
    {
        if(TryGetComponent(out CapsuleCollider unityCapsule))
        {
            Debug.Log("Capsule created");
            myCapsule = new BoundingCapsule(new MyVector3(transform.position), unityCapsule.radius, unityCapsule.height);
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
}
