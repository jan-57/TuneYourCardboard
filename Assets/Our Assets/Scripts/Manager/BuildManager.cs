using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private LayerMask placeableLayer;
    public bool BuildModeActivated;

    private RaycastHit hit;
    private GridHandler grid;
    private void Update()
    {
        if (BuildModeActivated)
        {
            UpdateHitFromScreenRay();
            if(hit.collider != null)
                UpdateGridVisualizer();
        }
    }

    private void UpdateHitFromScreenRay()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 100, placeableLayer);
      
    }

    private void UpdateGridVisualizer()
    {
     //   currentGrid = hit.collider.GetComponent<Grid>();
     //   currentGrid.LocalToCell(hit.point);
        
    }

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

           // Gizmos.DrawCube(currentGrid.LocalToCell(hit.point)+currentGrid.cellSize/2, currentGrid.cellSize);
            

        }

    }
    #endregion
}
