using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
		Initialize(stomachData.StomachSlots);
	}

	private void Initialize(List<StomachSlotData> stomachSlots)
	{
		RecognizeNeighbours(ROW_SIZE, COLUMN_SIZE);

		for (int i = 0; i < stomachSlots.Count; i++)
		{
			_stomachSlots[i].Initialize(stomachSlots[i]);
		}
	}
	
	public void Clear()
	{
		foreach (StomachSlotController slot in _stomachSlots)
		{
			slot.Clear();
		}
	}

	public void PlayerMoved(MoveDirection direction)
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
	
	public void AddSockToStomach(SockType roomPointType)
	{
		CheckPotentialGameOver();
		
		for (int row = ROW_SIZE - 1; row >= 0; row--)
		{
			int index = row * COLUMN_SIZE + (COLUMN_SIZE / 2);
			if (_stomachSlots[index].IsEmpty)
			{
				StomachSlotData stomachslotData = roomPointType switch
				{
					SockType.RedCreature => _redTinyCreaturePrefab,
					SockType.BlueCreature => _blueTinyCreaturePrefab,
					SockType.GreenCreature => _greenTinyCreaturePrefab,
					_ => throw new ArgumentOutOfRangeException()
				};
				
				_stomachSlots[index].SetSock(stomachslotData);
				break;
			}
		}

		ResetCts();
		
		CompleteThreeOfAKind(_animCts.Token).Forget();
	}

	public List<StomachSlotData> GetStomachSlots()
	{
		List<StomachSlotData> stomachSlots = new();
		foreach (StomachSlotController slot in _stomachSlots)
		{
			stomachSlots.Add(slot.GetStomachSlotData());
		}

		return stomachSlots;
	}
	
	public void SetStomachSlots(List<StomachSlotData> stateStomachSlots)
	{
		Clear();
		Initialize(stateStomachSlots);
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
	
		await MoveElementsDown();
		
		InputManager.Instance.UnblockMovement();
		
		CheckIfStomachIsEmpty();
	}

	private void CheckIfStomachIsEmpty()
	{
		bool isEmpty = true;
		foreach (StomachSlotController slot in _stomachSlots)
		{
			if (!slot.IsEmpty)
			{
				isEmpty = false;
				break;
			}
		}

		if (isEmpty)
		{
			GameController.Instance.LevelCompleted().Forget();
		}
	}

	private async UniTask MoveElementsDown()
	{
		bool moved = false;
		
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
						moved = true;
					}
				}
			}
		}

		if (!moved) return;

		ResetCts();
		await CompleteThreeOfAKind(_animCts.Token);
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
					_stomachSlots[index].SockType == _stomachSlots[index + 1].SockType &&
					_stomachSlots[index].SockType == _stomachSlots[index + 2].SockType)
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
					_stomachSlots[index].SockType == _stomachSlots[index + COLUMN_SIZE].SockType &&
					_stomachSlots[index].SockType == _stomachSlots[index + 2 * COLUMN_SIZE].SockType)
				{
					matchedSlots.Add(_stomachSlots[index]);
					matchedSlots.Add(_stomachSlots[index + COLUMN_SIZE]);
					matchedSlots.Add(_stomachSlots[index + 2 * COLUMN_SIZE]);
				}
			}
		}
	
		return matchedSlots;
	}
	
	private void CheckPotentialGameOver()
	{
		if (_stomachSlots[2].IsEmpty)
		{
			return;
		}
		
		GameController.Instance.GameOver().Forget();
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