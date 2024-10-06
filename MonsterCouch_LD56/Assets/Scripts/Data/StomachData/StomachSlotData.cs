using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Create StomachSlotData", fileName = "StomachSlotData", order = 0)]
public class StomachSlotData : ScriptableObject
{
	public StomachSlotData(StomachSockType sockType)
	{
		SockType = sockType;
	}
	
	[FormerlySerializedAs("TinyCreatureType")] public StomachSockType SockType;
	public GameObject StomachElementPrefab;
}

public enum StomachSockType
{
	Empty,
	Red,
	Blue,
	Green,
}