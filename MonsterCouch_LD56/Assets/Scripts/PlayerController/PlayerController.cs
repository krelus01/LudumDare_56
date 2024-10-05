using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public void Initialize(Transform startingPosition)
	{
		gameObject.transform.position = startingPosition.position;
		
		InputManager.Instance.MoveUp += () => Move(MoveDirection.Up);
		InputManager.Instance.MoveDown += () => Move(MoveDirection.Down);
		InputManager.Instance.MoveLeft += () => Move(MoveDirection.Left);
		InputManager.Instance.MoveRight += () => Move(MoveDirection.Right);
	}

	private void Move(MoveDirection direction)
	{
		Debug.Log("Move" + direction);
		
		Transform trasform = RoomController.Instance.MovePlayer(direction);
	}
}

public enum MoveDirection
{
	Up,
	Down,
	Left,
	Right
}