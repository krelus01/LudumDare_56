using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create RoomRowData", fileName = "RoomRowData", order = 0)]
public class RoomRowData : ScriptableObject
{
	public List<RoomPointData> RoomGridPoints;
}