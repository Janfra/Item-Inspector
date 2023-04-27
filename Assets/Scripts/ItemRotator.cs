using MyMathsComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    [SerializeField]
    private MyVector3 eulerAngles;
    public MyTransform selectedObjectTransform;

    [SerializeField]
    private MyVector3 axis;

    private void Update()
    {
        if (selectedObjectTransform == null) return;

        selectedObjectTransform.SetQuatRotation(eulerAngles.x, axis);
    }
}
