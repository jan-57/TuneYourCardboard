using UnityEngine;

public class PlayerPickDropOBJ : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float hitDistance = 2.5f;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit,
                    hitDistance))
            {
                if (hit.transform.TryGetComponent(out PickUpOBJ pickUpObj))
                {
                    Debug.Log("Can grab object");
                }
            }
        }
        
    }
}
