using UnityEngine;

namespace MyMathsComponents
{
    public static class MyMathsLibrary
    {
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
    }

    public class Matrix4By4
    {
        public float[,] values;

        public static Matrix4By4 Identity { get { return new Matrix4By4(); } }

        #region Constructors

        /// <summary>
        /// Returns a matrix with identity values
        /// </summary>
        public Matrix4By4()
        {
            values = new float[4, 4];

            values[0, 0] = 1;
            values[1, 0] = 0;
            values[2, 0] = 0;
            values[3, 0] = 0;

            values[0, 1] = 0;
            values[1, 1] = 1;
            values[2, 1] = 0;
            values[3, 1] = 0;

            values[0, 2] = 0;
            values[1, 2] = 0;
            values[2, 2] = 1;
            values[3, 2] = 0;

            values[0, 3] = 0;
            values[1, 3] = 0;
            values[2, 3] = 0;
            values[3, 3] = 1;
        }

        /// <summary>
        /// Returns a matrix with identity values and last row replaced by the given parameter values
        /// </summary>
        /// <param name="worldPosition"></param>
        public Matrix4By4(MyVector3 worldPosition)
        {
            values = new float[4, 4];

            values[0, 0] = 1;
            values[1, 0] = 0;
            values[2, 0] = 0;
            values[3, 0] = 0;

            values[0, 1] = 0;
            values[1, 1] = 1;
            values[2, 1] = 0;
            values[3, 1] = 0;

            values[0, 2] = 0;
            values[1, 2] = 0;
            values[2, 2] = 1;
            values[3, 2] = 0;

            values[0, 3] = worldPosition.x;
            values[1, 3] = worldPosition.y;
            values[2, 3] = worldPosition.z;
            values[3, 3] = 1;
        }

        public Matrix4By4(MyVector3 column1, MyVector3 column2, MyVector3 column3, MyVector3 column4)
        {
            values = new float[4, 4];

            values[0, 0] = column1.x;
            values[1, 0] = column1.y;
            values[2, 0] = column1.z;
            values[3, 0] = 0;

            values[0, 1] = column2.x;
            values[1, 1] = column2.y;
            values[2, 1] = column2.z;
            values[3, 1] = 0;

            values[0, 2] = column3.x;
            values[1, 2] = column3.y;
            values[2, 2] = column3.z;
            values[3, 2] = 0;

            values[0, 3] = column4.x;
            values[1, 3] = column4.y;
            values[2, 3] = column4.z;
            values[3, 3] = 1;
        }

        public Matrix4By4(MyVector4 column1, MyVector4 column2, MyVector4 column3, MyVector4 column4)
        {
            values = new float[4, 4];

            values[0, 0] = column1.x;
            values[1, 0] = column1.y;
            values[2, 0] = column1.z;
            values[3, 0] = column1.w;

            values[0, 1] = column2.x;
            values[1, 1] = column2.y;
            values[2, 1] = column2.z;
            values[3, 1] = column2.w;

            values[0, 2] = column3.x;
            values[1, 2] = column3.y;
            values[2, 2] = column3.z;
            values[3, 2] = column3.w;

            values[0, 3] = column4.x;
            values[1, 3] = column4.y;
            values[2, 3] = column4.z;
            values[3, 3] = column4.w;
        }

        #endregion

        #region Operators

        public static MyVector4 operator *(Matrix4By4 lhs, MyVector4 rhs)
        {
            MyVector4 rv = MyVector4.zero;

            for (int rowIndex = 0; rowIndex < 4; rowIndex++)
            {
                float returnValue = 0;

                // X * X + Y * Y + Z * Z + W * 1 since last identity will always be 1 for transformation
                returnValue = lhs.values[rowIndex, 0] * rhs.x + lhs.values[rowIndex, 1] * rhs.y + lhs.values[rowIndex, 2] * rhs.z + lhs.values[rowIndex, 3] * 1;

                // Alternative:
                // MyVector4 currentRow = lhs.GetRow(rowIndex);
                // returnValue = currentRow.x * rhs.x + currentRow.y * rhs.y + currentRow.z * rhs.z + currentRow.w * 1;

                // Set the calculated return value to each position
                switch (rowIndex)
                {
                    case 0:
                        rv.x = returnValue;

                        break;
                    case 1:
                        rv.y = returnValue;

                        break;
                    case 2:
                        rv.z = returnValue;

                        break;
                    case 3:
                        rv.w = returnValue;

                        break;
                }
            }

            return rv;
        }

        public static MyVector4 operator *(MyVector4 lhs, Matrix4By4 rhs)
        {
            MyVector4 rv = MyVector4.zero;

            for (int rowIndex = 0; rowIndex < 4; rowIndex++)
            {
                float returnValue = 0;

                // X * X + Y * Y + Z * Z + W * 1 since last identity will always be 1 for transformation
                returnValue = rhs.values[rowIndex, 0] * lhs.x + rhs.values[rowIndex, 1] * lhs.y + rhs.values[rowIndex, 2] * lhs.z + rhs.values[rowIndex, 3] * 1;

                // Alternative:
                // MyVector4 currentRow = rhs.GetRow(rowIndex);
                // returnValue = currentRow.x * lhs.x + currentRow.y * lhs.y + currentRow.z * lhs.z + currentRow.w * 1;

                // Set the calculated return value to each position
                switch (rowIndex)
                {
                    case 0:
                        rv.x = returnValue;

                        break;
                    case 1:
                        rv.y = returnValue;

                        break;
                    case 2:
                        rv.z = returnValue;

                        break;
                    case 3:
                        rv.w = returnValue;

                        break;
                }
            }

            return rv;
        }

