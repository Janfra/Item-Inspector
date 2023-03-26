using UnityEngine;

namespace MyMathsComponents
{
    public class Quat
    {
        // Quat identity Multiply: 1, 0, 0, 0. Addition: 0, 0, 0, 0

        // Scalar
        public float w;
        // Vector
        public MyVector3 v;

        #region Constructors

        public Quat()
        {
            w = 0.0f;
            v = new MyVector3();
        }

        public Quat(float Angle, MyVector3 Axis)
        {
            float halfAngle = Angle / 2;
            w = Mathf.Cos(halfAngle);

            float angleSinResult = Mathf.Sin(halfAngle);
            v = Axis * angleSinResult;
        }

        public Quat(MyVector3 Position)
        {
            w = 0.0f;
            v = new MyVector3(Position);
        }

        #endregion

        #region Operators

        public static Quat operator*(Quat lhs, Quat rhs)
        {
            Quat rv = new Quat();

            rv.w = rhs.w * lhs.w - MyVector3.VectorDotProduct(rhs.v, lhs.v);
            rv.v = rhs.w * lhs.v + lhs.w * rhs.v + MyMathsLibrary.LeftVectorCrossProduct(lhs.v, rhs.v);

            return rv;
        }

        #endregion

        /// <summary>
        /// Returns the magnitude / lenght of the quaternion squared.
        /// </summary>
        /// <returns></returns>
        public float GetLenghtSq()
        {
            return w * w + v.GetLenghtSq();
        }

        /// <summary>
        /// Returns the magnitude / lenght of the quaternion.
        /// </summary>
        /// <returns></returns>
        public float GetLenght()
        {
            return Mathf.Sqrt(w * w + v.GetLenghtSq());
        }

        /// <summary>
        /// Returns an unit-lenght quaternion of this quaternion.
        /// </summary>
        /// <returns></returns>
        public Quat NormalizeQuat()
        {
            Quat rv = new Quat();
            float magnitude = GetLenght();

            rv.w = w / magnitude;
            rv.v = v / magnitude;

            return rv;
        }

        /// <summary>
        /// Returns quaternion inversed. NOTE: Only unit-lenght quat.
        /// </summary>
        /// <returns></returns>
        public Quat GetInversedQuat()
        {
            Quat rv = new Quat();

            rv.w = w;
            rv.v = -v;

            return rv;
        }

        /// <summary>
        /// Returns quaternion values as an axis angle to mantain unit-lenght quat.
        /// </summary>
        /// <returns></returns>
        public MyVector4 GetAxisAngle()
        {
            MyVector4 rv = new MyVector4();

            // Inverse cosine to get half angle back
            float halfAngle = Mathf.Acos(w);
            rv.w = halfAngle * 2;

            float angleSinResult = Mathf.Sin(halfAngle);
            rv.x = v.x / angleSinResult;
            rv.y = v.y / angleSinResult;
            rv.z = v.z / angleSinResult;

            return rv;
        }

        /// <summary>
        /// Spherical Linear Interpolation along A and B, based on 't'.
        /// </summary>
        /// <param name="q">A</param>
        /// <param name="r">B</param>
        /// <param name="t">point along A and B on decimal</param>
        /// <returns></returns>
        public static Quat Slerp(Quat q, Quat r, float t)
        {
            t = Mathf.Clamp01(t);

            Quat d = r * q.GetInversedQuat();
            MyVector4 axisAngle = d.GetAxisAngle();
            Quat dT = new Quat(axisAngle.w * t, new MyVector3(axisAngle));

            return dT * q;
        }

        #region Test

        public static MyVector3 RotateVector(MyVector3 vector, float angle, MyVector3 direction)
        {
            MyVector3 rotatedVector = new MyVector3();
            Quat q = new Quat(angle, direction);
            Quat K = new Quat(vector);

            rotatedVector = (q * K * q.GetInversedQuat()).v;
            return rotatedVector;
        }

        #endregion

        public override string ToString()
        {
            return $"Quat - w: {w} {v}";
        }
    }
}
