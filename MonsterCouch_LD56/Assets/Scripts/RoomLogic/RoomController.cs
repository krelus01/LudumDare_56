using System.Collections;
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
			
			foreach (RoomRow roomRow in GetComponentsInChildren<RoomRow>())
			{
				_roomRows.Add(roomRow.Id, roomRow);
			}
			
			DontDestroyOnLoad(gameObject);
		}
	}

	public void Initialize(RoomData data)
	{
		
	}
	
	public Transform PlacePlayer(RoomData data)
	{
		return PlacePlayer(data.PlayerStartingRow, data.PlayerStartingRowPoint);
	}
	
	public Transform MovePlayer(MoveDirection direction)
	{
		return null;
	}

	private Transform PlacePlayer(int row, int pointInRow)
	{
		_playerCurrentRow = row;
		_playerCurrentPointInRow = pointInRow;
		
		return _roomRows[row].GetRoomPoint(pointInRow).transform;
	}
}
