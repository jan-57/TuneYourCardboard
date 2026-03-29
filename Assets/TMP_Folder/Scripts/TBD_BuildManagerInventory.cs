using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class TBD_BuildManagerInventory : MonoBehaviour
{
    public GameObject[] items;
    public int currentSlot = 0;
    public BuildManager bm;

    private InputSystem_Player playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new InputSystem_Player();
    }
    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Next.performed += OnNext;
        playerInput.Player.Previous.performed += OnLast;
    }
    private void OnDisable()
    {
        playerInput.Player.Next.performed -= OnNext;
        playerInput.Player.Previous.performed -= OnLast;
        playerInput.Disable();
    }
    private void OnNext(InputAction.CallbackContext context)
    {
        if (currentSlot < items.Length - 1) 
        {
            currentSlot++;

            bm.SetObjectToPlace(items[currentSlot]);
        }
    }
    private void OnLast(InputAction.CallbackContext context)
    {
        if (currentSlot > 0)
        {
            currentSlot--;

            bm.SetObjectToPlace(items[currentSlot]);
        }
    }
}