        public static MyVector4 operator *(Matrix4By4 lhs, MyVector3 rhs)
        {
            MyVector4 rv = MyVector4.zero;

            for (int rowIndex = 0; rowIndex < 4; rowIndex++)
            {
                float returnValue = 0;
                returnValue = lhs.values[rowIndex, 0] * rhs.x + lhs.values[rowIndex, 1] * rhs.y + lhs.values[rowIndex, 2] * rhs.z + lhs.values[rowIndex, 3] * 1;

                // Set the calculated return value to each position
                switch (rowIndex)
                {
                    case 0:
                        rv.x = returnValue;

                        break;
                    case 1:
                        rv.y = returnValue;

                        break;
                    case 2:
                        rv.z = returnValue;

                        break;
                    case 3:
                        rv.w = returnValue;

                        break;
                }
            }

            return rv;
        }

        public static MyVector4 operator *(MyVector3 rhs, Matrix4By4 lhs)
        {
            MyVector4 rv = MyVector4.zero;

            for (int rowIndex = 0; rowIndex < 4; rowIndex++)
            {
                float returnValue = 0;
                returnValue = lhs.values[rowIndex, 0] * rhs.x + lhs.values[rowIndex, 1] * rhs.y + lhs.values[rowIndex, 2] * rhs.z + lhs.values[rowIndex, 3] * 1;

                // Set the calculated return value to each position
                switch (rowIndex)
                {
                    case 0:
                        rv.x = returnValue;

                        break;
                    case 1:
                        rv.y = returnValue;

                        break;
                    case 2:
                        rv.z = returnValue;

                        break;
                    case 3:
                        rv.w = returnValue;

                        break;
                }
            }

            return rv;
        }

        public static Matrix4By4 operator *(Matrix4By4 lhs, Matrix4By4 rhs)
        {
            Matrix4By4 rv = new Matrix4By4();
        
            // Access all rows available on the left matrix
            for(int lhsRowIndex = 0; lhsRowIndex < 4; lhsRowIndex++)
            {
                // Access all columns available on the right matrix
                for(int rhsColumnIndex = 0; rhsColumnIndex < 4; rhsColumnIndex++)
                {
                    float returnValue = 0;

                    // Access in both directions needed (right and down) before moving to the next index to set in the resulting matrix
                    for(int directionIndex = 0; directionIndex < 4; directionIndex++)
                    {
                        // Access each value on the right matrix row
                        float lhsMatrixValue = lhs.values[lhsRowIndex, directionIndex];

                        // Access each value on the left matrix column
                        float rhsMatrixValue = rhs.values[directionIndex, rhsColumnIndex];

                        // Calculate current value of the return matrix
                        returnValue += lhsMatrixValue * rhsMatrixValue;
                    }

                    // Set the value to the current row of the left matrix and the current column of the right matrix
                    rv.values[lhsRowIndex, rhsColumnIndex] = returnValue;
                }
            }

            return rv;
        }

        #endregion

        #region Transform Helpers

        /// <summary>
        /// Sets the last column that is used for translation to the given vector3
        /// </summary>
        /// <param name="worldPosition"></param>
        public void SetWorldPositionColumn(MyVector3 worldPosition)
        {
            values[0, 3] = worldPosition.x;
            values[1, 3] = worldPosition.y;
            values[2, 3] = worldPosition.z;
        }

        /// <summary>
        /// Returns the values at the last column used for world position as a vector3
        /// </summary>
        /// <returns></returns>
        public MyVector3 GetWorldPosition()
        {
            // Each array used its respectively X, Y, Z of the last column in the matrix.
            return new MyVector3(values[0, 3], values[1, 3], values[2, 3]);
        }

        /// <summary>
        /// Inverses the values stored at the world position to turn it back to model space.
        /// </summary>
        /// <returns></returns>
        public Matrix4By4 TranslationInverse()
        {
            Matrix4By4 rv = Identity;

            MyVector3 inversedPosition = GetWorldPosition();
            inversedPosition.x = -inversedPosition.x;
            inversedPosition.y = -inversedPosition.y;
            inversedPosition.z = -inversedPosition.z;

            rv.SetWorldPositionColumn(inversedPosition);
            return rv;
        }

        /// <summary>
        /// Inverses the values used for rotation by doing matrix transpose 
        /// </summary>
        /// <returns></returns>
        public Matrix4By4 RotationInverse()
        {
            // Transposed matrix
            return new Matrix4By4(GetRow(0), GetRow(1), GetRow(2), GetRow(3));
        }

