using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
	public GameObject deadEnd, roadStraight, corner, threeWay, fourWay;

	public void FixRoadAtPos(PlacementManager placementManager, Vector3Int tempPos)	// replace the correct model base on it surrouding
	{
		CellType[] result = placementManager.GetNeighbourTypesFor(tempPos);      // base on grid manager the check should follow Left -> Up -> Right -> Down
		int roadCount = 0;
		roadCount = result.Where(x => x == CellType.Road).Count();              // see how many of type Road

		if(roadCount == 0 || roadCount == 1)
		{
			CreateDeadEnd(placementManager, result, tempPos);
		}
		else if(roadCount == 2)
		{
			if (CreateStraightRoad(placementManager, result, tempPos))
				return;

			CreateCorner(placementManager, result, tempPos);
		}
		else if(roadCount == 3)
		{
			Create3Way(placementManager, result, tempPos);
		}
		else
		{
			Create4Way(placementManager, result, tempPos);
		}
	}

	private void Create4Way(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)
	{
		placementManager.ModifyStructureModel(tempPos, fourWay, Quaternion.identity);
	}

	private void Create3Way(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)
	{
		if(result[1] ==  CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)				// if neighbours are Up -> Right -> Down
		{
			placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.identity);						// dont rotate
		}
		else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)        // if neighbours are Right -> Down -> left
		{
			placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,90,0));                 // rotate 90 degree
		}
		else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)        // if neighbours are  Down -> left -> Up
		{
			placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0, 180, 0));				// rotate 180 degree
		}
		else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)        // if neighbours are left -> Up -> Right
		{
			placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,270,0));                // rotate 270 degree
		}

	}

	private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)
	{
		if (result[1] == CellType.Road && result[2] == CellType.Road)									// if neighbours are Up -> Right
		{
			placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0, 90, 0));          
		}
		else if (result[2] == CellType.Road && result[3] == CellType.Road)								// if neighbours are Right -> Down
		{
			placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0, 180, 0));       
		}
		else if (result[3] == CellType.Road && result[0] == CellType.Road)								// if neighbours are  Down -> left
		{
			placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0, 270, 0));      
		}
		else if (result[0] == CellType.Road && result[1] == CellType.Road)								// if neighbours are left -> Up
		{
			placementManager.ModifyStructureModel(tempPos, corner, Quaternion.identity);      
		}
	}

	private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)
	{
		if (result[0] == CellType.Road && result[2] == CellType.Road)                                   // if neighbours are Left -> Right
		{
			placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.identity);
			return true;
		}
		else if (result[1] == CellType.Road && result[3] == CellType.Road)                              // if neighbours are Up -> Down
		{
			placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.Euler(0, 90, 0));
			return true;
		}

		return false;
	}

	private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)
	{
		if (result[1] == CellType.Road)                                                                 // if neighbour is Up
		{
			placementManager.ModifyStructureModel(tempPos, deadEnd, Quaternion.Euler(0, 270, 0));
		}
		else if (result[2] == CellType.Road)                                                            // if neighbour isRight
		{
			placementManager.ModifyStructureModel(tempPos, deadEnd, Quaternion.identity);
		}
		else if (result[3] == CellType.Road)                                                            // if neighbour is Down
		{
			placementManager.ModifyStructureModel(tempPos, deadEnd, Quaternion.Euler(0, 90, 0));
		}
		else if (result[0] == CellType.Road)                                                            // if neighbour is left
		{
			placementManager.ModifyStructureModel(tempPos, deadEnd, Quaternion.Euler(0, 180, 0));
		}
	}
}
