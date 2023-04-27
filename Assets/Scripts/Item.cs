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
    public MyTransform myTransform;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private static SettingsManager settingsManager;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        myTransform.OnTranslated += UpdateColliderCentre;
        settingsManager = SettingsManager.Instance;
    }

    public void MoveToPosition(MyVector3 targetPosition)
    {
        StartCoroutine(StartMovingTo(targetPosition));
    }

    private IEnumerator StartMovingTo(MyVector3 targetPosition)
    {
        float t = 0f;
        float alphaProgress = 0f;
        MyVector3 initialPosition = myTransform.Position;

        while (alphaProgress != 1)
        {
            t += settingsManager.GetTime();
            alphaProgress = settingsManager.GetInterpolationAlpha(t, Mathf.Abs(movingDuration));
            myTransform.SetPosition(MyVector3.Lerp(initialPosition, targetPosition, alphaProgress));
            yield return null; 
        }

        yield return null; 
    }

    public void UpdateColliderCentre(MyVector3 transformPosition)
    {
        boxCollider.center = transformPosition.ConvertToUnityVector();
    }
}
