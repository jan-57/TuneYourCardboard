///
/// TMP CODE
///
using UnityEngine;
using UnityEngine.InputSystem;

public class ForCamLookingAndRotation : MonoBehaviour
{
    #region variables
    //cam
    [SerializeField] float sensX;
    [SerializeField] float sensY;
    float xRotation;
    float yRotation;
    //Reference
    [SerializeField] Transform orientation;
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (!PauseManager.GameIsPaused || !PauseManager.InputIsPaused)
        {
            float mouseX = Mouse.current.delta.x.ReadValue() * Time.deltaTime * sensX;
            float mouseY = Mouse.current.delta.y.ReadValue() * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}