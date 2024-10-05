using System;
using System.Collections.Generic;
using UnityEngine;

public class StomachController : MonoBehaviour
{
	public static StomachController Instance;
	
	private const int ROW_SIZE = 5;
	private const int COLUMN_SIZE = 5;
	
	[SerializeField] private List<StomachSlotController> _stomachSlots;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void Initialize(StomachData stomachData)
	{
		RecognizeNeighbours(ROW_SIZE, COLUMN_SIZE);
		
		for (int i = 0; i < stomachData.StomachSlots.Count; i++)
		{
			_stomachSlots[i].Initialize(stomachData.StomachSlots[i]);
		}
	}

	public void RecognizeNeighbours(int rowSize, int columnSize)
	{
		foreach (var slot in _stomachSlots)
		{
			int id = slot.Id;
			int row = (id - 1) / columnSize;
			int col = (id - 1) % columnSize;

			List<StomachSlotController> neighbours = new List<StomachSlotController>();

			// Check above
			if (row > 0)
				neighbours.Add(_stomachSlots[(row - 1) * columnSize + col]);

			// Check below
			if (row < rowSize - 1)
				neighbours.Add(_stomachSlots[(row + 1) * columnSize + col]);

			// Check left
			if (col > 0)
				neighbours.Add(_stomachSlots[row * columnSize + (col - 1)]);

			// Check right
			if (col < columnSize - 1)
				neighbours.Add(_stomachSlots[row * columnSize + (col + 1)]);

			slot.SetNeighbours(neighbours);
		}
	}
}