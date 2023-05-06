using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;
using System;
using System.Runtime.CompilerServices;

[ExecuteInEditMode]
public class MyTransform : MonoBehaviour
{
    public event Action<MyVector3> OnTranslated;
    public event Action<MyVector3> OnRotated;
    public event Action<MyVector3> OnScaled;

    private MyVector3[] modelVertices;
    private Matrix4By4 myTransformMatrix = new Matrix4By4();
    private Matrix4By4 scaleMatrix = new Matrix4By4();
    private Matrix4By4 rotationMatrix = new Matrix4By4();
    private Matrix4By4 translateMatrix = new Matrix4By4();

    [Header("References")]
    [SerializeField]
    private MeshFilter MF;
    [SerializeField]
    private Mesh originalMesh;

    [Header("Config")]
    [SerializeField]
    private MyVector3 scale = MyVector3.One;
    public MyVector3 Scale => scale;

    [SerializeField]
    private MyVector3 rotation;
    public MyVector3 Rotation => rotation;

    [SerializeField]
    private MyVector3 translate;
    public MyVector3 Position => translate;

    private Quat slerpStart;
    private Quat slerpTarget;

    private void Start()
    {
        if ((modelVertices == null || modelVertices.Length == 0) && originalMesh != null)
        {
            GetModelVertices();
        }
        SetValues(scale, rotation, translate);
    }

    private void OnEnable()
    {
        OnTranslated?.Invoke(translate);
        Debug.Log(GetRightAxis());
    }

    private void OnValidate()
    {
        if (MF == null)
        {
            MF = GetComponent<MeshFilter>();
        }

        if (modelVertices == null || modelVertices.Length == 0 && originalMesh != null)
        {
            GetModelVertices();
        }

        if (modelVertices != null && modelVertices.Length != 0 && originalMesh != null)
        {
            SetValues(scale, GetRotationRadians(), translate);
        }
    }

    #region Constructors

    public MyTransform()
    {
        myTransformMatrix = new Matrix4By4();
        scaleMatrix = new Matrix4By4();
        rotationMatrix = new Matrix4By4();
        translateMatrix = new Matrix4By4();
    }

    public MyTransform(MeshFilter mF, Vector3 scale, Vector3 rotation, Vector3 translate)
    {
        MF = mF;
        SetScale(new MyVector3(scale));
        SetRotation(new MyVector3(rotation));
        SetPosition(new MyVector3(translate));
    }

    public MyTransform(MeshFilter mF)
    {
        MF = mF;
    }
    
    #endregion

    public MyVector3 GetRightAxis()
    {
        return MyMathsLibrary.LeftVectorCrossProduct(GetForwardAxis(), MyVector3.Up);
    }

    public MyVector3 GetForwardAxis()
    {
        return MyMathsLibrary.EulerRotationToDirection(GetRotationRadians());
    }

    public MyVector3 GetRotationRadians()
    {
        return MyMathsLibrary.VectorDegreeValuesToRadians(rotation);
    }

    public void GetModelVertices()
    {
        MF = GetComponent<MeshFilter>();

        if (MF != null && originalMesh != null)
        {
            MF.sharedMesh = Instantiate(originalMesh);
            modelVertices = new MyVector3[MF.sharedMesh.vertexCount];
            for (int i = 0; i < MF.sharedMesh.vertexCount; i++)
            {
                modelVertices[i] = new MyVector3(MF.sharedMesh.vertices[i]);
            }
        }
    }

    public MyVector3 GetVectorAtLocalSpace(MyVector3 worldPosition)
    {
        return new MyVector3(worldPosition * GetInversedMatrix());
    }

    private Matrix4By4 GetInversedMatrix()
    {
        return scaleMatrix.ScaleInverse() * (rotationMatrix.RotationInverse() * translateMatrix.TranslationInverse());
    }

    public void SetValues(Vector3 scale, Vector3 rotation, Vector3 translate)
    {
        SetScale(new MyVector3(scale));
        SetRotation(new MyVector3(rotation));
        SetPosition(new MyVector3(translate));
    }

    public void SetValues(MyVector3 scale, MyVector3 rotation, MyVector3 translate)
    {
        SetScale(scale);
        SetRotation(rotation);
        SetPosition(translate);
    }

    public void UpdateTransform()
    {
        myTransformMatrix = translateMatrix * (rotationMatrix * scaleMatrix);
        SetMeshVertices(myTransformMatrix);  
    }

    public void SetRotation(MyVector3 rotationAngles)
    {
        rotation = rotationAngles;
        OnRotated?.Invoke(rotation);

        // Pitch and roll first then yaw
        Matrix4By4 rollMatrix = new Matrix4By4
        (
            new MyVector3(Mathf.Cos(rotationAngles.z), Mathf.Sin(rotationAngles.z), 0),
            new MyVector3(-Mathf.Sin(rotationAngles.z), Mathf.Cos(rotationAngles.z), 0),
            new MyVector3(0, 0, 1),
            MyVector3.zero
        );
        Matrix4By4 pitchMatrix = new Matrix4By4
        (
            new MyVector3(1, 0, 0),
            new MyVector3(0, Mathf.Cos(rotationAngles.x), Mathf.Sin(rotationAngles.x)),
            new MyVector3(0, -Mathf.Sin(rotationAngles.x), Mathf.Cos(rotationAngles.x)),
            MyVector3.zero
        );
        Matrix4By4 yawMatrix = new Matrix4By4
        (
            new MyVector3(Mathf.Cos(rotationAngles.y), 0, -Mathf.Sin(rotationAngles.y)),
            new MyVector3(0, 1, 0),
            new MyVector3(Mathf.Sin(rotationAngles.y), 0, Mathf.Cos(rotationAngles.y)),
            MyVector3.zero
        );
        
        rotationMatrix = yawMatrix * (pitchMatrix * rollMatrix);
        UpdateTransform();
    }

