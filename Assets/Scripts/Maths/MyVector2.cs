using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyMathsComponents
{
    public class MyVector2
    {
        [SerializeField]
        public float x, y;

        #region Constructors

        public MyVector2()
        {
            x = 0;
            y = 0;
        }
        public MyVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public MyVector2(MyVector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public MyVector2(MyVector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
        }
        public MyVector2(Vector2 unityVector)
        {
            x = unityVector.x;
            y = unityVector.y;
        }

        #endregion

        #region Static Shorthands / Global Basis Vectors

        public static MyVector2 zero { get { return new MyVector2(); } }
        public static MyVector2 Zero => zero;

        private static MyVector2 right { get { return new MyVector2(1, 0); } }
        public static MyVector2 Right => right;

        private static MyVector2 up { get { return new MyVector2(0, 1); } }
        public static MyVector2 Up => up;

        #endregion

        #region Operators

        /// <summary>
        /// Adds two vectors values and returns a new vector with the result.
        /// </summary>
        /// <returns>New vector with values added.</returns>
        public static MyVector2 AddVectors(MyVector2 a, MyVector2 b)
        {
            MyVector2 rv = new MyVector2();

            rv.x = a.x + b.x;
            rv.y = a.y + b.y;

            return rv;
        }

        /// <summary>
        /// Subtract two vectors values and returns a new vector with the result.
        /// </summary>
        /// <returns>New vector with values subtracted.</returns>
        public static MyVector2 SubtractVectors(MyVector2 a, MyVector2 b)
        {
            MyVector2 rv = new MyVector2();

            rv.x = a.x - b.x;
            rv.y = a.y - b.y;

            return rv;
        }

        /// <summary>
        /// Multiplies all values in vector by the scalar.
        /// </summary>
        /// <returns>New vector with values multiplied by scalar.</returns>
        public static MyVector2 MultiplyByValue(MyVector2 vector, float valueScalor)
        {
            // Change all positions by scalor
            vector.x = vector.x * valueScalor;
            vector.y = vector.y * valueScalor;

            return vector;
        }

        /// <summary>
        /// Divides all values in vector by the divisor.
        /// </summary>
        /// <returns>New vector with values divided by divisor.</returns>
        public static MyVector2 DivideByValue(MyVector2 vector, float valueDivisor)
        {
            MyVector2 rv = new MyVector2();

            // Change all positions by scalar
            rv.x = vector.x / valueDivisor;
            rv.y = vector.y / valueDivisor;

            return rv;
        }

        public static bool CompareVectorsLenght(MyVector2 a, MyVector2 b, bool canBeEqual = false)
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

        public static MyVector2 operator +(MyVector2 lhs, MyVector2 rhs)
        {
            return AddVectors(lhs, rhs);
        }

        public static MyVector2 operator -(MyVector2 lhs, MyVector2 rhs)
        {
            return SubtractVectors(lhs, rhs);
        }

        public static MyVector2 operator *(MyVector2 vector, float valueScalor)
        {
            return MultiplyByValue(vector, valueScalor);
        }

        public static MyVector2 operator *(float valueScalor, MyVector2 vector)
        {
            return MultiplyByValue(vector, valueScalor);
        }

        public static MyVector2 operator /(MyVector2 vector, float valueScalor)
        {
            return DivideByValue(vector, valueScalor);
        }

        public static MyVector2 operator /(float valueScalor, MyVector2 vector)
        {
            return DivideByValue(vector, valueScalor);
        }

        public static bool operator >(MyVector2 biggerVector, MyVector2 smallerVector)
        {
            return CompareVectorsLenght(biggerVector, smallerVector);
        }

        public static bool operator <(MyVector2 smallerVector, MyVector2 biggerVector)
        {
            return CompareVectorsLenght(smallerVector, biggerVector);
        }

        public static bool operator >=(MyVector2 biggerVector, MyVector2 smallerVector)
        {
            return CompareVectorsLenght(biggerVector, smallerVector, true);
        }

        public static bool operator <=(MyVector2 smallerVector, MyVector2 biggerVector)
        {
            return CompareVectorsLenght(smallerVector, biggerVector, true);
        }

        #endregion

        #region Unity Vector Compatibility

        public MyVector2 GetDirectionToVector(Vector2 targetPosition)
        {
            MyVector2 myVectorTargetPosition = new MyVector2(targetPosition);
            return myVectorTargetPosition - this;
        }
        public Vector2 ConvertToUnityVector()
        {
            return new Vector2(x, y);
        }

        #endregion

        public float GetLenght()
        {
            float rv = 0.0f;

            rv = Mathf.Sqrt(x * x + y * y);

            return rv;
        }

        /// <summary>
        /// More efficient way of comparing lenght when not needing exact lenght.
        /// </summary>
        /// <returns>Lenght without square rooting it.</returns>
        public float GetLenghtSq()
        {
            float rv = 0.0f;

            rv = x * x + y * y;

            return rv;
        }

        /// <summary>
        /// Returns a new vector with a lenght of 1 pointing the same direction as vector normalized.
        /// </summary>
        /// <returns>A new vector with a lenght of 1.</returns>
        public MyVector2 NormalizeVector()
        {
            MyVector2 rv = new MyVector2();

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
        public static float VectorDotProduct(MyVector2 a, MyVector2 b, bool isNormalized = true)
        {
            float rv = 0.0f;

            if (isNormalized)
            {
                MyVector2 normalizedA = a.NormalizeVector();
                MyVector2 normalizedB = b.NormalizeVector();

                rv = normalizedA.x * normalizedB.x + normalizedA.y * normalizedB.y;
            }
            else
            {
                rv = a.x * b.x + a.y * b.y;
            }


            return rv;
        }

        public MyVector2 GetDirectionToVector(MyVector2 targetPosition)
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
        public static MyVector2 Lerp(MyVector2 a, MyVector2 b, float t)
        {
            return a * (1.0f - t) + b * t;
        }

        public override string ToString()
        {
            string rv = "";

            rv = $"X: {x}, Y: {y}";

            return rv;
        }
    }
}