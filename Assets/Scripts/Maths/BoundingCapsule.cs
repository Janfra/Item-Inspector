using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;
using Unity.VisualScripting;
using System.Security.Cryptography;

[System.Serializable]
public class BoundingCapsule
{
    [SerializeField]
    MyVector3 centrePointA;
    [SerializeField]
    MyVector3 centrePointB;
    [SerializeField]
    float radius;

    public BoundingCapsule(MyVector3 CentrePointA, MyVector3 CentrePointB, float Radius)
    {
        this.centrePointA = CentrePointA;
        this.centrePointB = CentrePointB;
        this.radius = Radius;
    }

    public BoundingCapsule(MyVector3 CentrePoint, float Radius, float Height)
    {
        radius = Radius;
        SetHeight(CentrePoint, Height);
    }

    public MyVector3 GetBoundHighestPosition()
    {
        if(centrePointA.y > centrePointB.y)
        {
            return new MyVector3(centrePointA.x, centrePointA.y + radius, centrePointA.z);
        }
        else
        {
            return new MyVector3(centrePointB.x, centrePointB.y + radius, centrePointB.z);
        }
    }

    public bool Intersects(BoundingSphere OtherCircle)
    {
        float combinedRadiusSq = (radius - OtherCircle.radius) * (radius + OtherCircle.radius);

        return FindClosestPoint(centrePointA, centrePointB, OtherCircle.centrePoint) <= combinedRadiusSq;
    }

    public static float FindClosestPoint(MyVector3 A, MyVector3 B, MyVector3 C)
    {
        // Check for A being the closest point
        MyVector3 AB = A.GetDirectionToVector(B);
        MyVector3 AC = A.GetDirectionToVector(C);
        float DotProductToAB = MyVector3.VectorDotProduct(AC, AB);

        if(DotProductToAB < 0)
        {
            return AC.GetLenghtSq();
        }

        // Check for B being the closest point
        MyVector3 BA = B.GetDirectionToVector(A);
        MyVector3 BC = B.GetDirectionToVector(C);
        float DotProductToBA = MyVector3.VectorDotProduct(BA, BC);

        if(DotProductToBA < 0)
        {
            return BC.GetLenghtSq();
        }

        // Return the closest point to projection
        MyVector3 ABProjection = MyMathsLibrary.GetProjectionPoint(AC, AB); 
        return GetProjectSqDistance(AC, ABProjection);
    }

    private static float GetProjectSqDistance(MyVector3 AC, MyVector3 AB)
    {
        return AC.GetLenghtSq() - (MyVector3.VectorDotProduct(AC, AB)) * (MyVector3.VectorDotProduct(AC, AB)) / AB.GetLenghtSq();
    }

    public void SetDistance(float Distance, bool FromAToB = true)
    {
        if (FromAToB)
        {
            MyVector3 ABDistance = centrePointA.GetDirectionToVector(centrePointB);
            Debug.Log($"Initial distance: {ABDistance.GetLenght()}");

            centrePointB = ABDistance.NormalizeVector() * Distance;
            Debug.Log($"New distance: {centrePointB.GetLenght()}");
        }
        else
        {
            MyVector3 BADistance = centrePointB.GetDirectionToVector(centrePointA);
            Debug.Log($"Initial distance: {BADistance.GetLenght()}");

            centrePointA = BADistance.NormalizeVector() * Distance;
            Debug.Log($"New distance: {centrePointB.GetLenght()}");
        }
    }

    public void SetHeight(MyVector3 Centre, float Height)
    {
        centrePointA = Centre + (MyVector3.Up * (Height / 2));
        centrePointA.y -= radius;

        centrePointB = Centre - (MyVector3.Up * (Height / 2));
        centrePointB.y += radius;
    }

    public void OnGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centrePointA.ConvertToUnityVector(), radius);
        Gizmos.DrawSphere(centrePointB.ConvertToUnityVector(), radius);
    }
}
