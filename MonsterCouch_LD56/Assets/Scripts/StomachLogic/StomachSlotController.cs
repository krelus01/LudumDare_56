using System;
using System.Collections.Generic;
using UnityEngine;

public class StomachSlotController : MonoBehaviour
{
	[SerializeField] private int _slotId;
	[SerializeField] private List<StomachSlotController> _neighbours;
	
	private StomachSlotData _stomachSlotData;

	public int Id => _slotId;
	public bool IsEmpty => _stomachSlotData.TinyCreatureType == StomachElementType.Empty;

	public void SetNeighbours(List<StomachSlotController> neighbours)
	{
		_neighbours = neighbours;
	}

	public void Initialize(StomachSlotData stomachSlot)
	{
		_stomachSlotData = stomachSlot;
		
		if (stomachSlot.TinyCreatureType != StomachElementType.Empty)
		{
			Instantiate(stomachSlot.StomachElementPrefab, transform);
		}
	}
	
	public void MoveElementFrom(StomachSlotController sourceSlot)
	{
		// Update the target slot's data with the source slot's data
		_stomachSlotData = sourceSlot._stomachSlotData;

		// Set the source slot's data to empty
		StomachSlotData emptySlot = ScriptableObject.CreateInstance<StomachSlotData>();
		emptySlot.TinyCreatureType = StomachElementType.Empty;
		sourceSlot._stomachSlotData = emptySlot;

		// Update the visual representation
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		if (_stomachSlotData.TinyCreatureType != StomachElementType.Empty)
		{
			Instantiate(_stomachSlotData.StomachElementPrefab, transform);
		}

		foreach (Transform child in sourceSlot.transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void SetTinyCreature(StomachSlotData stomachSlot)
	{
		_stomachSlotData = stomachSlot;
		
		Instantiate(stomachSlot.StomachElementPrefab, transform);
	}
}