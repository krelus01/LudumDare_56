using System.Collections.Generic;
using UnityEngine;

public class RoomRow : MonoBehaviour
{
	[SerializeField] private int _rowId;
	
	[SerializeField] private Dictionary<int, RoomGridPoint> _roomGridPoints = new();
	
	public int Id => _rowId;

	private void Awake()
	{
		foreach (RoomGridPoint roomPoint in GetComponentsInChildren<RoomGridPoint>())
		{
			_roomGridPoints.Add(roomPoint.Id, roomPoint);
		}
	}

	public RoomGridPoint GetRoomPoint(int pointInRow)
	{
		return _roomGridPoints[pointInRow];
	}

	public void Initialize(RoomRowData roomRowData)
	{
		for (int i = 0; i < roomRowData.RoomGridPoints.Count; i++)
		{
			RoomPointData roomPointData = roomRowData.RoomGridPoints[i];
			_roomGridPoints[i + 1].SetRoomPoint(roomPointData);
		}
	}
}
