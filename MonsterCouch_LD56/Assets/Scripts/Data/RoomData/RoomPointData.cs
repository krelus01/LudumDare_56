using UnityEngine;

[CreateAssetMenu(menuName = "Create RoomPointData", fileName = "RoomPointData", order = 0)]
public class RoomPointData : ScriptableObject
{
	public TinyCreatureType RoomPointType;
	public GameObject RoomPointPrefab;
}

public enum TinyCreatureType
{
	Empty,
	Wall,
	GreenCreature,
	BlueCreature,
	RedCreature,
}