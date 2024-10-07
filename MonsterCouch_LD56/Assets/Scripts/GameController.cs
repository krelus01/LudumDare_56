using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController Instance;
	
	[SerializeField] private GameEndPanel _gameEndPanel;
	[SerializeField] private TutorialManager _tutorialManager;
	
	[Space]
	[SerializeField] private RoomController _roomController;
	[SerializeField] private StomachController _stomachController;
	[SerializeField] private PlayerController _playerController;

	[Space]
	[SerializeField] private GameObject _playerPrefab;
	
	[Space]
	[SerializeField] private List<LevelData> _levels;
	[SerializeField] private bool _skipTutorial;

	private LevelUndoSystem _undoSystem = new();
	
	private int _currentLevelIndex = 0;
	private bool _isGameOverOrCompleted = false;


	public void StartGame()
	{
		if (_skipTutorial)
		{
			Initialize();
		}
		else
		{
			StartGameWithTutorial().Forget();
		}
	}
	
	public void MakeSaveForUndo()
	{
		_undoSystem.SaveState(new GameState(
		    _stomachController.GetStomachSlots(),
			_roomController.GetPlayerPosition(),
		    _playerController.CurrentDirection, 
		    _roomController.GetRoomData()
		));
	}
	
	public async UniTaskVoid GameOver(string message)
	{
		if (_isGameOverOrCompleted)
		{
			return;
		}
		
		_isGameOverOrCompleted = true;
		
		InputManager.Instance.BlockMovement();
		
		_gameEndPanel.ShowGameOver(message);
		
		await UniTask.Delay(2000);
		
		_gameEndPanel.Hide();
		
		InputManager.Instance.UnblockMovement();
		
		RestartLevel();
	}
	
	public async UniTaskVoid LevelCompleted()
	{
		if (_isGameOverOrCompleted)
		{
			return;
		}
		
		_isGameOverOrCompleted = true;
		
		InputManager.Instance.BlockMovement();
		
		_gameEndPanel.ShowLevelCompleted();
		
		await UniTask.Delay(2000);
		
		_gameEndPanel.Hide();
		
		_currentLevelIndex++;
		if (_currentLevelIndex >= _levels.Count)
		{
			_gameEndPanel.ShowGameBeaten();
			return;
		}
		
		InputManager.Instance.UnblockMovement();
		
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
	}

	private async UniTaskVoid StartGameWithTutorial()
	{
		await _tutorialManager.ShowTutorial();
		
		LevelData levelData = _levels[_currentLevelIndex];
		
		_roomController.Initialize(levelData.Room);
		Transform startingPosition = _roomController.PlacePlayer(levelData);
		
		_stomachController.Initialize(levelData.Stomach);
		
		_playerController = Instantiate(_playerPrefab).GetComponent<PlayerController>();
		
		await UniTask.WaitUntil(() => _tutorialManager.IsFinished);
		
		InputManager.Instance.RestartLevel += RestartLevel;
		InputManager.Instance.UndoMove += UndoMove;
		
		_playerController.Initialize(startingPosition);
	}

	private void Initialize()
	{
		InputManager.Instance.RestartLevel += RestartLevel;
		InputManager.Instance.UndoMove += UndoMove;
		
		LevelData levelData = _levels[_currentLevelIndex];
		
		_roomController.Initialize(levelData.Room);
		Transform startingPosition = _roomController.PlacePlayer(levelData);
		
		_stomachController.Initialize(levelData.Stomach);
		
		_playerController = Instantiate(_playerPrefab).GetComponent<PlayerController>();
		_playerController.Initialize(startingPosition);
	}

	private void UndoMove()
	{
		GameState state = _undoSystem.GetUndoState();
		if (state == null)
		{
			return;
		}
		
		_roomController.SetRoomData(state.RoomData);
		Transform undoPlayerPosition = _roomController.SetPosition(state.PlayerPosition);
		_stomachController.SetStomachSlots(state.StomachSlots);
		_playerController.SetPositionAndDirection(undoPlayerPosition, state.PlayerDirection);
	}

	private void RestartLevel()
	{
		_isGameOverOrCompleted = false;
		
		InputManager.Instance.RestartLevel -= RestartLevel;
		InputManager.Instance.UndoMove -= UndoMove;
		
		_roomController.Clear();
		_stomachController.Clear();
		_playerController.Clear();
		_undoSystem.Clear();
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
	
	public GameState GetUndoState()
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