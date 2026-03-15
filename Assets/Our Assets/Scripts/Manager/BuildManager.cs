using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private LayerMask placeableLayer;
    [SerializeField] private GridHandler gridToTest;
    [SerializeField] private GameObject ObjectToTest;
    [field:SerializeField] public bool BuildModeActivated { get; private set;}

    [SerializeField] private Material holoMat_Valid, holoMat_Error;

    private RaycastHit hit;
    private GridHandler grid;
    private GameObject holoObject;
    private Vector3 placeToHideHolo = new Vector3(0, -20, 0);
    private Vector3 cellPosWorldSpace = new Vector3(0, -20, 0);
    private float targetRotation = 0;

    private InputSystem_Player inputSystem;
    #endregion

    private void Awake()
    {
        grid = gridToTest;
        cellPosWorldSpace = placeToHideHolo;
        inputSystem = new InputSystem_Player();

        ChangeHoloObject(ObjectToTest);        
    }
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Interact.performed += OnPlayerInteractPerformed;
        inputSystem.Player.Rotate.performed += OnPlayerRotatePerformed;
    }
    private void OnDisable()
    {
        inputSystem.Player.Interact.performed -= OnPlayerInteractPerformed;
        inputSystem.Player.Rotate.performed -= OnPlayerRotatePerformed;
        inputSystem.Disable();
    }

    private void Update()
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused) return;

        if (BuildModeActivated)
        {
            UpdateHitFromScreenRay();
            if (hit.collider != null)
            {
                UpdateCellPosWorldSpace();
                ShowHolo();
            }
            else
            {
                holoObject.transform.position = placeToHideHolo;
            }
        }
    }



    #region On...Performed    
    private void OnPlayerRotatePerformed(InputAction.CallbackContext _context)
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused) return;
        targetRotation += 90;
    }
    private void OnPlayerInteractPerformed(InputAction.CallbackContext _context)
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused || !CanObjectBePlacedHere()) return;
        grid.AddObject(ObjectToTest, grid.GetCellID_FromCellWorldPos(cellPosWorldSpace), holoObject.transform.rotation, -hit.normal);
    }
#endregion

    private bool CanObjectBePlacedHere()
    {
        if (cellPosWorldSpace == hit.point) return false; 
        if (hit.collider == null) return false;

        return true;
    }

    #region Holo
    private void ShowHolo()
    {
        if (CanObjectBePlacedHere()) SwitchHoloObjectMaterial(true); //Not Optimal        
        else SwitchHoloObjectMaterial(false);

        holoObject.transform.position = cellPosWorldSpace + (grid.CellSize / 2) * -hit.normal;
        holoObject.transform.rotation = Quaternion.AngleAxis(targetRotation, hit.normal) * Quaternion.FromToRotation(Vector3.up, hit.normal); // final rotation = rotate like a bottle * stick from surface                
    }

    private void ChangeHoloObject(GameObject _newHoloObject)
    {
        Destroy(holoObject);
        holoObject = Instantiate(_newHoloObject);
        holoObject.transform.position = placeToHideHolo;

        SwitchHoloObjectMaterial(true);

        foreach (Collider _collider in holoObject.GetComponents<Collider>())
        {
            _collider.enabled = false;
        }
    }
    private void SwitchHoloObjectMaterial(bool _validPlacingPos)
    {
        Material[] _a = holoObject.GetComponent<Renderer>().materials;
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = _validPlacingPos? holoMat_Valid : holoMat_Error;
        }
        holoObject.GetComponent<Renderer>().materials = _a;
    }
    #endregion

    #region Update Variables
    private void UpdateHitFromScreenRay()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 50, placeableLayer);      
    }   

    private void UpdateCellPosWorldSpace()
    {
        cellPosWorldSpace = grid.GetCellWorldPos_FromWorldPos(hit.point);
    }
    #endregion

    #region Gizmo
    [Header("Visualization")]
    [SerializeField] bool showGizmos = true;
    [SerializeField] bool onlyWhenSelected = false;
    [SerializeField] Color gizmoColor = new Color(1f, 0.92f, 0.016f, 0.35f);
    [SerializeField] float hitGizmoSize = 0.2f;

    private void OnDrawGizmos()
    {
        if (!showGizmos || onlyWhenSelected) return;
        DrawGizmo();
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos || !onlyWhenSelected) return;
        DrawGizmo();
    }

    private void DrawGizmo()
    {
        Gizmos.color = gizmoColor;

        if (hit.collider != null)
        {
            Gizmos.DrawRay(Camera.main.transform.position, hit.point- Camera.main.transform.position);
            Gizmos.DrawSphere(hit.point, hitGizmoSize);

            Gizmos.DrawCube(cellPosWorldSpace,Vector3.one* 0.2f);

        }

    }
    #endregion
}