    #region Quaternion Rotation

    public void SetQuatRotation(float angle, MyVector3 axis, bool useMatrix = false)
    {
        if(angle == 0)
        {
            angle = MyMathsLibrary.ZERO_IN_RADIANS;
        }
        axis = axis.NormalizeVector();

        Quat rotation = new Quat(angle, axis);
        if (useMatrix)
        {
            rotationMatrix = Matrix4By4.QuaternionToRotationMatrix(rotation);
            UpdateTransform();
            return;
        }

        SetMeshVerticesQuaternion(rotation);
    }

    public void SetQuatRotation(MyVector3 eulerAngles, bool useMatrix = false)
    {
        Quat rotation = Quat.EulerToQuaternion(eulerAngles);
        this.rotation = eulerAngles;
        OnRotated?.Invoke(this.rotation);

        if (useMatrix)
        {
            rotationMatrix = Matrix4By4.QuaternionToRotationMatrix(rotation);
            UpdateTransform();
            return;
        }

        SetMeshVerticesQuaternion(rotation);
    }

    public void SetSlerp(Quat slerpStart, Quat slerpTarget, MyVector3 eulerAngles)
    {
        this.slerpStart = slerpStart;
        this.slerpTarget = slerpTarget;

        rotation = eulerAngles;
        OnRotated?.Invoke(rotation);
    }

    public void SlerpToTarget(float t)
    {
        // If no changes are going to be made, return
        if (slerpStart == slerpTarget)
        {
            return;
        }

        // Debug.Log(t);
        rotationMatrix = Matrix4By4.QuaternionToRotationMatrix(Quat.Slerp(slerpStart, slerpTarget, t));
        UpdateTransform();
    }

    private void SetMeshVerticesQuaternion(Quat quat)
    {
        quat = quat.NormalizeQuat();
        List<Vector3> newVertices = new();
        rotationMatrix = Matrix4By4.QuaternionToRotationMatrix(quat);

        // Debug.Log(quat.GetLenght());

        foreach (MyVector3 vector in modelVertices)
        {
            Quat vectorToRotate = new Quat(vector);
            Quat rotatedQuaternion = quat * vectorToRotate * quat.GetInversedQuat();
            MyVector4 worldVector = translateMatrix * rotatedQuaternion.v;

            Vector3 vectorResult = new MyVector3(worldVector).ConvertToUnityVector();
            newVertices.Add(vectorResult);
        }

        MF.sharedMesh.vertices = newVertices.ToArray();
        RecalculateMesh();
    }

    #endregion

    public void SetScale(MyVector3 newScale)
    {
        scale = newScale;
        OnScaled?.Invoke(scale);
        scaleMatrix.SetIdentityValues(newScale);
        UpdateTransform();
    }

    public void SetPosition(MyVector3 worldPosition)
    {
        translate = worldPosition;
        OnTranslated?.Invoke(translate);
        translateMatrix.SetWorldPositionColumn(worldPosition);
        UpdateTransform();
    }

    private Matrix4By4 GetBasisVectorMatrix(MyVector3 forward)
    {
        MyVector3 right = MyMathsLibrary.LeftVectorCrossProduct(forward, MyVector3.Up);
        MyVector3 up = MyMathsLibrary.VectorCrossProduct(forward, right);
        Matrix4By4 matrix4By4 = new Matrix4By4(right, up, forward, MyVector3.Zero);

        return matrix4By4;
    }

    private void SetMeshVertices(Matrix4By4 matrix)
    {
        List<Vector3> newVertices = new();

        foreach (MyVector3 vector in modelVertices)
        {
            MyVector4 result = matrix * vector;
            Vector3 vectorResult = new MyVector3(result).ConvertToUnityVector();
            newVertices.Add(vectorResult);
        }

        MF.sharedMesh.vertices = newVertices.ToArray();
        RecalculateMesh();
    }

    private void RecalculateMesh()
    {
        MF.sharedMesh.RecalculateNormals();
        MF.sharedMesh.RecalculateTangents();
    }

    #region Testing

    private void MatrixMultiplicationTest()
    {
        MyVector3 column1 = new MyVector3(10, 20, 30);
        MyVector3 column2 = new MyVector3(10, 20, 10);
        MyVector3 column3 = new MyVector3(10, 50, 50);
        MyVector3 column4 = new MyVector3(10, 50, 50);

        Matrix4By4 testMatrix = new Matrix4By4(column1, column2, column3, column4);
        Debug.Log($"Matrix multiplication result: {myTransformMatrix * testMatrix}");
    }

    #endregion
}

public interface IMyColliderUpdate
{
    void UpdateColliderCentre(MyVector3 transformPosition);
}

#region OUTDATED

//public void Slerp(MyVector3 startAngles, MyVector3 endAngles, float t)
//{
//    startAngles = Quat.GetValidEulerAngles(startAngles);
//    endAngles = Quat.GetValidEulerAngles(endAngles);

//    Quat startRotation = GetTotalEulerRotation(startAngles);
//    Quat endRotation = GetTotalEulerRotation(endAngles);

//    rotationMatrix = Matrix4By4.QuaternionToRotationMatrix(Quat.Slerp(startRotation, endRotation, t));
//    UpdateTransform();
//}

#endregion