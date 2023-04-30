using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;

[System.Serializable]
public class BoundingCircle : MonoBehaviour
{
    public MyVector2 centrePoint;
    public float radius;

    private void Start()
    {
        centrePoint = new MyVector2(transform.position);
    }

    public BoundingCircle(MyVector2 CentrePoint, float Radius)
    {
        this.centrePoint = CentrePoint;
        this.radius = Radius;
    }

    public bool Intersects(BoundingCircle OtherCircle)
    {
        MyVector2 VectorToOther = OtherCircle.centrePoint - centrePoint;

        float CombineRadiusSq = OtherCircle.radius + radius;
        CombineRadiusSq *= CombineRadiusSq;

        return VectorToOther.GetLenghtSq() <= CombineRadiusSq;
    }

    public MyVector2 GetPerimeterAtDirection(MyVector2 direction)
    {
        direction = direction.NormalizeVector();
        MyVector2 perimeter = centrePoint + (direction * radius);
        return perimeter;
    }

    private void OnDrawGizmos()
    {
        if(centrePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centrePoint.ConvertToUnityVector(), radius);
        }
    }
}
