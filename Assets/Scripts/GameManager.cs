using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
	public RoadManager roadManager;
    public InputManager inputManager;
	public UIController uiController;

	private void Start()
	{
		uiController.OnRoadPlacement += RoadPlacementHandler;
		uiController.OnHousePlacement += HousePlacementHandler;
		uiController.OnSpecialPlacement += SpecialPlacementHandler;
	}

	private void SpecialPlacementHandler()
	{
		ClearInputAction();
	}

	private void HousePlacementHandler()
	{
		ClearInputAction();
	}

	private void RoadPlacementHandler()
	{
		ClearInputAction();

		inputManager.OnMouseClick += roadManager.PlaceRoad;
		inputManager.OnMouseHold += roadManager.PlaceRoad;
		inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
	}

	private void ClearInputAction()
	{
		inputManager.OnMouseClick = null;
		inputManager.OnMouseHold = null;
		inputManager.OnMouseUp = null;
	}

	private void Update()
	{
		cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
	}
}
