using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;

[System.Serializable]
public class DegreeRotation : IItemRotator
{
    #region Euler Rotation

    [SerializeField]
    private GameObject cursorMovementImage;
    [SerializeField]
    private GameObject cursorMovementImageCenter;

    [Header("Rotation degrees")]
    [SerializeField]
    float pitchRotation;
    [SerializeField]
    float yawRotation;

    private MyVector3 mouseInitialPos;
    private const float ROTATION_DETECTION_THRESHOLD = 0.7f;
    private const float MAX_ROTATION_SPEED = 2f;

    #endregion
    // NOTE: Y: Yaw, X: Pitch, Z: Roll

    [Header("Tranform")]
    [SerializeField]
    MyTransform transform;
    MyVector3 rotation = new MyVector3();

    public void OnRotateUpdate()
    {
        transform.SetRotation(MyMathsLibrary.VectorDegreeValuesToRadians(rotation));
        SetRotation();
    }
    public void SetRotationTarget(MyTransform transform)
    {
        this.transform = transform;
    }

    private void SetRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetMousePosition();
        }
        else if (Input.GetMouseButton(0))
        {
            RotateBasedOnMouseMovement();
            UpdateRotation();
        }
        else
        {
            cursorMovementImage.SetActive(false);
            cursorMovementImageCenter.SetActive(false);
        }
    }

    private void UpdateRotation()
    {
        rotation.y = yawRotation;
        rotation.x = pitchRotation;
    }

    private void SetMousePosition()
    {
        mouseInitialPos = new MyVector3(Input.mousePosition);
        cursorMovementImage.SetActive(true);
        cursorMovementImage.transform.position = mouseInitialPos.ConvertToUnityVector();
        cursorMovementImageCenter.SetActive(true);
        cursorMovementImageCenter.transform.position = mouseInitialPos.ConvertToUnityVector();
    }

    private void RotateBasedOnMouseMovement()
    {
        // Get the direction that the mouse was moved from the point initialled clicked
        MyVector3 mouseCurrentPosition = new MyVector3(Input.mousePosition);
        MyVector3 pointingVector = mouseInitialPos.GetDirectionToVector(mouseCurrentPosition);

        // Mouse movement direction
        float rightDotProduct = MyVector3.VectorDotProduct(pointingVector, MyVector3.Right);
        float upDotProduct = MyVector3.VectorDotProduct(pointingVector, MyVector3.Up);

        // Speed the rotation based on how far the mouse position is now from the initial position
        float distanceMultiplier = Mathf.Clamp(pointingVector.GetLenght() * 0.01f, 0, MAX_ROTATION_SPEED);

        // If the mouse was moved to the right or left by at least the threshold, rotate it in that direction
        if (Mathf.Abs(rightDotProduct) > ROTATION_DETECTION_THRESHOLD)
        {
            pitchRotation += rightDotProduct * distanceMultiplier;
        }

        // Same with up or down
        if (Mathf.Abs(upDotProduct) > ROTATION_DETECTION_THRESHOLD)
        {
            yawRotation += upDotProduct * distanceMultiplier;
        }

        MoveCursorHelper(xDirectionOffset: rightDotProduct, yDirectionOffset: upDotProduct, distanceMultiplier: distanceMultiplier);
    }

    private void MoveCursorHelper(float xDirectionOffset, float yDirectionOffset, float distanceMultiplier)
    {
        // Up multiplier to make it more visible, could change to instead use normalization
        distanceMultiplier += 1.7f * distanceMultiplier;

        // From the mouse position set move it based on mouse movement to show the reference used to rotate
        MyVector3 positionOffset = new MyVector3(xDirectionOffset * distanceMultiplier + mouseInitialPos.x, yDirectionOffset * distanceMultiplier + mouseInitialPos.y);
        if (positionOffset.GetLenghtSq() > 0)
        {
            cursorMovementImage.transform.position = positionOffset.ConvertToUnityVector();
        }
    }
}
