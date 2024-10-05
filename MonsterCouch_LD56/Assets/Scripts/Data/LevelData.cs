using UnityEngine;

[CreateAssetMenu(menuName = "Create LevelData", fileName = "LevelData", order = 0)]
public class LevelData : ScriptableObject
{
	public int PlayerStartingRow;
	public int PlayerStartingRowPoint;
	
	public RoomData Room;
	public StomachData Stomach;
}
