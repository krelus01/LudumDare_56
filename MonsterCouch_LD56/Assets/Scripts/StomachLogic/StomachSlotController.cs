using System.Collections.Generic;
using UnityEngine;

public class StomachSlotController : MonoBehaviour
{
	[SerializeField] private int _slotId;
	[SerializeField] private List<StomachSlotController> _neighbours;
	
	public int Id => _slotId;

	public void SetNeighbours(List<StomachSlotController> neighbours)
	{
		_neighbours = neighbours;
	}

	public void Initialize(StomachSlotData stomachSlot)
	{
		if (stomachSlot.TinyCreatureType != StomachElementType.Empty)
		{
			Instantiate(stomachSlot.StomachElementPrefab, transform);
		}
	}
}
