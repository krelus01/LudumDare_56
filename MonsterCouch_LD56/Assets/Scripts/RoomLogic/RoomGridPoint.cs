using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGridPoint : MonoBehaviour
{
	[SerializeField] private int _id;
	[SerializeField] private RoomPointData _roomPointData;
	[Space]
	[SerializeField] private GameObject _floorPrefab;
	
	
	public int Id => _id;

	public void SetRoomPoint(RoomPointData data)
	{
		_roomPointData = data;
		
		Instantiate(_floorPrefab, transform);

		if (_roomPointData != null && _roomPointData.RoomPointType != RoomPointType.Empty)
		{ 
			Instantiate(data.RoomPointPrefab, transform);
		}
	}
}