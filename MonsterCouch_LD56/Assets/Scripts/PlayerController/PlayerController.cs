using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Ease _rotationEase = Ease.Linear;
	[SerializeField] private float _rotationDuration = 0.75f;
	[SerializeField] private Transform _couchVisuals;
	
	private MoveDirection _currentDirection = MoveDirection.Up;
	
	public void Initialize(Transform startingPosition)
	{
		gameObject.transform.position = startingPosition.position;
		
		InputManager.Instance.MoveUp += () => Move(MoveDirection.Up);
		InputManager.Instance.MoveLeft += () => Move(MoveDirection.Left);
		InputManager.Instance.MoveRight += () => Move(MoveDirection.Right);
	}

	private void Move(MoveDirection direction)
	{
		Debug.Log("Move" + direction);

		if (direction == MoveDirection.Left)
		{
			_currentDirection = _currentDirection switch
			{
				MoveDirection.Up => MoveDirection.Left,
				MoveDirection.Left => MoveDirection.Down,
				MoveDirection.Down => MoveDirection.Right,
				MoveDirection.Right => MoveDirection.Up,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		else if (direction == MoveDirection.Right)
		{
			_currentDirection = _currentDirection switch
			{
				MoveDirection.Up => MoveDirection.Right,
				MoveDirection.Right => MoveDirection.Down,
				MoveDirection.Down => MoveDirection.Left,
				MoveDirection.Left => MoveDirection.Up,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		
		RotateTowardsDirection(_currentDirection);
		MoveToTransform(RoomController.Instance.MovePlayer(_currentDirection));
	}

	private void RotateTowardsDirection(MoveDirection direction)
	{
		switch (direction)
		{
			case MoveDirection.Up:
				_couchVisuals.DORotate(new Vector3(0, 0, 0), _rotationDuration).SetEase(_rotationEase);
				break;
			case MoveDirection.Down:
					_couchVisuals.DORotate(new Vector3(0, 180, 0), _rotationDuration).SetEase(_rotationEase);
				break;
			case MoveDirection.Left:
					_couchVisuals.DORotate(new Vector3(0, 270, 0), _rotationDuration).SetEase(_rotationEase);
				break;
			case MoveDirection.Right:
					_couchVisuals.DORotate(new Vector3(0, 90, 0), _rotationDuration).SetEase(_rotationEase);
				break;
		}
	}

	private void MoveToTransform(Transform targetTransform)
	{
	    gameObject.transform.DOMove(targetTransform.position, 0.5f);
	}
}

public enum MoveDirection
{
	Up,
	Right,
	Down,
	Left,
}