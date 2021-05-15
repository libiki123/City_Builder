using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
    private Vector2 cameraMovementVector;

    [SerializeField] Camera mainCamera;

    public LayerMask groundMask;

    public Vector2 CameraMovementVector
	{
        get { return cameraMovementVector; }
	}

	private void Update()
	{
        CheckClickDownEvent();
		CheckClickUpEvent();
		CheckClickHoldEvent();
		CheckArrowInput();
	}

	private Vector3Int? RaycastGround()
	{
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))		// check if mouse click hit the ground
		{
			Vector3Int posInt = Vector3Int.RoundToInt(hit.point);
			return posInt;
		}
		return null;
	}

	private void CheckArrowInput()
	{
		cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void CheckClickHoldEvent()
	{
		if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)	// mouse hold and check if pointer are not over game UI
		{
			var pos = RaycastGround();
			if(pos != null)
			{
				OnMouseHold?.Invoke(pos.Value);
			}
		}
	}

	private void CheckClickUpEvent()
	{
		if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)  // mouse up and check if pointer are not over game UI
		{
			OnMouseUp?.Invoke();
		}
	}

	private void CheckClickDownEvent()
	{
		if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)  // mouse down and check if pointer are not over game UI
		{
			var pos = RaycastGround();
			if (pos != null)
			{
				OnMouseClick?.Invoke(pos.Value);
			}
		}
	}
}
