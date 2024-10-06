using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private static InputManager _instance;

	public static InputManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<InputManager>();
				if (_instance == null)
				{
					GameObject singletonObject = new(typeof(InputManager).Name);
					_instance = singletonObject.AddComponent<InputManager>();
				}
			}
			return _instance;
		}
	}

	public bool IsMovementBlocked { get; private set; }
	
	public Action MoveUp;
	public Action MoveLeft;
	public Action MoveRight;
	public Action RestartLevel;
	public Action UndoMove;
	
	private Action _backupMoveUp;
	private Action _backupMoveLeft;
	private Action _backupMoveRight;
	private Action _backupUndoMove;

	private Dictionary<string, Action> actions = new();
	
	public void Clear()
	{
		MoveUp = null;
		MoveLeft = null;
		MoveRight = null;
		RestartLevel = null;
		UndoMove = null;
		
		_backupMoveUp = null;
		_backupMoveLeft = null;
		_backupMoveRight = null;
		_backupUndoMove = null;
	}
	
	public void BlockMovement()
	{
		if (IsMovementBlocked)
		{
			return;
		}
		
		IsMovementBlocked = true;
		
		_backupMoveUp = MoveUp;
		_backupMoveLeft = MoveLeft;
		_backupMoveRight = MoveRight;
		_backupUndoMove = UndoMove;
		

		MoveUp = null;
		MoveLeft = null;
		MoveRight = null;
		UndoMove = null;
	}

	public void UnblockMovement()
	{
		IsMovementBlocked = false;
		
		MoveUp = _backupMoveUp;
		MoveLeft = _backupMoveLeft;
		MoveRight = _backupMoveRight;
		UndoMove = _backupUndoMove;
	}
	
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			
			actions = new Dictionary<string, Action>
			{
				{"MoveUp", () => MoveUp?.Invoke()},
				//{"MoveDown", () => MoveDown?.Invoke()},
				{"MoveLeft", () => MoveLeft?.Invoke()},
				{"MoveRight", () => MoveRight?.Invoke()},
				{"RestartLevel", () => RestartLevel?.Invoke()},
				{"Undo", () => UndoMove?.Invoke()},
			};
			
			PlayerInput playerInput = GetComponent<PlayerInput>();
			playerInput.onActionTriggered += OnActionTriggered;
			
			DontDestroyOnLoad(gameObject);
		}
	}

	private void OnActionTriggered(InputAction.CallbackContext context)
	{
		if (context.performed == false)
		{
			return;
		}
		
		if (actions.TryGetValue(context.action.name, out Action action))
		{
			action.Invoke();
		}
	}
}
