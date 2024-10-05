using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class StomachController : MonoBehaviour
{
	public static StomachController Instance;

	private const int ROW_SIZE = 5;
	private const int COLUMN_SIZE = 5;

	[SerializeField] private List<StomachSlotController> _stomachSlots;
	[Space]
	[SerializeField] private StomachSlotData _emptyTinyCreaturePrefab;
	[SerializeField] private StomachSlotData _redTinyCreaturePrefab;
	[SerializeField] private StomachSlotData _blueTinyCreaturePrefab;
	[SerializeField] private StomachSlotData _greenTinyCreaturePrefab;

	
	private CancellationTokenSource _animCts;
	
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
		InputManager.Instance.MoveUp += () => Moved(MoveDirection.Up);
		InputManager.Instance.MoveLeft += () => Moved(MoveDirection.Left);
		InputManager.Instance.MoveRight += () => Moved(MoveDirection.Right);

		RecognizeNeighbours(ROW_SIZE, COLUMN_SIZE);

		for (int i = 0; i < stomachData.StomachSlots.Count; i++)
		{
			_stomachSlots[i].Initialize(stomachData.StomachSlots[i]);
		}
	}
	
	public void Clear()
	{
		InputManager.Instance.MoveUp -= () => Moved(MoveDirection.Up);
		InputManager.Instance.MoveLeft -= () => Moved(MoveDirection.Left);
		InputManager.Instance.MoveRight -= () => Moved(MoveDirection.Right);
		
		foreach (StomachSlotController slot in _stomachSlots)
		{
			slot.Clear();
		}
	}

	public void AddTinyCreature(TinyCreatureType roomPointType)
	{
		for (int row = ROW_SIZE - 1; row >= 0; row--)
		{
			int index = row * COLUMN_SIZE + (COLUMN_SIZE / 2);
			if (_stomachSlots[index].IsEmpty)
			{
				StomachSlotData stomachslotData = roomPointType switch
				{
					TinyCreatureType.RedCreature => _redTinyCreaturePrefab,
					TinyCreatureType.BlueCreature => _blueTinyCreaturePrefab,
					TinyCreatureType.GreenCreature => _greenTinyCreaturePrefab,
					_ => throw new ArgumentOutOfRangeException()
				};
				
				_stomachSlots[index].SetTinyCreature(stomachslotData);
				break;
			}
		}

		ResetCts();
		
		CompleteThreeOfAKind(_animCts.Token).Forget();
	}


	private async UniTask CompleteThreeOfAKind(CancellationToken animCtsToken)
	{
		List<StomachSlotController> matchedSlots = FindThreeOrMoreOfAKind();
	
		List<UniTask> tasks = new();
		foreach (StomachSlotController slot in matchedSlots)
		{
		    tasks.Add(slot.CompleteSlotAsync(_emptyTinyCreaturePrefab, animCtsToken));
		}
		
		InputManager.Instance.BlockMovement();
		
		await UniTask.WhenAll(tasks);
	
		MoveElementsDown();
		
		InputManager.Instance.UnblockMovement();
	}
	
	private void MoveElementsDown()
	{
		for (int col = 0; col < COLUMN_SIZE; col++)
		{
			for (int row = ROW_SIZE - 1; row >= 0; row--)
			{
				int index = row * COLUMN_SIZE + col;

				if (!_stomachSlots[index].IsEmpty)
				{
					int targetIndex = FindLowestEmptySlot(col, row);

					if (targetIndex != -1)
					{
						_stomachSlots[targetIndex].MoveElementFrom(_stomachSlots[index]);
					}
				}
			}
		}
	}

	private int FindLowestEmptySlot(int col, int startRow)
	{
		for (int row = ROW_SIZE - 1; row > startRow; row--)
		{
			int index = row * COLUMN_SIZE + col;

			if (_stomachSlots[index].IsEmpty)
			{
				return index;
			}
		}

		return -1;
	}
	
	private List<StomachSlotController> FindThreeOrMoreOfAKind()
	{
		List<StomachSlotController> matchedSlots = new();
	
		// Check rows for matches
		for (int row = 0; row < ROW_SIZE; row++)
		{
			for (int col = 0; col < COLUMN_SIZE - 2; col++)
			{
				int index = row * COLUMN_SIZE + col;
				if (!_stomachSlots[index].IsEmpty &&
					_stomachSlots[index].ElementType == _stomachSlots[index + 1].ElementType &&
					_stomachSlots[index].ElementType == _stomachSlots[index + 2].ElementType)
				{
					matchedSlots.Add(_stomachSlots[index]);
					matchedSlots.Add(_stomachSlots[index + 1]);
					matchedSlots.Add(_stomachSlots[index + 2]);
				}
			}
		}
		
		// Check columns for matches
		for (int col = 0; col < COLUMN_SIZE; col++)
		{
			for (int row = 0; row < ROW_SIZE - 2; row++)
			{
				int index = row * COLUMN_SIZE + col;
				if (!_stomachSlots[index].IsEmpty &&
					_stomachSlots[index].ElementType == _stomachSlots[index + COLUMN_SIZE].ElementType &&
					_stomachSlots[index].ElementType == _stomachSlots[index + 2 * COLUMN_SIZE].ElementType)
				{
					matchedSlots.Add(_stomachSlots[index]);
					matchedSlots.Add(_stomachSlots[index + COLUMN_SIZE]);
					matchedSlots.Add(_stomachSlots[index + 2 * COLUMN_SIZE]);
				}
			}
		}
	
		return matchedSlots;
	}
	
	
	
	private void Moved(MoveDirection direction)
	{
		if (direction == MoveDirection.Right)
		{
			MoveElementsToMostRight();
		}
		else if (direction == MoveDirection.Left)
		{
			MoveElementsToMostLeft();
		}
		
		ResetCts();
		
		CompleteThreeOfAKind(_animCts.Token).Forget();
	}

	private void MoveElementsToMostRight()
	{
		for (int row = 0; row < ROW_SIZE; row++)
		{
			for (int col = COLUMN_SIZE - 1; col >= 0; col--)
			{
				int index = row * COLUMN_SIZE + col;

				if (!_stomachSlots[index].IsEmpty)
				{
					int targetIndex = FindMostRightEmptySlot(row, col);

					if (targetIndex != -1)
					{
						_stomachSlots[targetIndex].MoveElementFrom(_stomachSlots[index]);
					}
				}
			}
		}
	}

	private void MoveElementsToMostLeft()
	{
		for (int row = 0; row < ROW_SIZE; row++)
		{
			for (int col = 0; col < COLUMN_SIZE; col++)
			{
				int index = row * COLUMN_SIZE + col;

				if (!_stomachSlots[index].IsEmpty)
				{
					int targetIndex = FindMostLeftEmptySlot(row, col);

					if (targetIndex != -1)
					{
						_stomachSlots[targetIndex].MoveElementFrom(_stomachSlots[index]);
					}
				}
			}
		}
	}

	private int FindMostRightEmptySlot(int row, int startCol)
	{
		for (int col = COLUMN_SIZE - 1; col > startCol; col--)
		{
			int index = row * COLUMN_SIZE + col;

			if (_stomachSlots[index].IsEmpty)
			{
				return index;
			}
		}

		return -1;
	}

	private int FindMostLeftEmptySlot(int row, int startCol)
	{
		for (int col = 0; col < startCol; col++)
		{
			int index = row * COLUMN_SIZE + col;

			if (_stomachSlots[index].IsEmpty)
			{
				return index;
			}
		}

		return -1;
	}

	private void RecognizeNeighbours(int rowSize, int columnSize)
	{
		foreach (StomachSlotController slot in _stomachSlots)
		{
			int id = slot.Id;
			int row = (id - 1) / columnSize;
			int col = (id - 1) % columnSize;

			List<StomachSlotController> neighbours = new();

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
	
	private void ResetCts()
	{
		_animCts?.Cancel();
		_animCts = new CancellationTokenSource();
	}
}