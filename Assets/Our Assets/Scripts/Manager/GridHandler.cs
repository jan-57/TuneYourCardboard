using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    #region Variables
    [field:SerializeField] public float CellSize { get; private set; } = 0.5f;
    [SerializeField] private Transform first_AnchorPos, second_AnchorPos;
    [SerializeField] private Vector3 _startCellID, _endCellID;

    [SerializeField] private List<BuildList_Objects> objects_List = new List<BuildList_Objects>();
    [SerializeField] private List<BuildList_Walls> walls_List = new List<BuildList_Walls>();

    private int xCount, yCount, zCount;
    private Vector3 minGrid, maxGrid;
    private float wallThickness = 0.01f;
#endregion

    private void Awake()
    {
        UpdateGridData();
        SpawnPreset();
    }

    #region Spawn/ Add/ Create
    #region Add 
    //Add new object/wall
    public void AddObject(GameObject _objREPLACEWITHID, Vector3 _cellID, Quaternion _rotation, Vector3 _directionToOffsetToo, int _objID = 0) //remove = 0
    {
        BuildList_Objects bl = new BuildList_Objects();
        bl.cellID = _cellID;
        bl.directionalOffset = _directionToOffsetToo;
        bl.TMP_objectID_Replacemend = _objREPLACEWITHID;        
        Debug.Log("bl" + bl);
        Debug.Log("repl" + bl.TMP_objectID_Replacemend);
        Debug.Log("Get" + bl.TMP_objectID_Replacemend.GetComponent<PlaceableObjectData>());
        Debug.Log("Cells" + bl.TMP_objectID_Replacemend.GetComponent<PlaceableObjectData>().AdditionelCellsToOccupy);
        bl.additionelCellsToOccupy = bl.TMP_objectID_Replacemend.GetComponent<PlaceableObjectData>().AdditionelCellsToOccupy;
        Debug.Log(bl.cellID + "  " + _cellID);
        bl.objectRotation = _rotation;
        bl.objectID = _objID;
        objects_List.Add(bl);        
        CreateObject(bl);
    }
    public void AddBoxOfWalls(GameObject _wallREPLACEWITHID, Vector3 _cellId_Start, Vector3 _cellId_End)
    {
        CreateBoxOfWalls(_cellId_Start, _cellId_End, _wallREPLACEWITHID);
    }

    /// <summary>
    /// Use AddBoxOfWalls() if plausible, its simpler
    /// </summary>
    /// <param name="_wallREPLACEWITHID">Wall type</param>
    /// <param name="_cellId_Start">pos not cellID, Center of finished wall</param>
    /// <param name="_cellId_End"></param>
    public void AddSingleWall(GameObject _wallREPLACEWITHID, Vector3 _pos, Vector3 _scale)
    {
        CreateWall(_pos, _scale, _wallREPLACEWITHID);
    }
    #endregion

    #region Spawn
    //Initiate Prefab
    private void SpawnPreset()
    {
        foreach (BuildList_Walls _wall in walls_List)
        {
            CreateBoxOfWalls(_wall.cellID_Start, _wall.cellID_End, _wall.TMP_objectID_Replacemend);                  
        }

        foreach (BuildList_Objects _obj in objects_List)
        {
            CreateObject(_obj);         
        }
    }
    #endregion

    #region Create
    //Instantiate object/walls
    private void CreateObject(BuildList_Objects _objInfo)
    {
        GameObject tmp = Instantiate(_objInfo.TMP_objectID_Replacemend, first_AnchorPos);
        tmp.transform.localPosition = _objInfo.cellID + _objInfo.directionalOffset * (CellSize/2);
        tmp.transform.localRotation = _objInfo.objectRotation;
    }
    private void CreateBoxOfWalls(Vector3 _cellA, Vector3 _cellB, GameObject _wallPrefab)
    {
        Vector3 _min = Vector3.Min(_cellA, _cellB);
        Vector3 _max = Vector3.Max(_cellA, _cellB);

        Debug.Log($"Min: {_min.x} {_min.y} {_min.z}, Max: {_max.x} {_max.y} {_max.z}");

        Bounds _bounds = new Bounds();

        _bounds.SetMinMax(_min * CellSize, _max * CellSize + Vector3.one*CellSize);
        Debug.Log(_bounds.center.x+" "+ _bounds.center.y+ " "+ _bounds.center.z);

        // Floor
        CreateWall(new Vector3(_bounds.center.x, _bounds.min.y, _bounds.center.z), 
                   new Vector3(_bounds.size.x, wallThickness, _bounds.size.z), _wallPrefab); 
        // Roof
        CreateWall(new Vector3(_bounds.center.x, _bounds.max.y, _bounds.center.z), 
                   new Vector3(_bounds.size.x + wallThickness, wallThickness, _bounds.size.z + wallThickness), _wallPrefab); 
        // Left
        CreateWall(new Vector3(_bounds.min.x, _bounds.center.y, _bounds.center.z), 
                   new Vector3(wallThickness, _bounds.size.y + wallThickness, _bounds.size.z + wallThickness), _wallPrefab); 
        // Right
        CreateWall(new Vector3(_bounds.max.x, _bounds.center.y, _bounds.center.z), 
                   new Vector3(wallThickness, _bounds.size.y + wallThickness, _bounds.size.z + wallThickness), _wallPrefab); 
        // Front
        CreateWall(new Vector3(_bounds.center.x, _bounds.center.y, _bounds.min.z), 
                   new Vector3(_bounds.size.x + wallThickness, _bounds.size.y + wallThickness, wallThickness), _wallPrefab); 
        // Back
        CreateWall(new Vector3(_bounds.center.x, _bounds.center.y, _bounds.max.z), 
                   new Vector3(_bounds.size.x + wallThickness, _bounds.size.y + wallThickness, wallThickness), _wallPrefab);         
    }
    private void CreateWall(Vector3 _pos, Vector3 _scale, GameObject _wallPrefab)
    {
        var wall = Instantiate(_wallPrefab, first_AnchorPos);
        wall.transform.localPosition = _pos;
        wall.transform.localScale = _scale;
    }
    #endregion
    #endregion

    #region Helper Methods
    public Vector3 GetCellWorldPos_FromID(Vector3 _cellID)
    {
        Vector3 center = new Vector3(minGrid.x + _cellID.x * CellSize + CellSize * 0.5f,
                                      minGrid.y + _cellID.y * CellSize + CellSize * 0.5f,
                                      minGrid.z + _cellID.z * CellSize + CellSize * 0.5f);
        Debug.Log(center);
        return center;              
    }
    public Vector3 GetCellWorldPos_FromWorldPos(Vector3 _posToCheck)
    {
        if (_posToCheck.x < minGrid.x || _posToCheck.y < minGrid.y || _posToCheck.z < minGrid.z ||
            _posToCheck.x > maxGrid.x || _posToCheck.y > maxGrid.y || _posToCheck.z > maxGrid.z)
                return _posToCheck;

        return new Vector3(Mathf.Floor((_posToCheck.x - minGrid.x) / CellSize) * CellSize + minGrid.x + CellSize * 0.5f,
                           Mathf.Floor((_posToCheck.y - minGrid.y) / CellSize) * CellSize + minGrid.y + CellSize * 0.5f,
                           Mathf.Floor((_posToCheck.z - minGrid.z) / CellSize) * CellSize + minGrid.z + CellSize * 0.5f);        
    }
    public Vector3 GetCellID_FromCellWorldPos(Vector3 _cellWorldPosToCheck)
    {
        return first_AnchorPos.InverseTransformPoint(_cellWorldPosToCheck);
    }

    private void UpdateGridData()
    {
        minGrid = Vector3.Min(first_AnchorPos.position, second_AnchorPos.position);
        maxGrid = Vector3.Max(first_AnchorPos.position, second_AnchorPos.position);

        xCount = Mathf.FloorToInt((maxGrid.x - minGrid.x) / CellSize);
        yCount = Mathf.FloorToInt((maxGrid.y - minGrid.y) / CellSize);
        zCount = Mathf.FloorToInt((maxGrid.z - minGrid.z) / CellSize);
    }
    public Transform GetFirst_Anchor()
    {
        return first_AnchorPos;
    }
    #endregion

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
        if (first_AnchorPos == null || second_AnchorPos == null || CellSize <= 0f)
            return;
        UpdateGridData();

        Gizmos.color = gizmoColor_Dots;

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    Vector3 center = new Vector3(minGrid.x + x * CellSize + CellSize * 0.5f,
                                                  minGrid.y + y * CellSize + CellSize * 0.5f,
                                                  minGrid.z + z * CellSize + CellSize * 0.5f);

                    Gizmos.DrawSphere(center, CellSize * 0.1f);
                }
            }
        }
    }
    #endregion
}

#region structs
[Serializable]
public struct BuildList_Objects
{
    public Vector3 cellID;
    public Vector3 directionalOffset;
    public Vector3 additionelCellsToOccupy;
    public Quaternion objectRotation;
    public int objectID;
    public GameObject TMP_objectID_Replacemend;
}
[Serializable]
public struct BuildList_Walls
{
    public Vector3 cellID_Start, cellID_End;
    public Vector3 directionalOffset;
    public Quaternion objectRotation;
    public int objectID;
    public GameObject TMP_objectID_Replacemend;
}
#endregion