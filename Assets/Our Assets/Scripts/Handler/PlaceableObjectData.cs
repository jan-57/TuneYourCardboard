using UnityEngine;
using System.Collections.Generic;
using System;

public class PlaceableObjectData : MonoBehaviour
{
    [field:SerializeField] public int ObjID{  get; private set; }
    [field:SerializeField] public ObjectType ObjectType { get; private set; }
    [field:SerializeField] public Vector3 AdditionelCellsToOccupy {  get; private set; }
    [field:SerializeField] public PlacesObjectCanBePlace[] PlacesThisCanBePlaced { get; private set; }
}


public enum PlacesObjectCanBePlace
{
    Floor,
    Walls,
    Ceiling
}
 
public enum ObjectType
{
    Object,
    SingleWall,
    CubeWall
}