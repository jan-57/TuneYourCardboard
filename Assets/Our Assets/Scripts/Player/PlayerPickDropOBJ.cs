using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickDropOBJ : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform grabPointTrasnform; 
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float hitDistance = 2.5f;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit,
                    hitDistance))
            {
                if (hit.transform.TryGetComponent(out PickUpOBJ pickUpObj))
                {
                    pickUpObj.Grab(grabPointTrasnform);
                    Debug.Log(hit.transform.name);
                }
            }
        }
        
    }
}
