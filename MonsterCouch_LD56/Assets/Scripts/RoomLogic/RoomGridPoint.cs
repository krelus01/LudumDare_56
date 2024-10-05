using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGridPoint : MonoBehaviour
{
	[SerializeField] private int _id;
	[Space]
	[SerializeField] private GameObject _floorPrefab;
	
	public int Id => _id;

	public void SetRoomPoint(RoomPointData data)
	{
		Instantiate(_floorPrefab, transform);
	}
}