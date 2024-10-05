using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform _couchVisuals;
	
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
		
		RotateTowardsDirection(direction);
		MoveToTransform(RoomController.Instance.MovePlayer(direction));
	}

	private void RotateTowardsDirection(MoveDirection direction)
	{
		switch (direction)
		{
			case MoveDirection.Up:
				_couchVisuals.rotation = Quaternion.Euler(0, 0, 0);
				break;
			case MoveDirection.Down:
				_couchVisuals.rotation = Quaternion.Euler(0, 180, 0);
				break;
			case MoveDirection.Left:
				_couchVisuals.rotation = Quaternion.Euler(0, 270, 0);
				break;
			case MoveDirection.Right:
				_couchVisuals.rotation = Quaternion.Euler(0, 90, 0);
				break;
		}
	}

	private void MoveToTransform(Transform trasform)
	{
		gameObject.transform.position = trasform.position;
	}
}

public enum MoveDirection
{
	Up,
	Down,
	Left,
	Right
}