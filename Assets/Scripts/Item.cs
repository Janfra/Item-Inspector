using MyMathsComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IMyColliderUpdate
{
    [Header("Config")]
    [SerializeField]
    private float movingDuration = 1f;

    [Header("References")]
    [SerializeField]
    private MyTransform myTransform;
    [SerializeField]
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        myTransform.OnTranslated += UpdateColliderCentre;
    }

    public void MoveToPosition(MyVector3 Position)
    {
        StartCoroutine(StartMovingTo(Position));
    }

    private IEnumerator StartMovingTo(MyVector3 TargetPosition)
    {
        float t = 0f;
        float progress = 0f;
        MyVector3 initialPosition = myTransform.Position;

        while (progress != 1)
        {
            t += Time.deltaTime;
            progress = MyMathsLibrary.EaseInDecimal(t, Mathf.Abs(movingDuration));
            myTransform.SetPosition(MyVector3.Lerp(initialPosition, TargetPosition, progress));
            yield return null; 
        }

        yield return null; 
    }

    public void UpdateColliderCentre(MyVector3 transformPosition)
    {
        boxCollider.center = transformPosition.ConvertToUnityVector();
    }
}
