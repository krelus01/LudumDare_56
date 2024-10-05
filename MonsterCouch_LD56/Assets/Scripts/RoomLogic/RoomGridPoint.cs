using UnityEngine;

public class RoomGridPoint : MonoBehaviour
{
	[SerializeField] private int _id;
	[SerializeField] private RoomPointData _roomPointData;
	[Space]
	[SerializeField] private GameObject _floorPrefab;
	
	
	public int Id => _id;
	public RoomPointData RoomPointData => _roomPointData;

	public void SetRoomPoint(RoomPointData data)
	{
		_roomPointData = data;
		
		Instantiate(_floorPrefab, transform);

		if (_roomPointData != null && _roomPointData.RoomPointType != TinyCreatureType.Empty)
		{ 
			Instantiate(data.RoomPointPrefab, transform);
		}
	}
	
	public Transform GetRoomPointTransform()
	{
		return transform;
	}

	public RoomPointData GetRoomPointData()
	{
		return RoomPointData;
	}

	public void Consume()
	{
		_roomPointData = ScriptableObject.CreateInstance<RoomPointData>();
		_roomPointData.RoomPointType = TinyCreatureType.Empty;
		
		GetComponentInChildren<TinyCreatureInRoomController>().Clear();
	}
}