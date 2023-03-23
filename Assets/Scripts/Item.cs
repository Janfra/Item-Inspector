using MyMathsComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IMyColliderUpdate
{
    [SerializeField]
    MyTransform myTransform;
    [SerializeField]
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        myTransform.OnTranslated += UpdateColliderCentre;
    }

    public void MoveToPosition(MyVector3 Position)
    {
        Debug.Log(Position);
        myTransform.SetPosition(Position);
    }

    public void UpdateColliderCentre(MyVector3 transformPosition)
    {
        boxCollider.center = transformPosition.ConvertToUnityVector();
    }
}
