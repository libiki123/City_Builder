using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housePrefabs, specialPrefabs, bigStructurePrefabs;
    public PlacementManager placementManager;

    private float[] houseWeights, specialWeights, bigStructureWeights;

	private void Start()
	{
        houseWeights = housePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
		bigStructureWeights = bigStructurePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int pos)
	{
		if (CheckPosBeforePlacement(pos))
		{
			int randomIndex = GetRandomWeightedIndex(houseWeights);
			placementManager.PlaceObjectOnTheMap(pos, housePrefabs[randomIndex].prefab, CellType.Structure);
			AudioPlayer.Instance.PlayPlacementSound();
		}
	}

	internal void PlaceBigStructure(Vector3Int pos)
	{
		int width = 2;
		int height = 2;
		if(CheckBigStructure(pos, width, height))
		{
			int randomIndex = GetRandomWeightedIndex(bigStructureWeights);
			placementManager.PlaceObjectOnTheMap(pos, bigStructurePrefabs[randomIndex].prefab, CellType.Structure, width, height);
			AudioPlayer.Instance.PlayPlacementSound();
		}
	}

	private bool CheckBigStructure(Vector3Int pos, int width, int height)		// check the surround of big structure base on width and height
	{
		bool nearRoad = false;
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				var newPos = pos + new Vector3Int(x, 0, z);
				if (!DefaultCheck(newPos))							// check if the pos is in bound
					return false;                                   // false if any of the pos is out of bound

				if (!nearRoad)                                      // if 1 of surround pos near a road dont need to check again
					nearRoad = RoadCheck(newPos);                   // check if the pos near a road
			}
		}
		return nearRoad;
	}

	public void PlaceSpecial(Vector3Int pos)
	{
		if (CheckPosBeforePlacement(pos))
		{
			int randomIndex = GetRandomWeightedIndex(specialWeights);
			placementManager.PlaceObjectOnTheMap(pos, specialPrefabs[randomIndex].prefab, CellType.Structure);
			AudioPlayer.Instance.PlayPlacementSound();
		}
	}

	private int GetRandomWeightedIndex(float[] weights)
	{
		float sum = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			sum += weights[i];
		}

		float randomValue = UnityEngine.Random.Range(0, sum);
		float tempSum = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			if(randomValue >= tempSum && randomValue < tempSum + weights[i])
			{
				return i;
			}
			tempSum += weights[i];
		}

		return 0;
	}

	private bool CheckPosBeforePlacement(Vector3Int pos)
	{
		if (!DefaultCheck(pos))
			return false;

		if (!RoadCheck(pos))
			return false;

		return true;
	}

	private bool RoadCheck(Vector3Int pos)
	{
		if (placementManager.GetNeighbourTypeFor(pos, CellType.Road).Count <= 0)        // no road arround
		{
			Debug.Log("Must be near a road");
			return false;
		}

		return true;
	}

	private bool DefaultCheck(Vector3Int pos)
	{
		if (!placementManager.CheckPosInBound(pos))                                     // out of grid bound
		{
			Debug.Log("This pos is out of bound");
			return false;
		}

		if (!placementManager.CheckIfPosIsFree(pos))                                    // not free
		{
			Debug.Log("This pos is not Empty");
			return false;
		}

		return true;
	}
}

[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    [Range(0, 1)] public float weight;
}
