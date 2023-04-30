using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathsComponents;
using UnityEngine.EventSystems;

[System.Serializable]
public class DegreeRotation : IItemRotator
{
    #region Euler Rotation

    [Header("Helper UI")]
    [SerializeField]
    private BoundingCircle cursorHelper;
    [SerializeField]
    private BoundingCircle cursorHelperBackground;

    [Header("Rotation degrees")]
    [SerializeField]
    float pitchRotation;
    [SerializeField]
    float yawRotation;

    private MyVector3 mouseInitialPos;
    private const float ROTATION_DETECTION_THRESHOLD = 0.6f;
    private const float MAX_ROTATION_SPEED = 2f;
    private const float ROTATION_SPEED_DISTANCE_SCALAR = 0.015f;

    private bool isPointSelected;

    #endregion
    // NOTE: Y: Yaw, X: Pitch, Z: Roll

    [Header("Tranform")]
    [SerializeField]
    MyTransform transform;
    MyVector3 rotation = new MyVector3();

    /// <summary>
    /// Sets the rotation of the object as well as updating to match input
    /// </summary>
    public void OnRotateUpdate()
    {
        transform.SetRotation(MyMathsLibrary.VectorDegreeValuesToRadians(rotation));
        SetRotation();
    }

    /// <summary>
    /// Sets the target to rotate
    /// </summary>
    /// <param name="transform"></param>
    public void SetRotationTarget(MyTransform transform)
    {
        this.transform = transform;
    }

    /// <summary>
    /// Checks to set the rotation of the object based on mouse input
    /// </summary>
    private void SetRotation()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isPointSelected = true;
            SetMousePosition();
        }
        else if (Input.GetMouseButton(0) && isPointSelected)
        {
            RotateBasedOnMouseMovement();
            UpdateRotationEuler();
        }
        else
        {
            cursorHelper.gameObject.SetActive(false);
            cursorHelperBackground.gameObject.SetActive(false);
            isPointSelected = false;
        }
    }

    /// <summary>
    /// Updates the euler angles for the rotation
    /// </summary>
    private void UpdateRotationEuler()
    {
        rotation.y = yawRotation;
        rotation.x = pitchRotation;
    }

    /// <summary>
    /// Sets the helper position
    /// </summary>
    private void SetMousePosition()
    {
        mouseInitialPos = new MyVector3(Input.mousePosition);
        cursorHelper.gameObject.SetActive(true);
        cursorHelper.transform.position = mouseInitialPos.ConvertToUnityVector();
        cursorHelper.centrePoint = new(mouseInitialPos);

        cursorHelperBackground.gameObject.SetActive(true);
        cursorHelperBackground.transform.position = mouseInitialPos.ConvertToUnityVector();
        cursorHelperBackground.centrePoint = new(mouseInitialPos);
    }

    /// <summary>
    /// Rotates the object based on the movement of the cursor based on its initial position
    /// </summary>
    private void RotateBasedOnMouseMovement()
    {
        // Get the direction that the mouse was moved from the point initialled clicked
        MyVector3 mouseCurrentPosition = new MyVector3(Input.mousePosition);
        MyVector3 pointingVector = mouseInitialPos.GetDirectionToVector(mouseCurrentPosition);

        // Mouse movement direction
        float rightDotProduct = MyVector3.VectorDotProduct(pointingVector, MyVector3.Right);
        float upDotProduct = MyVector3.VectorDotProduct(pointingVector, MyVector3.Up);

        // Speed the rotation based on how far the mouse position is now from the initial position
        float distanceMultiplier = Mathf.Clamp(pointingVector.GetLenght() * ROTATION_SPEED_DISTANCE_SCALAR, 0, MAX_ROTATION_SPEED);

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

    /// <summary>
    /// Updates the position of the mouse helper to represent the speed and direction of rotation
    /// </summary>
    /// <param name="xDirectionOffset"></param>
    /// <param name="yDirectionOffset"></param>
    /// <param name="distanceMultiplier"></param>
    private void MoveCursorHelper(float xDirectionOffset, float yDirectionOffset, float distanceMultiplier)
    {
        // Get direction of movement
        MyVector2 direction = new MyVector2(xDirectionOffset, yDirectionOffset);

        // Get perimeter of helper circles at direction
        MyVector2 cursorHelperPerimeter = cursorHelper.GetPerimeterAtDirection(direction);
        MyVector2 cursorHelperBackgroundPerimeter = cursorHelperBackground.GetPerimeterAtDirection(direction);

        // Get the speed of rotation and use it to decide how close the helper is to the border
        float rotationSpeed = MyMathsLibrary.GetNormalized(0, MAX_ROTATION_SPEED, distanceMultiplier);;
        MyVector2 newPosition = MyVector2.Lerp(cursorHelperPerimeter, cursorHelperBackgroundPerimeter, rotationSpeed);

        // Offset the radius to move to the perimeter and not be at the perimeter
        newPosition -= direction * cursorHelper.radius;

        if (newPosition.GetLenghtSq() > 0)
        {
            cursorHelper.transform.position = newPosition.ConvertToUnityVector();
        }
    }
}
