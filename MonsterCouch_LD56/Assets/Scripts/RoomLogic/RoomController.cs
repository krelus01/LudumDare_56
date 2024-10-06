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
	
	public void Clear()
	{
		foreach (KeyValuePair<int,RoomRow> roomRow in _roomRows)
		{
			roomRow.Value.Clear();
		}
		
		_roomRows.Clear();
	}
	
	public Transform PlacePlayer(LevelData data)
	{
		return PlacePlayer(data.PlayerStartingRow, data.PlayerStartingRowPoint);
	}
	
	public Transform MovePlayer(MoveDirection direction)
	{
		GameController.Instance.MakeSaveForUndo();
		
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

		ConsumeSockFrom3d(roomPoint);

		return roomPoint.GetRoomPointTransform();
	}

	
	
	public Vector2Int GetPlayerPosition()
	{
		return new Vector2Int(_playerCurrentRow, _playerCurrentPointInRow);
	}
	
	public RoomData GetRoomData()
	{
		RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
		
		foreach (KeyValuePair<int, RoomRow> roomRow in _roomRows)
		{
			RoomRowData roomRowData = ScriptableObject.CreateInstance<RoomRowData>();
			
			foreach (KeyValuePair<int, RoomGridPoint> roomPoint in roomRow.Value.GetRoomGridPoints())
			{
				roomRowData.RoomGridPoints.Add(roomPoint.Value.GetRoomPointData());
			}
			
			roomData.RoomRows.Add(roomRowData);
		}
		
		return roomData;
	}
	
	
	private void ConsumeSockFrom3d(RoomGridPoint roomPoint)
	{
		if (roomPoint.RoomPointData.RoomPointType != SockType.Empty && roomPoint.RoomPointData.RoomPointType != SockType.Wall)
		{
			StomachController.Instance.AddSockToStomach(roomPoint.RoomPointData.RoomPointType);
			roomPoint.Consume();
		}
		
		GameOverIfEverySockIsConsumed();
	}

	private void GameOverIfEverySockIsConsumed()
	{
		foreach (KeyValuePair<int, RoomRow> roomRow in _roomRows)
		{
			foreach (KeyValuePair<int, RoomGridPoint> roomPoint in roomRow.Value.GetRoomGridPoints())
			{
				if (roomPoint.Value.RoomPointData.RoomPointType != SockType.Empty && roomPoint.Value.RoomPointData.RoomPointType != SockType.Wall)
				{
					return;
				}
			}
		}
		
		GameController.Instance.GameOver();
	}

	private Transform PlacePlayer(int row, int pointInRow)
	{
		_playerCurrentRow = row;
		_playerCurrentPointInRow = pointInRow;
		
		return _roomRows[row].GetRoomPoint(pointInRow).transform;
	}
}