        /// <summary>
        /// Inverses the values of the identity used for scaling
        /// </summary>
        /// <returns></returns>
        public Matrix4By4 ScaleInverse()
        {
            Matrix4By4 rv = Identity;

            // Inverse current scale
            MyVector4 inversedIdentityValues = GetIdentityValues();
            inversedIdentityValues.x = 1.0f / inversedIdentityValues.x;
            inversedIdentityValues.y = 1.0f / inversedIdentityValues.y;
            inversedIdentityValues.z = 1.0f / inversedIdentityValues.z;

            // Set the inversed values to the returning matrix
            rv.SetIdentityValues(inversedIdentityValues);
            return rv;
        }

        /// <summary>
        /// Converts a quaternion to a rotation matrix
        /// </summary>
        /// <param name="quat"></param>
        /// <returns></returns>
        public static Matrix4By4 QuaternionToRotationMatrix(Quat quat)
        {
            Matrix4By4 rv = new Matrix4By4();
            MyVector3 v = quat.v;

            rv.values[0, 0] = 1 - 2 * (v.y * v.y + v.z * v.z);
            rv.values[1, 0] = 2 * (v.x * v.y - v.z * quat.w);
            rv.values[2, 0] = 2 * (v.x * v.z + v.y * quat.w);

            rv.values[0, 1] = 2 * (v.x * v.y + v.z * quat.w);
            rv.values[1, 1] = 1 - 2 * (v.x * v.x + v.z * v.z);
            rv.values[2, 1] = 2 * (v.y * v.z - v.x * quat.w);

            rv.values[0, 2] = 2 * (v.x * v.z - v.y * quat.w);
            rv.values[1, 2] = 2 * (v.y * v.z + v.x * quat.w);
            rv.values[1, 2] = 1 - 2 * (v.x * v.x + v.y * v.y);

            return rv;
        }

        #endregion

        /// <summary>
        /// Return a vector4 with all the values in a row.
        /// </summary>
        /// <param name="rowNumber">Row to return as a vector4</param>
        /// <returns>Row as a vector4</returns>
        public MyVector4 GetRow(int rowNumber)
        {
            // Value at each column at a row
            return new MyVector4(values[rowNumber, 0], values[rowNumber, 1], values[rowNumber, 2], values[rowNumber, 3]);
        }

        /// <summary>
        /// Return a vector4 with all the values in a column.
        /// </summary>
        /// <param name="columnNumber">Column to return as a vector4</param>
        /// <returns>Column as a vector4</returns>
        public MyVector4 GetColumn(int columnNumber)
        {
            // Value at each row at a column 
            return new MyVector4(values[0, columnNumber], values[1, columnNumber], values[2, columnNumber], values[3, columnNumber]);
        }

        /// <summary>
        /// Sets the identity values of the matrix to the given vector values
        /// </summary>
        /// <param name="identityValues">Values to set at identity positions</param>
        public void SetIdentityValues(MyVector3 identityValues)
        {
            SetIdentityX(identityValues.x);
            SetIdentityY(identityValues.y);
            SetIdentityZ(identityValues.z);
        }

        /// <summary>
        /// Sets the identity values of the matrix to the given vector values
        /// </summary>
        /// <param name="identityValues">Values to set at identity positions</param>
        public void SetIdentityValues(MyVector4 identityValues)
        {
            SetIdentityX(identityValues.x);
            SetIdentityY(identityValues.y);
            SetIdentityZ(identityValues.z);
            SetIdentityW(identityValues.w);
        }

        /// <summary>
        /// Returns a vector4 with all identity values in the matrix
        /// </summary>
        /// <returns>Vector4 containing all identity values</returns>
        public MyVector4 GetIdentityValues()
        {
            // Values at identity positions
            return new MyVector4(values[0, 0], values[1, 1], values[2, 2], values[3, 3]);
        }

        /// <summary>
        /// Sets the identity x
        /// </summary>
        /// <param name="x"></param>
        public void SetIdentityX(float x)
        {
            values[0, 0] = x;
        }

        /// <summary>
        /// Sets the identity y
        /// </summary>
        /// <param name="y"></param>
        public void SetIdentityY(float y)
        {
            values[1, 1] = y;
        }

        /// <summary>
        /// Sets the identity Y
        /// </summary>
        /// <param name="z"></param>
        public void SetIdentityZ(float z)
        {
            values[2, 2] = z;
        }

        /// <summary>
        /// Sets the identity W
        /// </summary>
        /// <param name="w"></param>
        public void SetIdentityW(float w)
        {
            values[3, 3] = w;
        }

        /// <summary>
        /// Returns the matrix as a readable string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            MyVector4 column1 = new MyVector4(values[0, 0], values[1, 0], values[2, 0], values[3, 0]);
            MyVector4 column2 = new MyVector4(values[0, 1], values[1, 1], values[2, 1], values[3, 1]);
            MyVector4 column3 = new MyVector4(values[0, 2], values[1, 2], values[2, 2], values[3, 2]);
            MyVector4 column4 = new MyVector4(values[0, 3], values[1, 3], values[2, 3], values[3, 3]);

            return $"Column 1: {column1}, Column 2: {column2}, Column 3: {column3}, Column 4: {column4}";
        }
    }
}