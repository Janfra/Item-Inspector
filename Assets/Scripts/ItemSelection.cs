using MyMathsComponents;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    Pedestal pedestal;

    [SerializeField]
    private TextMeshProUGUI itemSelectedUI;
    [SerializeField]
    private TextMeshProUGUI objectHoveredUI;
    [SerializeField]
    private Button deselectButton;

    private Item selectedItem;
    private MyVector3 initialPosition;

    private void Awake()
    {
        if (TryGetComponent(out Camera Camera))
        {
            cam = Camera;
        }

        if(objectHoveredUI == null)
        {
            Debug.LogError("No text set to the hovered");
        }

        if(pedestal == null)
        {
            Debug.LogError("No pedestal set to selection");
        }

        if(deselectButton == null)
        {
            Debug.LogError("No deselect button on item selection");
        }
    }

    private void Update()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit Hit, Mathf.Infinity))
        {
            SetHoveredName(Hit.collider.name);
            if (Input.GetMouseButtonDown(0))
            {
                if (Hit.collider.TryGetComponent(out Item Item))
                {
                    if(Item != selectedItem)
                    {
                        SetSelectedItem(Item);
                    }
                }
            }
        }
        else
        {
            SetHoveredName("");
        }
    }

    private void MoveToPedestal()
    {
        Debug.Log("Selected pedestal to move item to");
        selectedItem.MoveToPosition(pedestal.GetPlacingPosition());
    }

    private void SetSelectedItem(Item Item)
    {
        Debug.Log("Selected item");
        selectedItem = Item;
        SetItemName(Item.name);
        initialPosition = Item.myTransform.Position;
        MoveToPedestal();
    }

    private void SetHoveredName(string Name)
    {
        objectHoveredUI.text = Name;
    }

    private void SetItemName(string Name)
    {
        itemSelectedUI.text = Name;
    }

    public void DeselectItem()
    {
        selectedItem.MoveToPosition(initialPosition);
        selectedItem = null;
        SetItemName("");
    }
}
