using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
	public static RoomController Instance;
	
	[SerializeField] private Dictionary<int, RoomRow> _roomRows = new();
	
	private int _playerCurrentRow;
	private int _playerCurrentPointInRow;
	
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void Initialize(RoomData data)
	{
		foreach (RoomRow roomRow in GetComponentsInChildren<RoomRow>())
		{
			_roomRows.Add(roomRow.Id, roomRow);
		}
		
		for (int i = 0; i < data.RoomRows.Count; i++)
		{
			RoomRowData roomRowData = data.RoomRows[i];
			_roomRows[i + 1].Initialize(roomRowData);
		}
	}
	
	public Transform PlacePlayer(LevelData data)
	{
		return PlacePlayer(data.PlayerStartingRow, data.PlayerStartingRowPoint);
	}
	
	public Transform MovePlayer(MoveDirection direction)
	{
		switch (direction)
		{
			case MoveDirection.Up:
				if (_playerCurrentRow < 7)
				{
					_playerCurrentRow++;
				}
				break;
			case MoveDirection.Down:
				if (_playerCurrentRow > 1)
				{
					_playerCurrentRow--;
				}
				break;
			case MoveDirection.Left:
				if (_playerCurrentPointInRow > 1)
				{
					_playerCurrentPointInRow--;
				}
				break;
			case MoveDirection.Right:
				if (_playerCurrentPointInRow < 7)
				{
					_playerCurrentPointInRow++;
				}
				break;
		}

		RoomGridPoint roomPoint = _roomRows[_playerCurrentRow].GetRoomPoint(_playerCurrentPointInRow);

		ConsumeTinyCreature(roomPoint);

		return roomPoint.GetRoomPointTransform();
	}

	private void ConsumeTinyCreature(RoomGridPoint roomPoint)
	{
		if (roomPoint.RoomPointData.RoomPointType != TinyCreatureType.Empty && roomPoint.RoomPointData.RoomPointType != TinyCreatureType.Wall)
		{
			StomachController.Instance.AddTinyCreature(roomPoint.RoomPointData.RoomPointType);
			roomPoint.Consume();
		}
	}

	private Transform PlacePlayer(int row, int pointInRow)
	{
		_playerCurrentRow = row;
		_playerCurrentPointInRow = pointInRow;
		
		return _roomRows[row].GetRoomPoint(pointInRow).transform;
	}
}
