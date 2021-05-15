using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housePrefabs, specialPrefabs;
    public PlacementManager placementManager;

    private float[] houseWeights, specialWeights;

	private void Start()
	{
        houseWeights = housePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int pos)
	{
		if (CheckPOsBeforePlacement(pos))
		{
			int randomIndex = GetRandomWeightedIndex(houseWeights);
			placementManager.PlaceObjectOnTheMap(pos, housePrefabs[randomIndex].prefab, CellType.Structure);
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

	private bool CheckPOsBeforePlacement(Vector3Int pos)
	{
		if (!placementManager.CheckPosInBound(pos))										// out of grid bound
			return false;

		if (!placementManager.CheckIfPosIsFree(pos))									// not free
			return false;

		if (placementManager.GetNeighbourTypeFor(pos, CellType.Road).Count <= 0)		// no road arround
			return false;

		return true;
	}
}

[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    [Range(0, 1)] public float weight;
}
