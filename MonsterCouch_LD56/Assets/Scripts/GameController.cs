using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private RoomController _roomController;
	[SerializeField] private PlayerController _playerController;

	[Space]
	[SerializeField] private GameObject _playerPrefab;
	
	[Space]
	[SerializeField] private List<RoomData> _levels;

	private int _currentLevelIndex = 0;

	private void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		RoomData roomData = _levels[_currentLevelIndex];
		
		_roomController.Initialize(roomData);
		Transform transform = _roomController.PlacePlayer(roomData);
		
		_playerController = Instantiate(_playerPrefab).GetComponent<PlayerController>();
		_playerController.Initialize(transform);
	}
}