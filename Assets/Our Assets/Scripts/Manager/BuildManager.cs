using System.Linq;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    #region Variables
    [field:SerializeField] public bool BuildModeActivated { get; private set;}

    [Header("Settings")]
    [SerializeField] private LayerMask placeableLayer;
    [SerializeField] private Material holoMat_Valid, holoMat_Error;

    [Header("Debug")]
    [SerializeField] private GridHandler gridToTest;
    [SerializeField] private GameObject objectToPlace;

    //--private Variables--
    private Vector3 debug_CollisionPoint;
    private float targetRotation = 0;
    private Vector3 placeToHideHolo = new Vector3(0, -20, 0);
    private Vector3 cellPosWorldSpace = new Vector3(0, -20, 0);
    
    //LastFrame
    private Vector3 lastFrame_CellPosWorldSpace = new Vector3(0, -20, 0);
    private quaternion lastFrame_HoloRotation;
    private float lastFrame_TargetRotation = 0;

    //References
    private InputSystem_Player inputSystem;
    private RaycastHit hit;
    private GridHandler grid;
    private GameObject holoObject;
    #endregion

    #region Setup
    private void Awake()
    {
        if(gridToTest!=null) grid = gridToTest;
        cellPosWorldSpace = placeToHideHolo;

        lastFrame_TargetRotation = targetRotation;
        lastFrame_CellPosWorldSpace = cellPosWorldSpace;

        if (objectToPlace != null)
        {
            ChangeHoloObject(objectToPlace);
            lastFrame_HoloRotation = holoObject.transform.rotation; //Needs to be set after ChangeHoloObject gets called!
        }

        inputSystem = new InputSystem_Player();
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
    #endregion
    private void Update()
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused) return;

        if (BuildModeActivated && objectToPlace != null)
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
        {
            lastFrame_TargetRotation = targetRotation;

            if(targetRotation >= 270)            
                targetRotation = 0;            
            else            
                targetRotation += 90;                        
        }
    }
    private void OnPlayerInteractPerformed(InputAction.CallbackContext _context)
    {
        if (PauseManager.InputIsPaused || PauseManager.GameIsPaused || objectToPlace == null || !CanObjectBePlacedHere()) return;

        
        switch (objectToPlace.GetComponent<PlaceableObjectData>().ObjectType) //ToDo: See if we can use a look up table instead
        {
            case ObjectType.Object:
                Debug.Log($"cellPosWorldSpace: {cellPosWorldSpace}");
                Debug.Log($"CellIdFromCellpos: {grid.GetCellID_From_CellPos(cellPosWorldSpace)}"); 
                Debug.Log($"--CellIdFromWorldpos: {grid.GetCellID_From_WorldPos(cellPosWorldSpace)}");
                grid.AddObject(objectToPlace, grid.GetCellID_From_CellPos(cellPosWorldSpace), holoObject.transform.rotation, -hit.normal);
                break;
            case ObjectType.SingleWall:
                break;
            case ObjectType.CubeWall:
                Debug.Log(cellPosWorldSpace+"  "+ grid.GetCellID_From_WorldPos(cellPosWorldSpace));
                grid.AddBoxOfWalls(objectToPlace, grid.GetCellID_From_CellPos(cellPosWorldSpace), grid.GetCellID_From_WorldPos( cellPosWorldSpace + holoObject.transform.rotation * (objectToPlace.GetComponent<PlaceableObjectData>().AdditionelCellsToOccupy )) );
                break;
            default:
                grid.AddObject(objectToPlace, grid.GetCellID_From_CellPos(cellPosWorldSpace), holoObject.transform.rotation, -hit.normal); //a Placed thing is most likely to be an object
                break;
        }
    }
    #endregion

    #region Checks
    private bool CanObjectBePlacedHere()
    {
        if (cellPosWorldSpace == hit.point) return false; 
        if (hit.collider == null) return false;
        if (objectToPlace == null) return false;
        if (holoObject == null) return false;

        PlaceableObjectData _od = objectToPlace.GetComponent<PlaceableObjectData>();

        //Object Place Restriction
        if (!_od.PlacesThisCanBePlaced.Contains(PlacesObjectCanBePlace.Floor)        &&  hit.normal.y > 0)                                                  return false; 
        else if (!_od.PlacesThisCanBePlaced.Contains(PlacesObjectCanBePlace.Walls)   && (Mathf.Abs(hit.normal.x) > 0.2f || Mathf.Abs(hit.normal.z) > 0.2f)) return false;
        else if (!_od.PlacesThisCanBePlaced.Contains(PlacesObjectCanBePlace.Ceiling) &&  hit.normal.y < 0)                                                  return false; 

        //Collision Check
        debug_CollisionPoint = Vector3.zero;
        for (int x = 0; x <= _od.AdditionelCellsToOccupy.x; x++)
        {
            for (int y = 0; y <= _od.AdditionelCellsToOccupy.y; y++)
            {
                for (int z = 0; z <= _od.AdditionelCellsToOccupy.z; z++)
                {
                    Vector3 _posToCheck =  grid.GetFirst_Anchor().position + grid.GetCellID_From_CellPos(cellPosWorldSpace) * grid.CellSize
                                            - Vector3.one * (grid.CellSize/2 ) + 
                                            (holoObject.transform.rotation * (new Vector3(x, y, z) * grid.CellSize));

                                                             
                    if (grid.OccupyedCell_List.Contains(Vector3Int.RoundToInt(grid.GetCellID_From_CellPos(_posToCheck))))
                    {
                        debug_CollisionPoint = grid.GetCellID_From_WorldPos(_posToCheck);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private bool HasInputChanged()
    {
        if (!cellPosWorldSpace.Equals(lastFrame_CellPosWorldSpace)) return true;

        if (!holoObject.transform.rotation.Equals(lastFrame_HoloRotation)) return true;

        if (targetRotation != lastFrame_TargetRotation)
        {
            lastFrame_TargetRotation = targetRotation;
            return true; 
        }

        return false;
    }
    #endregion

    #region Holo
    private void ShowHolo()
    {
        if (objectToPlace != null && HasInputChanged())
        {
            holoObject.transform.position = cellPosWorldSpace + (grid.CellSize / 2) * -hit.normal;
            lastFrame_HoloRotation = holoObject.transform.rotation;
            holoObject.transform.rotation = Quaternion.AngleAxis(targetRotation, hit.normal) * Quaternion.FromToRotation(Vector3.up, hit.normal); // final rotation = rotate like a bottle * stick from surface                

            if (CanObjectBePlacedHere())
            {
                SwitchHoloObjectMaterial(true);        
            }
            else {
                SwitchHoloObjectMaterial(false);
            }

        }
    }

    private void ChangeHoloObject(GameObject _newHoloObject)
    {
        Destroy(holoObject);
        holoObject = Instantiate(_newHoloObject);
        holoObject.transform.position = placeToHideHolo;

        if (CanObjectBePlacedHere())        
            SwitchHoloObjectMaterial(true);
        else
            SwitchHoloObjectMaterial(false);

        foreach (Collider _collider in holoObject.GetComponents<Collider>())
        {
            _collider.enabled = false;
        }
    }
    private void SwitchHoloObjectMaterial(bool _validPlacingPos)
    {
        Material[] _mats = holoObject.GetComponent<Renderer>().materials;
        for (int i = 0; i < _mats.Length; i++)
        {
            _mats[i] = _validPlacingPos? holoMat_Valid : holoMat_Error;
        }
        holoObject.GetComponent<Renderer>().materials = _mats;
    }
    #endregion

    #region Update Variables
    public void SetBuildModeActivatedState(bool _newState) { BuildModeActivated = _newState; }
    public void SetGridToUse(GridHandler _newGrid) { grid = _newGrid; }
    public void SetObjectToPlace(GameObject _newObj) //ToDo: Switch to Lookup table (ID)
    {
        objectToPlace = _newObj;
        if(objectToPlace.GetComponent<PlaceableObjectData>() == null) { Debug.LogWarning($"Object <{objectToPlace.name}>, does not have a PlaceableObjectData component"); }
        ChangeHoloObject(objectToPlace);
    }
    private void UpdateHitFromScreenRay()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 50, placeableLayer);      
    }   

    private void UpdateCellPosWorldSpace()
    {
        lastFrame_CellPosWorldSpace = cellPosWorldSpace;
        cellPosWorldSpace = grid.GetCellWorldPos_FromWorldPos(hit.point);
    }
    #endregion

    #region Gizmos
    [Header("Visualization")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private bool onlyWhenSelected = false;
    [SerializeField] private Color gizmoColor_OccupyedCellDetected = Color.red;
    [SerializeField] private Color gizmoColor_HitMarker = new Color(1f, 0.92f, 0.016f, 0.35f);
    [SerializeField] private Color gizmoColor_SizeOfObj = new Color(1f, 0.92f, 0.016f, 0.35f);
    [SerializeField] private float hitGizmoSize = 0.2f;    

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

    #region Draw Methods
    private void DrawGizmo()
    {
        if (hit.collider != null)
        {
            DrawHitMarkerGizmo();
            DrawOccupyedCellDetectedGizmo();
            DrawCellsToOccupyGizmo();
        }
    }

    private void DrawOccupyedCellDetectedGizmo()
    {
        Gizmos.color = gizmoColor_OccupyedCellDetected;
        Gizmos.DrawSphere(debug_CollisionPoint + grid.GetFirst_Anchor().position, 0.1f);
    }
    private void DrawHitMarkerGizmo()
    {
        Gizmos.color = gizmoColor_HitMarker;

        Gizmos.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position);
        Gizmos.DrawSphere(hit.point, hitGizmoSize);

        Gizmos.DrawCube(cellPosWorldSpace, Vector3.one * 0.2f);
    }
    private void DrawCellsToOccupyGizmo()
    {
        Gizmos.color = gizmoColor_SizeOfObj;

        PlaceableObjectData _od = holoObject.GetComponent<PlaceableObjectData>(); //ToDo: optimize this
        for (int x = 0; x <= _od.AdditionelCellsToOccupy.x; x++)
        {
            for (int y = 0; y <= _od.AdditionelCellsToOccupy.y; y++)
            {
                for (int z = 0; z <= _od.AdditionelCellsToOccupy.z; z++)
                {
                    Gizmos.DrawCube(cellPosWorldSpace + holoObject.transform.rotation * (new Vector3(x, y, z) * 0.5f), Vector3.one * (grid.CellSize / 2));
                }
            }
        }
    }
    #endregion

    #endregion
}
