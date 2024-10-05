using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create StomachData", fileName = "StomachData", order = 0)]
public class StomachData : ScriptableObject
{
	public List<StomachSlotData> _stomachSlots;
	
	public List<StomachSlotData> StomachSlots => _stomachSlots;
}