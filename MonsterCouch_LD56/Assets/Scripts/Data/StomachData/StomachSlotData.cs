using UnityEngine;

[CreateAssetMenu(menuName = "Create StomachSlotData", fileName = "StomachSlotData", order = 0)]
public class StomachSlotData : ScriptableObject
{
	public StomachSlotData(StomachElementType tinyCreatureType)
	{
		TinyCreatureType = tinyCreatureType;
	}
	
	public StomachElementType TinyCreatureType;
	public GameObject StomachElementPrefab;
}

public enum StomachElementType
{
	Empty,
	Red,
	Blue,
	Green,
}