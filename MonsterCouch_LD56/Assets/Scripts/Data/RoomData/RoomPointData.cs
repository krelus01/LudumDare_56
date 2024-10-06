using UnityEngine;

[CreateAssetMenu(menuName = "Create RoomPointData", fileName = "RoomPointData", order = 0)]
public class RoomPointData : ScriptableObject
{
	public SockType RoomPointType;
	public GameObject RoomPointPrefab;
}

public enum SockType
{
	Empty,
	Wall,
	GreenCreature,
	BlueCreature,
	RedCreature,
}