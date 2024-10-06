using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Ease _rotationEase = Ease.Linear;
	[SerializeField] private float _rotationDuration = 0.75f;
	[SerializeField] private Transform _couchVisuals;
	
	private MoveDirection _currentDirection = MoveDirection.Up;
	
	public MoveDirection CurrentDirection => _currentDirection;

	public void Initialize(Transform startingPosition)
	{
		gameObject.transform.position = startingPosition.position;

		InputManager.Instance.MoveUp += () => Move(MoveDirection.Up).Forget();
		InputManager.Instance.MoveLeft += () => Move(MoveDirection.Left).Forget();
		InputManager.Instance.MoveRight += () => Move(MoveDirection.Right).Forget();
	}

	public void Clear()
	{
		InputManager.Instance.MoveUp -= () => Move(MoveDirection.Up).Forget();
		InputManager.Instance.MoveLeft -= () => Move(MoveDirection.Left).Forget();
		InputManager.Instance.MoveRight -= () => Move(MoveDirection.Right).Forget();
		
		Destroy(gameObject);
	}
	
	public void SetPositionAndDirection(Transform undoPlayerPosition, MoveDirection statePlayerDirection)
	{
		gameObject.transform.position = undoPlayerPosition.position;
		_currentDirection = statePlayerDirection;
		RotateTowardsDirection(_currentDirection);
	}
	
	
	
	private async UniTaskVoid Move(MoveDirection direction)
	{
		InputManager.Instance.BlockMovement();
		
		GameController.Instance.MakeSaveForUndo();
		
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
		SoundManager.Instance.PlayEffect(AudioClipType.CouchMoving);
		await StomachController.Instance.PlayerMoved(direction);
		RoomGridPoint gridPoint = RoomController.Instance.MovePlayer(_currentDirection);
		await UniTask.WhenAll(RoomController.Instance.ConsumeSockFrom3d(gridPoint), MoveToTransform(gridPoint.GetRoomPointTransform()));
		
		InputManager.Instance.UnblockMovement();
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

	private async UniTask MoveToTransform(Transform targetTransform)
	{
		await gameObject.transform.DOMove(targetTransform.position, 0.35f).AsyncWaitForCompletion();
	}
}

public enum MoveDirection
{
	Up,
	Right,
	Down,
	Left,
}