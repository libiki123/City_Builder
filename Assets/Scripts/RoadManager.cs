using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public List<Vector3Int> tempPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPosToRecheck = new List<Vector3Int>();

	private Vector3Int startPos;
	private bool placementMode = false;

	public RoadFixer roadFixer;

	private void Awake()
	{
		roadFixer = GetComponent<RoadFixer>();
	}

	public void PlaceRoad(Vector3Int pos)
	{
		if(!placementManager.CheckPosInBound(pos))				// if not within the grid
			return;
		if (!placementManager.CheckIfPosIsFree(pos))			// if it not empty
			return;	
		if (!placementMode)										// if not drag (clicking), can either end here or the start of drag mode
		{
			tempPlacementPositions.Clear();
			roadPosToRecheck.Clear();

			placementMode = true;
			startPos = pos;

			tempPlacementPositions.Add(pos);
			placementManager.PlaceTempStructure(pos, roadFixer.roadStraight, CellType.Road);		// put down a straightRoad as temporary			
		}
		else
		{
			placementManager.RemoveAllTempStructures();
			tempPlacementPositions.Clear();

			foreach (var posToFix in roadPosToRecheck)                  // in case player change their mind and drag else where
			{
				roadFixer.FixRoadAtPos(placementManager, posToFix);     // we want to fix the previous roadToCheck
			}

			roadPosToRecheck.Clear();

			tempPlacementPositions = placementManager.GetPathBetween(startPos, pos);

			foreach (var tempPos in tempPlacementPositions)
			{
				if (!placementManager.CheckIfPosIsFree(tempPos))            // if it not empty
				{
					roadPosToRecheck.Add(tempPos);
					continue;
				}
				placementManager.PlaceTempStructure(tempPos, roadFixer.roadStraight, CellType.Road);
			}
		}

		FixRoadPrefabs();
	}

	private void FixRoadPrefabs()
	{
		foreach (var tempPos in tempPlacementPositions)		
		{
			roadFixer.FixRoadAtPos(placementManager, tempPos);								// place the correct road base on it surrounding
			var neighbour = placementManager.GetNeighbourTypeFor(tempPos, CellType.Road);	// get sorround pos of type
			foreach (var roadPos in neighbour)
			{
				if(!roadPosToRecheck.Contains(roadPos))										// in case roadPosToCheck already cantain this pos
					roadPosToRecheck.Add(roadPos);
			}
		}

		foreach (var posTofix in roadPosToRecheck)
		{
			roadFixer.FixRoadAtPos(placementManager, posTofix);								// replace place the road with the correct model
		}
	}

	public void FinishPlacingRoad()
	{
		placementMode = false;
		placementManager.AddTermpStructureToStructureDictionary();

		if(tempPlacementPositions.Count > 0)
		{
			AudioPlayer.Instance.PlayPlacementSound();
		}

		tempPlacementPositions.Clear();
		startPos = Vector3Int.zero;
	}
}
