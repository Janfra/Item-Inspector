using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    Item selectedItem;

    private void Awake()
    {
        if (TryGetComponent(out Camera Camera))
        {
            cam = Camera;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit Hit, Mathf.Infinity))
            {
                if (Hit.collider.TryGetComponent(out Item Item))
                {
                    Debug.Log("Selected item");
                    selectedItem = Item;
                }
                else if (selectedItem != null && Hit.collider.TryGetComponent(out Pedestal pedestal))
                {
                    Debug.Log("Selected pedestal to move item to");
                    selectedItem.MoveToPosition(pedestal.GetPlacingPosition());
                    selectedItem = null;
                }
            }
        }   
    }
}
