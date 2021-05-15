using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
    float yHeight = 0;
    
    public void CreateModel(GameObject model)
	{
        var structure = Instantiate(model, transform);
        yHeight = structure.transform.position.y;
	}

    public void SwapModel(GameObject model, Quaternion rotation)		// replace the old stucture with new one
	{
		foreach (Transform child in transform)		// destroy all children object
		{
			Destroy(child.gameObject);
		}

		var structure = Instantiate(model, transform);
		structure.transform.localPosition = new Vector3(0, yHeight, 0);
		structure.transform.localRotation = rotation;
	}
}
