using UnityEngine;

namespace MyMathsComponents
{
    [System.Serializable]
    public class MyVector4
    {
        [SerializeField]
        public float x, y, z;
        public float w = 1;

        #region Constants

        public const float DOT_SAME_DIRECTION = 1;
        public const float DOT_OPPOSITE_DIRECTION = -1;

        #endregion

        #region Static Shorthands / Global Basis Vectors

        public static MyVector4 zero { get { return new MyVector4(); } }
        public static MyVector4 Zero => zero;

        private static MyVector4 right { get { return new MyVector4(1, 0, 0); } }
        public static MyVector4 Right => right;

        private static MyVector4 up { get { return new MyVector4(0, 1, 0); } }
        public static MyVector4 Up => up;

        private static MyVector4 forward { get { return new MyVector4(0, 0, 1); } }
        public static MyVector4 Forward => forward;

        #endregion

        #region Constructors

        public MyVector4()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public MyVector4(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }
        public MyVector4(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public MyVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public MyVector4(MyVector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }
        public MyVector4(Vector3 unityVector3)
        {
            x = unityVector3.x;
            y = unityVector3.y;
            z = unityVector3.z;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds two vectors values and returns a new vector with the result.
        /// </summary>
        /// <returns>New vector with values added.</returns>
        public static MyVector4 AddVectors(MyVector4 a, MyVector4 b)
        {
            MyVector4 rv = new MyVector4();

            rv.x = a.x + b.x;
            rv.y = a.y + b.y;
            rv.z = a.z + b.z;

            return rv;
        }

        /// <summary>
        /// Subtract two vectors values and returns a new vector with the result.
        /// </summary>
        /// <returns>New vector with values subtracted.</returns>
        public static MyVector4 SubtractVectors(MyVector4 a, MyVector4 b)
        {
            MyVector4 rv = new MyVector4();

            rv.x = a.x - b.x;
            rv.y = a.y - b.y;
            rv.z = a.z - b.z;

            return rv;
        }

        /// <summary>
        /// Multiplies all values in vector by the scalar.
        /// </summary>
        /// <returns>New vector with values multiplied by scalar.</returns>
        public static MyVector4 MultiplyByValue(MyVector4 vector, float scalarValue)
        {
            MyVector4 rv = new MyVector4();

            // Change all positions by scalar
            rv.x = vector.x * scalarValue;
            rv.y = vector.y * scalarValue;
            rv.z = vector.z * scalarValue;

            return rv;
        }

        /// <summary>
        /// Divides all values in vector by the divisor.
        /// </summary>
        /// <returns>New vector with values divided by divisor.</returns>
        public static MyVector4 DivideByValue(MyVector4 vector, float valueDivisor)
        {
            MyVector4 rv = new MyVector4();

            // Change all positions by scalar
            rv.x = vector.x / valueDivisor;
            rv.y = vector.y / valueDivisor;
            rv.z = vector.z / valueDivisor;

            return rv;
        }

        public static bool CompareVectorsLenght(MyVector4 a, MyVector4 b, bool canBeEqual = false)
        {
            // Get their lenghts without using sqrt function for performance
            if (!canBeEqual)
            {
                bool isASmaller = a.GetLenghtSq() < b.GetLenghtSq();
                return isASmaller;
            }
            else
            {
                bool isASmallerOrEqual = a.GetLenghtSq() <= b.GetLenghtSq();
                return isASmallerOrEqual;
            }
        }

        public static MyVector4 operator +(MyVector4 lhs, MyVector4 rhs)
        {
            return AddVectors(lhs, rhs);
        }

        public static MyVector4 operator -(MyVector4 lhs, MyVector4 rhs)
        {
            return SubtractVectors(lhs, rhs);
        }

        public static MyVector4 operator *(MyVector4 vector, float valueScalar)
        {
            return MultiplyByValue(vector, valueScalar);
        }

        public static MyVector4 operator *(float valueScalar, MyVector4 vector)
        {
            return MultiplyByValue(vector, valueScalar);
        }

        public static MyVector4 operator /(MyVector4 vector, float valueScalor)
        {
            return DivideByValue(vector, valueScalor);
        }

        public static MyVector4 operator /(float valueScalor, MyVector4 vector)
        {
            return DivideByValue(vector, valueScalor);
        }

        public static bool operator >(MyVector4 biggerVector, MyVector4 smallerVector)
        {
            return CompareVectorsLenght(biggerVector, smallerVector);
        }

        public static bool operator <(MyVector4 smallerVector, MyVector4 biggerVector)
        {
            return CompareVectorsLenght(smallerVector, biggerVector);
        }

        public static bool operator >=(MyVector4 biggerVector, MyVector4 smallerVector)
        {
            return CompareVectorsLenght(biggerVector, smallerVector, true);
        }

        public static bool operator <=(MyVector4 smallerVector, MyVector4 biggerVector)
        {
            return CompareVectorsLenght(smallerVector, biggerVector, true);
        }

        #endregion

        #region Unity Vector Compatibility

        /// <summary>
        /// Get a vector pointing at the target position.
        /// </summary>
        /// <param name="targetPosition">Unity vector to find direction to</param>
        /// <returns>Vector pointing at target</returns>
        public MyVector4 GetDirectionToVector(Vector3 targetPosition)
        {
            MyVector4 myVectorTargetPosition = new MyVector4(targetPosition);
            return myVectorTargetPosition - this;
        }

        /// <summary>
        /// Converts from MyVector3 to Unity vector.
        /// </summary>
        /// <returns>Unity vector with same values</returns>
        public Vector4 ConvertToUnityVector()
        {
            return new Vector4(x, y, z, w);
        }

        #endregion

        /// <summary>
        /// Gets the total lenght of the vector.
        /// </summary>
        /// <returns>Magnitude or lenght of vector.</returns>
        public float GetLenght()
        {
            float rv = 0.0f;

            rv = Mathf.Sqrt(x * x + y * y + z * z);

            return rv;
        }

        /// <summary>
        /// More efficient way of comparing lenght when not needing exact lenght.
        /// </summary>
        /// <returns>Lenght without square rooting it.</returns>
        public float GetLenghtSq()
        {
            float rv = 0.0f;

            rv = x * x + y * y + z * z;

            return rv;
        }

        /// <summary>
        /// Returns a new vector with a lenght of 1 pointing the same direction as vector normalized.
        /// </summary>
        /// <returns>A new vector with a lenght of 1.</returns>
        public MyVector4 NormalizeVector()
        {
            MyVector4 rv = new MyVector4();

            rv = DivideByValue(this, GetLenght());

            return rv;
        }

        /// <summary>
        /// Returns a value dictating whether 'a' and 'b' are pointing the same or opposite way.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <param name="isNormalized">Are vectors normalized before getting result value.</param>
        /// <returns>Value in between 1 and -1. 1: Pointing exact same direction -1: Pointing exact opposite direction</returns>
        public static float VectorDotProduct(MyVector4 a, MyVector4 b, bool isNormalized = true)
        {
            float rv = 0.0f;

            if (isNormalized)
            {
                MyVector4 normalizedA = a.NormalizeVector();
                MyVector4 normalizedB = b.NormalizeVector();

                rv = normalizedA.x * normalizedB.x + normalizedA.y * normalizedB.y + normalizedA.z * normalizedB.z;
            }
            else
            {
                rv = a.x * b.x + a.y * b.y + a.z * b.z;
            }


            return rv;
        }

        /// <summary>
        /// Get a vector pointing at the target position.
        /// </summary>
        /// <param name="targetPosition">Vector to find direction to.</param>
        /// <returns>Vector pointing at target.</returns>
        public MyVector4 GetDirectionToVector(MyVector4 targetPosition)
        {
            // Direction being pointed at needs to go first.
            return targetPosition - this;
        }

        /// <summary>
        /// Returns a value at a position in between 'a' and 'b' based on 't'. NOTE: t: 1 is at b, t: 0 is at a.
        /// </summary>
        /// <param name="a">Starting position</param>
        /// <param name="b">End position</param>
        /// <param name="t">Fraction value deciding at what point in between to return</param>
        /// <returns>A position in between 'a' and 'b'</returns>
        public static MyVector4 LinearLerp(MyVector4 a, MyVector4 b, float t)
        {
            return a * (1.0f - t) + b * t;
        }

        /// <summary>
        /// Turns class values into a string.
        /// </summary>
        /// <returns>Vector values as a string.</returns>
        public override string ToString()
        {
            string rv = "";

            rv = $"X: {x}, Y: {y}, Z: {z}, W: {w}";

            return rv;
        }
    }
}
