using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;

[System.Serializable]
public class BoundingCircle
{
    public MyVector2 centrePoint;
    public float radius;

    public BoundingCircle(MyVector2 CentrePoint, float Radius)
    {
        this.centrePoint = CentrePoint;
        this.radius = Radius;
    }

    public bool Intersects(BoundingCircle OtherCircle)
    {
        MyVector2 VectorToOther = OtherCircle.centrePoint - centrePoint;

        float CombineRadiusSq = (OtherCircle.radius + radius);
        CombineRadiusSq *= CombineRadiusSq;

        return VectorToOther.GetLenghtSq() <= CombineRadiusSq;
    }
}
