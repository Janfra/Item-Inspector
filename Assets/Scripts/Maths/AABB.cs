using UnityEngine;
using MyMathsComponents;

public class AABB
{
    MyVector3 MinExtent;
    MyVector3 MaxExtent;
    
    public float Top
    {
        get { return MaxExtent.y; }
    }

    public float Bottom
    {
        get { return MinExtent.y; }
    }

    public float Left
    {
        get { return MaxExtent.x; }
    }

    public float Right
    {
        get { return MinExtent.x; }
    }

    public float Front
    {
        get { return MaxExtent.z; }
    }

    public float Back
    {
        get { return MinExtent.z; }
    }

    public AABB(MyVector3 Min, MyVector3 Max)
    {
        MinExtent = Min;
        MaxExtent = Max;
    }

    public static bool Intersects(AABB Box1, AABB Box2)
    {
        return !(
                Box2.Left > Box1.Right ||
                Box2.Right < Box1.Left ||
                Box2.Top < Box1.Bottom ||
                Box2.Bottom > Box1.Top ||
                Box2.Back > Box1.Front ||
                Box2.Front < Box1.Back
                );
    }

    public static bool LineIntersection(AABB Box, MyVector3 StartPoint, MyVector3 EndPoint, out MyVector3 IntersectionPoint)
    {
        float Lowest = 0.0f;
        float Highest = 1.0f;

        IntersectionPoint = MyVector3.Zero;

        if(!IntersectingAxis(MyVector3.Right, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
        {
            return false;
        }

        if (!IntersectingAxis(MyVector3.Forward, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
        {
            return false;
        }

        if (!IntersectingAxis(MyVector3.Forward, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
        {
            return false;
        }

        IntersectionPoint = MyMathsLibrary.Lerp(StartPoint, EndPoint, Lowest);
        return true;
    }

    public static bool IntersectingAxis(MyVector3 Axis, AABB Box, MyVector3 StartPoint, MyVector3 EndPoint, ref float Lowest, ref float Highest)
    {
        float Minimum = 0.0f, Maximum = 1.0f; 

        if(Axis == MyVector3.Right)
        {
            Minimum = (Box.Left - StartPoint.x) / (EndPoint.x - StartPoint.x);
            Maximum = (Box.Right - StartPoint.x) / (EndPoint.x - StartPoint.x);
        }
        else if(Axis == MyVector3.Up)
        {
            Minimum = (Box.Bottom - StartPoint.y) / (EndPoint.y - StartPoint.y);
            Maximum = (Box.Top - StartPoint.y) / (EndPoint.y - StartPoint.y);
        }
        else if(Axis == MyVector3.Forward)
        {
            Minimum = (Box.Back - StartPoint.y) / (EndPoint.y - StartPoint.y);
            Maximum = (Box.Front - StartPoint.y) / (EndPoint.y - StartPoint.y);
        }

        if(Maximum < Minimum)
        {
            MyMathsLibrary.SwapValues(ref Maximum, ref Minimum);
        }

        if(Maximum < Lowest || Minimum > Highest)
        {
            return false;
        }

        Lowest = Mathf.Max(Minimum, Lowest);
        Highest = Mathf.Min(Maximum, Highest);

        if(Lowest > Highest)
        {
            return false;
        }

        return true;
    }
}
