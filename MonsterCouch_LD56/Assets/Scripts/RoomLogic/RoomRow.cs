using System.Collections.Generic;
using UnityEngine;

public class RoomRow : MonoBehaviour
{
	[SerializeField] private int _rowId;
	
	[SerializeField] private Dictionary<int, RoomGridPoint> _roomGridPoints = new();
	
	public int Id => _rowId;

	public void Initialize(RoomRowData roomRowData)
	{
		foreach (RoomGridPoint roomPoint in GetComponentsInChildren<RoomGridPoint>())
		{
			_roomGridPoints.Add(roomPoint.Id, roomPoint);
		}
		
		for (int i = 0; i < roomRowData.RoomGridPoints.Count; i++)
		{
			RoomPointData roomPointData = roomRowData.RoomGridPoints[i];
			_roomGridPoints[i + 1].SetRoomPoint(roomPointData);
		}
	}

	public void Clear()
	{
		foreach (KeyValuePair<int, RoomGridPoint> roomPoint in _roomGridPoints)
		{
			roomPoint.Value.Consume();
		}
		
		_roomGridPoints.Clear();
	}
	
	public RoomGridPoint GetRoomPoint(int pointInRow)
	{
		return _roomGridPoints[pointInRow];
	}

	public IEnumerable<KeyValuePair<int,RoomGridPoint>> GetRoomGridPoints()
	{
		return _roomGridPoints;
	}
}
