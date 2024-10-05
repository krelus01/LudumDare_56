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
			roomPoint.SetRoomPoint(null);
			_roomGridPoints.Add(roomPoint.Id, roomPoint);
		}
	}

	public Transform GetRoomPoint(int pointInRow)
	{
		return _roomGridPoints[pointInRow].transform;
	}

	public int GetRoomPointsCount()
	{
		return _roomGridPoints.Count + 1;
	}

	public void Initialize(RoomRowData roomRowData)
	{
		for (int i = 0; i < roomRowData.RoomGridPoints.Count; i++)
		{
			RoomPointData roomPointData = roomRowData.RoomGridPoints[i];
			_roomGridPoints[i].SetRoomPoint(roomPointData);
		}
	}
}
