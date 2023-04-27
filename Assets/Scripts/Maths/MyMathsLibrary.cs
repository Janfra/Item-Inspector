using UnityEngine;

namespace MyMathsComponents
{
    public static class MyMathsLibrary
    {
        public const float zeroToRadians = 6.28318530718f;

        public static float Vector2ToRadians(MyVector2 vector)
        {
            float rv = 0.0f;

            rv = Mathf.Atan(vector.y / vector.x);
            // can also be:
            // rv = Mathf.Atan2(vector.y, vector.x);

            return rv;
        }

        public static MyVector2 RadiansToVector(float angle)
        {
            MyVector2 rv = new MyVector2(Mathf.Cos(angle), Mathf.Sin(angle));

            return rv;
        }

        /// <summary>
        /// Returns the euler angles as a unit-lenght vector.
        /// </summary>
        /// <param name="eulerAngles"></param>
        /// <returns></returns>
        public static MyVector3 EulerRotationToDirection(MyVector3 eulerAngles)
        {
            MyVector3 rv = new MyVector3();

            // Euler angles must be in radians 
            rv.x = Mathf.Cos(eulerAngles.y) * Mathf.Cos(eulerAngles.x);
            rv.y = Mathf.Sin(eulerAngles.x);
            rv.z = Mathf.Cos(eulerAngles.x) * Mathf.Sin(eulerAngles.y);

            // Returns unit-lenght vector 
            return rv;
        }

        /// <summary>
        /// Returns a roll (Z) euler angles as a unit-lenght direction.
        /// </summary>
        /// <param name="eulerAngle"></param>
        /// <returns></returns>
        public static MyVector3 GetRollDirection(MyVector3 eulerAngle)
        {
            MyVector3 rv = new MyVector3();

            rv.x = Mathf.Cos(eulerAngle.z);
            rv.y = Mathf.Sin(eulerAngle.z);
            rv.z = 0.0f;

            // Returns unit-lenght vector 
            return rv;
        }

        /// <summary>
        /// Returns a vector cross product as a right-handed co-ordinate system.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MyVector3 VectorCrossProduct(MyVector3 a, MyVector3 b)
        {
            MyVector3 rv = new MyVector3();

            rv.x = a.y * b.z - a.z * b.y;
            rv.y = a.z * b.x - a.x * b.z;
            rv.z = a.x * b.y - a.y * b.x;

            return rv;
        }

