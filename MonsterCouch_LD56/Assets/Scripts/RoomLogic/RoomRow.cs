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
}
