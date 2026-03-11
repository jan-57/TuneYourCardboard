using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private LayerMask placeableLayer;
    [SerializeField] private GridHandler gridToTest;
    [SerializeField] private GameObject ObjectToTest;
    [field:SerializeField] public bool BuildModeActivated { get; private set;}

    [SerializeField] private Material holoMat;

    private RaycastHit hit;
    private GridHandler grid;
    private GameObject holoObject;
    private Vector3 placeToHideHolo = new Vector3(0, -20, 0);
    private Vector3 cellPosWorldSpace = new Vector3(0, -20, 0);
    private quaternion targetRotation;

    private InputSystem_Player inputSystem;

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
    }
    private void OnDisable()
    {
        inputSystem.Player.Interact.performed -= OnPlayerInteractPerformed;
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
        }
    }
    
    private void OnPlayerRotatePerformed()
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused) return;       
        // ToDo: code to rotate holoObject
    }
    private void OnPlayerInteractPerformed(InputAction.CallbackContext _context)
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused) return;
        grid.AddObject(ObjectToTest, grid.GetCellID_FromCellWorldPos(cellPosWorldSpace), holoObject.transform.rotation);
    }

    private void ShowHolo()
    {
        if(cellPosWorldSpace != hit.point)
        {        
            holoObject.transform.position = cellPosWorldSpace;            
        }
    }

    private void ChangeHoloObject(GameObject _newHoloObject)
    {
        Destroy(holoObject);
        holoObject = Instantiate(_newHoloObject);
        holoObject.transform.position = placeToHideHolo;
        
        Material[] _a = holoObject.GetComponent<Renderer>().materials;
        for (int i = 0; i < _a.Length; i++)
        {
            _a[i] = holoMat;
        }
        holoObject.GetComponent<Renderer>().materials = _a;
    }

    #region Helper
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
