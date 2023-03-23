using MyMathsComponents;
using UnityEngine;

[System.Serializable]
public class BoundingSphere 
{
    public MyVector3 centrePoint;
    public float radius;

    public BoundingSphere(MyVector3 CentrePoint, float Radius)
    {
        this.centrePoint = CentrePoint;
        this.radius = Radius;
    }

    public bool Intersects(BoundingSphere OtherCircle)
    {
        MyVector3 VectorToOther = OtherCircle.centrePoint - centrePoint;

        float CombineRadiusSq = (OtherCircle.radius + radius);
        CombineRadiusSq *= CombineRadiusSq;

        return VectorToOther.GetLenghtSq() <= CombineRadiusSq;
    }
}
