using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
	public int width, height;
	Grid placementGrid;

	private Dictionary<Vector3Int, StructureModel> tempRoadObjects = new Dictionary<Vector3Int, StructureModel>();
	private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();		// structure that already been placed on the map

	private void Start()
	{
		placementGrid = new Grid(width, height);
	}

	internal CellType[] GetNeighbourTypesFor(Vector3Int position)		// get surround pos 
	{
		return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);	
	}

	internal void PlaceObjectOnTheMap(Vector3Int pos, GameObject prefab, CellType structure)
	{
		throw new NotImplementedException();
	}

	internal bool CheckPosInBound(Vector3Int pos)
	{
		if(pos.x >= 0 && pos.x < width && pos.z >= 0 && pos.z < height)		// Check if the pos is within Grid
		{
			return true;
		}
		return false;
	}

	internal bool CheckIfPosIsFree(Vector3Int pos)			
	{
		return CheckIfPosOfType(pos, CellType.Empty);       // Check if the pos is Empty
	}

	private bool CheckIfPosOfType(Vector3Int pos, CellType type)
	{
		return placementGrid[pos.x, pos.z] == type;			// check if pos have the same type as in grid
	}

	internal void PlaceTempStructure(Vector3Int pos, GameObject structurePrefab, CellType type)
	{
		placementGrid[pos.x, pos.z] = type;													// add it to the road list in Grid
		StructureModel structure = CreateNewStructureModel(pos, structurePrefab, type);		// create a game object with structureModel and a structure VFX
		tempRoadObjects.Add(pos, structure);
	}

	internal List<Vector3Int> GetNeighbourTypeFor(Vector3Int pos, CellType type)		// get the surround pos of type
	{
		var neighbourVerticies = placementGrid.GetAdjacentCellsOfType(pos.x, pos.z, type);	// get the surround pos from grid with the same type
		List<Vector3Int> neighbours = new List<Vector3Int>();
		foreach (var point in neighbourVerticies)
		{
			neighbours.Add(new Vector3Int(point.X, 0, point.Y));
		}
		return neighbours;
	}

	private StructureModel CreateNewStructureModel(Vector3Int pos, GameObject structurePrefab, CellType type)
	{
		GameObject structure = new GameObject(type.ToString());				// create new gameobject named type
		structure.transform.SetParent(transform);
		structure.transform.localPosition = pos;
		var structureModel = structure.AddComponent<StructureModel>();		// add Structure model script to it
		structureModel.CreateModel(structurePrefab);						// create the structure prefab
		return structureModel;
	}
	internal List<Vector3Int> GetPathBetween(Vector3Int startPos, Vector3Int endPos)
	{
		var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPos.x, startPos.z), new Point(endPos.x, endPos.z));		// list of point toward the end pos
		List<Vector3Int> path = new List<Vector3Int>();
		foreach (Point point in resultPath)
		{
			path.Add(new Vector3Int(point.X, 0, point.Y));		// add point to Vector3 list
		}
		return path;
	}

	internal void RemoveAllTempStructures()
	{
		foreach (var structure in tempRoadObjects.Values)	// loop through all structure model
		{
			var pos = Vector3Int.RoundToInt(structure.transform.position);
			placementGrid[pos.x, pos.z] = CellType.Empty;           // reset the type of this grid
			Destroy(structure.gameObject);
		}

		tempRoadObjects.Clear();
	}

	internal void AddTermpStructureToStructureDictionary()
	{
		foreach (var structure in tempRoadObjects)
		{
			structureDictionary.Add(structure.Key, structure.Value);
		}

		tempRoadObjects.Clear();
	}

	public void ModifyStructureModel(Vector3Int pos, GameObject newModel, Quaternion rotation)		// replace the old structure to a new one
	{
		if (tempRoadObjects.ContainsKey(pos))
			tempRoadObjects[pos].SwapModel(newModel, rotation);          // get the structure model base on pos and sawp the model
		else if (structureDictionary.ContainsKey(pos))
			structureDictionary[pos].SwapModel(newModel, rotation);          // get the structure model base on pos and sawp the model
	}
}
