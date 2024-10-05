using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create RoomData", fileName = "RoomData", order = 0)]
public class RoomData : ScriptableObject
{
	public int PlayerStartingRow;
	public int PlayerStartingRowPoint;
	
	public List<RoomRowData> RoomRows;
}