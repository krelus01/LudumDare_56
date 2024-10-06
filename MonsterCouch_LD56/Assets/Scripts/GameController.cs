using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController Instance;
	
	[SerializeField] private GameEndPanel _gameEndPanel;
	
	[Space]
	[SerializeField] private RoomController _roomController;
	[SerializeField] private StomachController _stomachController;
	[SerializeField] private PlayerController _playerController;

	[Space]
	[SerializeField] private GameObject _playerPrefab;
	
	[Space]
	[SerializeField] private List<LevelData> _levels;

	private LevelUndoSystem _undoSystem = new();
	
	private int _currentLevelIndex = 0;
	
	
	public void MakeSaveForUndo()
	{
		_undoSystem.SaveState(new GameState(
		    _stomachController.GetStomachSlots(),
			_roomController.GetPlayerPosition(),
		    _playerController.CurrentDirection, 
		    _roomController.GetRoomData()
		));
	}
	
	public async UniTaskVoid GameOver()
	{
		_gameEndPanel.ShowGameOver();
		
		await UniTask.Delay(2000);
		
		_gameEndPanel.Hide();
		
		RestartLevel();
	}
	
	public async UniTaskVoid LevelCompleted()
	{
		_gameEndPanel.ShowLevelCompleted();
		
		await UniTask.Delay(2000);
		
		_gameEndPanel.Hide();
		
		_currentLevelIndex++;
		if (_currentLevelIndex >= _levels.Count)
		{
			_currentLevelIndex = 0;
		}
		
		RestartLevel();
	}
	
	
	
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
		
		Initialize();
	}

	private void Initialize()
	{
		InputManager.Instance.RestartLevel += RestartLevel;
		
		LevelData levelData = _levels[_currentLevelIndex];
		
		_roomController.Initialize(levelData.Room);
		Transform startingPosition = _roomController.PlacePlayer(levelData);
		
		_stomachController.Initialize(levelData.Stomach);
		
		_playerController = Instantiate(_playerPrefab).GetComponent<PlayerController>();
		_playerController.Initialize(startingPosition);
	}

	private void RestartLevel()
	{
		_roomController.Clear();
		_stomachController.Clear();
		_playerController.Clear();
		InputManager.Instance.Clear();
		Initialize();
	}
}

public class LevelUndoSystem
{
	private Stack<GameState> _states = new();
	
	public void SaveState(GameState state)
	{
		_states.Push(state);
	}
	
	public GameState Undo()
	{
		if (_states.Count == 0)
		{
			return null;
		}
		
		return _states.Pop();
	}
	
	public void Clear()
	{
		_states.Clear();
	}
}

public class GameState
{
	public List<StomachSlotData> StomachSlots { get; private set; }
	public Vector2Int PlayerPosition { get; private set; }
	public MoveDirection PlayerDirection { get; private set; }
	public RoomData RoomData { get; private set; }

	public GameState(List<StomachSlotData> stomachSlots, Vector2Int playerPosition, MoveDirection playerDirection, RoomData roomData)
	{
		StomachSlots = new List<StomachSlotData>(stomachSlots);
		PlayerPosition = playerPosition;
		PlayerDirection = playerDirection;
		RoomData = roomData;
	}
}