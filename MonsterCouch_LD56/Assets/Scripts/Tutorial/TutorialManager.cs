using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] private TutorialControlPanelController _leftButtonPanel;
	[SerializeField] private TutorialControlPanelController _rightButtonPanel;
	[SerializeField] private TutorialControlPanelController _upButtonPanel;
	[SerializeField] private TutorialControlPanelController _undoButtonPanel;
	[SerializeField] private TutorialControlPanelController _restartLevelButtonPanel;
	
	public bool IsFinished { get; private set; }
	
	private bool _leftPressed;
	private bool _rightPressed;
	private bool _upPressed;
	private bool _undoPressed;
	private bool _restartLevelPressed;
	
	
	public async UniTask ShowTutorial()
	{
		IsFinished = false;
		
		gameObject.SetActive(true);
		await gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).ToUniTask();
		
		InputManager.Instance.MoveLeft += () => PressLeft().Forget();
		InputManager.Instance.MoveRight += () => PressRight().Forget();
		InputManager.Instance.MoveUp += () => PressUp().Forget();
		InputManager.Instance.UndoMove += () => PressUndo().Forget();
		InputManager.Instance.RestartLevel += () => PressRestartLevel().Forget();
	}

	private async UniTaskVoid PressRestartLevel()
	{
		_restartLevelPressed = true;
		InputManager.Instance.RestartLevel += () => PressRestartLevel().Forget();
		
		await _restartLevelButtonPanel.InvokePressing();
		
		await CheckIfFinished();
	}

	private async UniTaskVoid PressUndo()
	{
		_undoPressed = true;
		InputManager.Instance.UndoMove -= () => PressUndo().Forget();
		
		await _undoButtonPanel.InvokePressing();
		
		await CheckIfFinished();
	}

	private async UniTaskVoid PressUp()
	{
		_upPressed = true;
		InputManager.Instance.MoveUp -= () => PressUp().Forget();
		
		await _upButtonPanel.InvokePressing();
		
		await CheckIfFinished();
	}

	private async UniTaskVoid PressRight()
	{
		_rightPressed = true;
		InputManager.Instance.MoveRight -= () => PressRight().Forget();
		
		await _rightButtonPanel.InvokePressing();
		
		await CheckIfFinished();
	}

	private async UniTaskVoid PressLeft()
	{
		_leftPressed = true;
		InputManager.Instance.MoveLeft -= () => PressLeft().Forget();
		
		await _leftButtonPanel.InvokePressing();
		
		await CheckIfFinished();
	}

	private async UniTask CheckIfFinished()
	{
		if (_leftPressed && _rightPressed && _upPressed && _undoPressed && _restartLevelPressed)
		{
			await UniTask.Delay(1000);
			await gameObject.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).ToUniTask();
			gameObject.SetActive(false);
			IsFinished = true;
		}
	}
}
