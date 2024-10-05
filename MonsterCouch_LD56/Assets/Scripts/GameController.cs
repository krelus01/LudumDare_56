using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private RoomController _roomController;
	[SerializeField] private StomachController _stomachController;
	[SerializeField] private PlayerController _playerController;

	[Space]
	[SerializeField] private GameObject _playerPrefab;
	
	[Space]
	[SerializeField] private List<LevelData> _levels;

	private int _currentLevelIndex = 0;

	private void Awake()
	{
		InputManager.Instance.RestartLevel += () =>
		{
			_roomController.Clear();
			_stomachController.Clear();
			_playerController.Clear();
			Initialize();
		};
		
		Initialize();
	}

	private void Initialize()
	{
		LevelData levelData = _levels[_currentLevelIndex];
		
		_roomController.Initialize(levelData.Room);
		Transform transform = _roomController.PlacePlayer(levelData);
		
		_stomachController.Initialize(levelData.Stomach);
		
		_playerController = Instantiate(_playerPrefab).GetComponent<PlayerController>();
		_playerController.Initialize(transform);
	}
}