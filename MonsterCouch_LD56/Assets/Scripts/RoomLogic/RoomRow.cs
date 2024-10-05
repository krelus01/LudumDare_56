using System.Collections.Generic;
using UnityEngine;

public class RoomRow : MonoBehaviour
{
	[SerializeField] private int _rowId;
	
	Dictionary<int, RoomGridPoint> _roomGridPoints = new();

	private void Awake()
	{
		foreach (RoomGridPoint roomPoint in GetComponentsInChildren<RoomGridPoint>())
		{
			roomPoint.SetRoomPoint(null);
			_roomGridPoints.Add(roomPoint.Id, roomPoint);
		}
	}
}
