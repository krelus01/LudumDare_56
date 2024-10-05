using UnityEngine;

[CreateAssetMenu(menuName = "Create RoomPointData", fileName = "RoomPointData", order = 0)]
public class RoomPointData : ScriptableObject
{
	public RoomPointType RoomPointType;
	public GameObject RoomPointPrefab;
}

public enum RoomPointType
{
	Empty,
	Wall,
	GreenCreature,
	BlueCreature,
	RedCreature,
}