using MyMathsComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemRotator : MonoBehaviour
{
    [SerializeField]
    private MyVector3 eulerAngles;

    [Header("Components")]
    [SerializeField]
    private DegreeRotation degreeRotation;
    [SerializeField]
    private QuaternionRotation quaternionRotation;
    private IItemRotator currentRotator;
    private MyTransform selectedObjectTransform;

    #region Mouse Rotation

    [Header("Helper UI")]
    [SerializeField]
    private BoundingCircle cursorHelperCircle;
    [SerializeField]
    private BoundingCircle cursorHelperBackground;
    [SerializeField]
    private Slider cursorHelperSlider;

    private MyVector3 mouseInitialPos;
    private const float ROTATION_DETECTION_THRESHOLD = 0.6f;
    private const float MAX_ROTATION_SPEED = 60f;
    private const float ROTATION_SPEED_DISTANCE_SCALAR = 0.8f;

    private bool isPointSelected;

    #endregion

    private void Awake()
    {
        currentRotator = degreeRotation;

        if (cursorHelperSlider != null)
        {
            cursorHelperSlider.maxValue = MAX_ROTATION_SPEED;
            cursorHelperSlider.minValue = -MAX_ROTATION_SPEED;
        }
        else
        {
            Debug.LogError("Cursor helper slider is null in Item Rotator");
        }

        if (cursorHelperCircle == null)
        {
            Debug.LogError("Cursor helper circle is null in Item Rotator");
        }

        if (cursorHelperBackground == null)
        {
            Debug.LogError("Cursor helper background is null in Item Rotator");
        }
    }

    private void Start()
    {
        SettingsManager.OnRotationTypeUpdated += UpdateRotator;
    }

    private void OnDisable()
    {
        SettingsManager.OnRotationTypeUpdated -= UpdateRotator;
    }

    private void Update()
    {
        if (selectedObjectTransform == null) return;
        if (!quaternionRotation.IsSlerping)
        {
            SetRotationWithMouseInput();
            currentRotator.OnRotateUpdate(eulerAngles);
        }
        else
        {
            quaternionRotation.SlerpToTarget();
        }
    }

    private void UpdateRotator(SettingsManager.RotationType rotationType)
    {
        switch (rotationType)
        {
            case SettingsManager.RotationType.Degrees:
                currentRotator = degreeRotation;
                break;

            case SettingsManager.RotationType.Quaternion:
                currentRotator = quaternionRotation;
                break;

            default:
                Debug.LogError("No rotation type set in item rotator");
                break;
        }
    }

    public void SetRotationTarget(MyTransform transform)
    {
        selectedObjectTransform = transform;
        degreeRotation.SetRotationTarget(transform);
        quaternionRotation.SetRotationTarget(transform);
    }

    #region Mouse Rotation

    /// <summary>
    /// Checks to set the rotation of the object based on mouse input
    /// </summary>
    private void SetRotationWithMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isPointSelected = true;
            SetMousePosition();
        }
        else if(Input.GetMouseButton(0) && Input.GetMouseButton(1) && isPointSelected)
        {
            cursorHelperSlider.gameObject.SetActive(true);
            RotateBasedOnMouseMovementForRoll();
        }
        else if (Input.GetMouseButton(0) && isPointSelected)
        {
            cursorHelperSlider.gameObject.SetActive(false);
            RotateBasedOnMouseMovement();
        }
        else
        {
            cursorHelperCircle.gameObject.SetActive(false);
            cursorHelperBackground.gameObject.SetActive(false);
            cursorHelperSlider.gameObject.SetActive(false);
            isPointSelected = false;
        }
    }

    /// <summary>
    /// Sets the helper position
    /// </summary>
    private void SetMousePosition()
    {
        // Get mouse position
        mouseInitialPos = new MyVector3(Input.mousePosition);

        // Show initial helpers
        cursorHelperCircle.gameObject.SetActive(true);
        cursorHelperBackground.gameObject.SetActive(true);

        // Set their position and centre on the mouse
        cursorHelperBackground.transform.position = mouseInitialPos.ConvertToUnityVector();
        cursorHelperCircle.transform.position = cursorHelperBackground.transform.position;
        cursorHelperCircle.centrePoint = new(mouseInitialPos);
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

            // TO MATCH VISUAL MOVEMENT FROM MOUSE VALUES WERE MODIFIED //
        // If the mouse was moved to the right or left by at least the threshold, rotate it in that direction
        if (Mathf.Abs(rightDotProduct) > ROTATION_DETECTION_THRESHOLD)
        {
            eulerAngles.y += -rightDotProduct * (distanceMultiplier * Time.deltaTime);
        }

        // Same with up or down
        if (Mathf.Abs(upDotProduct) > ROTATION_DETECTION_THRESHOLD)
        {
            eulerAngles.x += upDotProduct * (distanceMultiplier * Time.deltaTime);
        }

            // TO MATCH REAL ROTATION VALUES //
        // If the mouse was moved to the right or left by at least the threshold, rotate it in that direction
        //if (Mathf.Abs(rightDotProduct) > ROTATION_DETECTION_THRESHOLD)
        //{
        //    eulerAngles.x += rightDotProduct * (distanceMultiplier * Time.deltaTime);
        //}
        //
        //// Same with up or down
        //if (Mathf.Abs(upDotProduct) > ROTATION_DETECTION_THRESHOLD)
        //{
        //    eulerAngles.y += upDotProduct * (distanceMultiplier * Time.deltaTime);
        //}

        MoveCursorHelperCircle(xDirectionOffset: rightDotProduct, yDirectionOffset: upDotProduct, distanceMultiplier: distanceMultiplier);
    }

    /// <summary>
    /// Rotates the object based on the movement of the cursor based on its initial position
    /// </summary>
    private void RotateBasedOnMouseMovementForRoll()
    {
        // Get the direction that the mouse was moved from the point initialled clicked
        MyVector3 mouseCurrentPosition = new MyVector3(Input.mousePosition);
        MyVector3 pointingVector = mouseInitialPos.GetDirectionToVector(mouseCurrentPosition);

        // Mouse movement direction
        float rightDotProduct = MyVector3.VectorDotProduct(pointingVector, MyVector3.Right);

        // Speed the rotation based on how far the mouse position is now from the initial position
        float distanceMultiplier = Mathf.Clamp(pointingVector.GetLenght() * ROTATION_SPEED_DISTANCE_SCALAR, 0, MAX_ROTATION_SPEED);

            // TO MATCH VISUAL MOVEMENT FROM MOUSE VALUES WERE MODIFIED //
        if(distanceMultiplier > 0)
        {
            eulerAngles.z += -rightDotProduct * (distanceMultiplier * Time.deltaTime);

            MoveCursorHelperSlider(xDirectionOffset: rightDotProduct, distanceMultiplier);
        }

        // TO MATCH REAL ROTATION VALUES //
        //  eulerAngles.z += rightDotProduct * (distanceMultiplier * Time.deltaTime);
    }

    /// <summary>
    /// Updates the position of the mouse helper to represent the speed and direction of rotation
    /// </summary>
    /// <param name="xDirectionOffset"></param>
    /// <param name="yDirectionOffset"></param>
    /// <param name="distanceMultiplier"></param>
    private void MoveCursorHelperCircle(float xDirectionOffset, float yDirectionOffset, float distanceMultiplier)
    {
        // Get direction of movement
        MyVector2 direction = new MyVector2(xDirectionOffset, yDirectionOffset);

        // Get perimeter of helper circles at direction
        MyVector2 cursorHelperPerimeter = cursorHelperCircle.GetPerimeterAtDirection(direction);
        MyVector2 cursorHelperBackgroundPerimeter = cursorHelperBackground.GetPerimeterAtDirection(direction);

        // Get the speed of rotation and use it to decide how close the helper is to the border
        float rotationSpeed = MyMathsLibrary.GetNormalized(0, MAX_ROTATION_SPEED, distanceMultiplier); ;
        MyVector2 newPosition = MyVector2.Lerp(cursorHelperPerimeter, cursorHelperBackgroundPerimeter, rotationSpeed);

        // Offset the radius to move to the perimeter and not be at the perimeter
        newPosition -= direction * cursorHelperCircle.radius;

        if (newPosition.GetLenghtSq() > 0)
        {
            cursorHelperCircle.transform.position = newPosition.ConvertToUnityVector();
        }
    }

    /// <summary>
    /// Updates the slider value to match the speed of movement
    /// </summary>
    /// <param name="distanceMultiplier"></param>
    private void MoveCursorHelperSlider(float xDirectionOffset, float distanceMultiplier)
    {
        cursorHelperSlider.value = distanceMultiplier * xDirectionOffset;
    }

    #endregion

    #region Button Functions

    public void SetItemOrientationToTop()
    {
        MyVector3 topOrientation = new MyVector3(-90, 0, 0);
        quaternionRotation.SetSlerp(topOrientation, eulerAngles);
        eulerAngles = new MyVector3(topOrientation);
    }

    public void SetItemOrientationToBottom()
    {
        MyVector3 bottomOrientation = new MyVector3(90, 0, 0);
        quaternionRotation.SetSlerp(bottomOrientation, eulerAngles);
        eulerAngles = new MyVector3(bottomOrientation);
    }

    public void SetItemOrientationToRight()
    {
        MyVector3 rightOrientation = new MyVector3(0, 90, 0);
        quaternionRotation.SetSlerp(rightOrientation, eulerAngles);
        eulerAngles = new MyVector3(rightOrientation);
    }

    public void SetItemOrientationToLeft()
    {
        MyVector3 leftOrientation = new MyVector3(0, -90, 0);
        quaternionRotation.SetSlerp(leftOrientation, eulerAngles);
        eulerAngles = new MyVector3(leftOrientation);
    }

    public void SetItemOrientationToBack()
    {
        MyVector3 backOrientation = new MyVector3(180, 0, 0);
        quaternionRotation.SetSlerp(backOrientation, eulerAngles);
        eulerAngles = new MyVector3(backOrientation);
    }

    public void SetItemOrientationToFront()
    {
        MyVector3 frontOrientation = new MyVector3(0, 0, 0);
        quaternionRotation.SetSlerp(frontOrientation, eulerAngles);
        eulerAngles = new MyVector3(frontOrientation);
    }

    #endregion
}

public interface IItemRotator
{
    void OnRotateUpdate(MyVector3 eulerAngles);
    void SetRotationTarget(MyTransform transform);
    void UpdateEulerAngles(MyVector3 eulerAngles);
    MyVector3 GetEulers();
}