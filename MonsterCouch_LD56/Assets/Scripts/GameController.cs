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

	private int _currentLevelIndex = 0;

	
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