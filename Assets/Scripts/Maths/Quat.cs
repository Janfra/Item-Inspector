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

            rv.w = lhs.w * rhs.w - MyVector3.VectorDotProduct(rhs.v, lhs.v);
            rv.v = rhs.w * lhs.v + lhs.w * rhs.v + MyMathsLibrary.VectorCrossProduct(lhs.v, rhs.v);

            return rv;
        }

        #endregion

        /// <summary>
        /// Turns any values that are 0 to a positive equivalent to avoid the model disappearing
        /// </summary>
        /// <param name="eulerAngles"></param>
        /// <returns></returns>
        public static MyVector3 GetValidEulerAngles(MyVector3 eulerAngles)
        {
            MyVector3 rv = new MyVector3(eulerAngles);

            if (eulerAngles.x == 0)
            {
                rv.x = MyMathsLibrary.ZERO_IN_RADIANS;
            }

            if (eulerAngles.y == 0)
            {
                rv.y = MyMathsLibrary.ZERO_IN_RADIANS;
            }

            if (eulerAngles.z == 0)
            {
                rv.z = MyMathsLibrary.ZERO_IN_RADIANS;
            }

            return rv;
        }

        /// <summary>
        /// Returns the resulting quaternion rotation from the given euler angles
        /// </summary>
        /// <param name="eulerAngles"></param>
        /// <param name="isNormalised"></param>
        /// <returns></returns>
        public static Quat EulerToQuaternion(MyVector3 eulerAngles, bool isNormalised = true)
        {
            Quat rv = new Quat();

            eulerAngles = GetValidEulerAngles(MyMathsLibrary.VectorDegreeValuesToRadians(eulerAngles));

            float cr, cp, cy, sr, sp, sy, cpcy, spsy;

            cr = Mathf.Cos(eulerAngles.z / 2);
            cp = Mathf.Cos(eulerAngles.x / 2);
            cy = Mathf.Cos(eulerAngles.y / 2);
            sr = Mathf.Sin(eulerAngles.z / 2);
            sp = Mathf.Sin(eulerAngles.x / 2);
            sy = Mathf.Sin(eulerAngles.y / 2);

            cpcy = cp * cy;
            spsy = sp * sy;

            rv.w = cr * cpcy + sr * spsy;
            rv.v.x = sr * cpcy - cr * spsy;
            rv.v.y = cr * sp * cy + sr * cp * sy;
            rv.v.z = cr * cp * sy - sr * sp * cy;

            return rv;
        }

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
            d = d.NormalizeQuat();
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
