using System;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField] private float cubeSize = 1f;
    [SerializeField] private Transform first_AnchorPos, second_AnchorPos;

    [SerializeField] private List<BuildList_Objects> Objects = new List<BuildList_Objects>();

    private int xCount, yCount, zCount;
    private Vector3 min, max;
 
    private void Awake()
    {
        UpdateGridData();
        SpawnPreset();    
    }

    private void SpawnPreset()
    {
        foreach (BuildList_Objects obj in Objects)
        {
            GameObject tmp = Instantiate(obj.TMP_objectID_Replacemend, first_AnchorPos);
            tmp.transform.rotation = obj.objectRotation;
            tmp.transform.position = GetCellPos(obj.cellId);
        }
    }

    private Vector3 GetCellPos(Vector3 cellId)
    {

        Vector3 center = new Vector3(min.x + cellId.x * cubeSize + cubeSize * 0.5f,
                                      min.y + cellId.y * cubeSize + cubeSize * 0.5f,
                                      min.z + cellId.z * cubeSize + cubeSize * 0.5f);

        return center;              
    }

    private void UpdateGridData()
    {
        min = Vector3.Min(first_AnchorPos.position, second_AnchorPos.position);
        max = Vector3.Max(first_AnchorPos.position, second_AnchorPos.position);

        xCount = Mathf.FloorToInt((max.x - min.x) / cubeSize);
        yCount = Mathf.FloorToInt((max.y - min.y) / cubeSize);
        zCount = Mathf.FloorToInt((max.z - min.z) / cubeSize);
    }

    #region Gizmo
    [Header("Visualization")]
    [SerializeField] bool showGizmos = true;
    [SerializeField] bool onlyWhenSelected = false;
    [SerializeField] Color gizmoColor_Cube = new Color(1f, 0.92f, 0.016f, 0.35f);
    [SerializeField] Color gizmoColor_Dots = new Color(1f, 0.50f, 0.029f, 0.35f);

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
        DrawWiredBoxGizmo();
        DrawDotsGizmo();
    }

    private void DrawWiredBoxGizmo()
    {
        if (first_AnchorPos == null || second_AnchorPos == null)
            return;

        Vector3 center = (first_AnchorPos.position + second_AnchorPos.position) * 0.5f;
        Vector3 size = new Vector3( Mathf.Abs(first_AnchorPos.position.x - second_AnchorPos.position.x), 
                                    Mathf.Abs(first_AnchorPos.position.y - second_AnchorPos.position.y), 
                                    Mathf.Abs(first_AnchorPos.position.z - second_AnchorPos.position.z));

        Gizmos.color = gizmoColor_Cube;
        Gizmos.DrawWireCube(center, size);
    }
  
    private void DrawDotsGizmo()
    {
        if (first_AnchorPos == null || second_AnchorPos == null || cubeSize <= 0f)
            return;
        UpdateGridData();

        Gizmos.color = gizmoColor_Dots;

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    Vector3 center = new Vector3( min.x + x * cubeSize + cubeSize * 0.5f,
                                                  min.y + y * cubeSize + cubeSize * 0.5f,
                                                  min.z + z * cubeSize + cubeSize * 0.5f);

                    Gizmos.DrawSphere(center, cubeSize * 0.1f);
                }
            }
        }
    }
    #endregion

}

[Serializable]
public struct BuildList_Objects
{
    public Vector3 cellId;
    public Quaternion objectRotation;
    public int objectID;
    public GameObject TMP_objectID_Replacemend;
}