        /// <summary>
        /// Returns a vector cross product as a left-handed co-oordinate system for unity.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MyVector3 LeftVectorCrossProduct(MyVector3 a, MyVector3 b)
        {
            // Unity uses a left-handed co-ordinate system, so flip the cross product
            MyVector3 rv = new MyVector3();

            rv.x = b.y * a.z - b.z * a.y;
            rv.y = b.z * a.x - b.x * a.z;
            rv.z = b.x * a.y - b.y * a.x;

            return rv;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static float RadiansToDegrees(float radians)
        {
            float rv = 0.0f;

            rv = radians * (180 / Mathf.PI);

            return rv;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static float DegreesToRadians(float degrees)
        {
            float rv = 0.0f;

            rv = degrees * (Mathf.PI / 180);

            return rv;
        }

        /// <summary>
        /// Changes a vector values from degrees to radians.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 VectorDegreeValuesToRadians(Vector3 vector)
        {
            Vector3 rv = new Vector3();

            rv.x = DegreesToRadians(vector.x % 360);
            rv.y = DegreesToRadians(vector.y % 360);
            rv.z = DegreesToRadians(vector.z % 360);

            return rv;
        }

        /// <summary>
        /// Changes a vector values from degrees to radians.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static MyVector3 VectorDegreeValuesToRadians(MyVector3 vector)
        {
            MyVector3 rv = new MyVector3();

            rv.x = DegreesToRadians(vector.x % 360);
            rv.y = DegreesToRadians(vector.y % 360);
            rv.z = DegreesToRadians(vector.z % 360);

            return rv;
        }

        /// <summary>
        /// Returns projection from a vector to vector. Closest point along a vector.
        /// </summary>
        /// <param name="N"></param>
        /// <param name="vectorToProjectToV"></param>
        /// <returns></returns>
        public static MyVector3 GetProjectionPoint(MyVector3 N, MyVector3 vectorToProjectToV)
        {
            return MyVector3.VectorDotProduct(N, vectorToProjectToV, false) * vectorToProjectToV;
        }

        /// <summary>
        /// Linearly interpolates from point A to B by decimal value t.
        /// </summary>
        /// <param name="startPoint">Point A</param>
        /// <param name="endPoint">Point B</param>
        /// <param name="t">Decides where along path (AB) to return</param>
        /// <returns>Point between A and B decided by t. 0: A, 1: B</returns>
        public static float Lerp(float startPoint, float endPoint, float t)
        {
            return startPoint * (1 - t) + endPoint * t;
        }

        /// <summary>
        /// Swaps the values in between two float values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void SwapValues(ref float a, ref float b)
        {
            float temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Rotate a vertex along the axis to the given angle.
        /// </summary>
        /// <param name="Angle">Angle in radians</param>
        /// <param name="Axis"></param>
        /// <param name="Vertex"></param>
        /// <returns></returns>
        public static MyVector3 RotateVertexRodrigues(float Angle, MyVector3 Axis, MyVector3 Vertex)
        {
            MyVector3 rv = new MyVector3();

            rv = (Vertex * Mathf.Cos(Angle)) + MyVector3.VectorDotProduct(Vertex, Axis) * Axis * (1 - Mathf.Cos(Angle)) + LeftVectorCrossProduct(Axis, Vertex) * Mathf.Sin(Angle);

            return rv;
        }

        /// <summary>
        /// Returns value normalized in between the min and max values.
        /// </summary>
        /// <param name="min">Min value of the dataset</param>
        /// <param name="max">Max value of the dataset</param>
        /// <param name="value">Value to normalize</param>
        /// <returns>Value in between 0 and 1</returns>
        public static float GetNormalized(float min, float max, float value)
        {
            value = Mathf.Clamp(value, min, max);
            return (value - min) / (max - min); 
        }

        public static float SquareValue(float value)
        {
            return value * value;
        }

        #region Easing Functions

        /// Links for reference https://www.febucci.com/2018/08/easing-functions/ & http://gizma.com/easing/#cub1

        /// <summary>
        /// Flips decimal value for interpolation, going from 0 to 1, now to 1 to 0 or vice versa.
        /// </summary>
        /// <param name="t">Value being flip</param>
        /// <returns>Flipped Value</returns>
        private static float Flip(float t)
        {
            return 1 - t;
        }

        /// <summary>
        /// Returns an ease in value in between the initial value and final value given by the duration.
        /// </summary>
        /// <returns></returns>
        public static float EaseInValueWithinRange(float currentTime, float initialValue, float finalValue, float duration)
        {
            float rv;
            currentTime /= duration;

            rv = (finalValue * currentTime * currentTime * currentTime) + initialValue;
            return rv;
        }

        /// <summary>
        /// Returns a 0 to 1 value with the ease in function.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static float EaseInDecimal(float t, float duration)
        {
            t /= duration;
            float rv = t * t;
            return Mathf.Clamp01(rv);
        }

        public static float EaseOutDecimal(float t, float duration)
        {
            t = Mathf.Clamp(t, 0, duration);
            t /= duration;
            float rv = Flip(SquareValue(Flip(t)));
            return Mathf.Clamp01(rv);
        }

        public static float EaseInOutDecimal(float t, float duration)
        {
            t = Mathf.Clamp(t, 0, duration);
            float rv = Lerp(EaseInDecimal(t, duration), EaseOutDecimal(t, duration), t / duration);
            return Mathf.Clamp01(rv);
        }

        #endregion
    }

    public enum EasingFunctions
    {
        EaseIn,
        EaseOut,
        EaseInOut,
    }
}