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

	public Action MoveUp;
	public Action MoveDown;
	public Action MoveLeft;
	public Action MoveRight;

	private Dictionary<string, Action> actions = new();
	
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
				{"Jump", () => Debug.Log("Jump")},
				{"Attack", () => Debug.Log("Attack")},
				{"Interact", () => Debug.Log("Interact")},
				{"Pause", () => Debug.Log("Pause")}
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
