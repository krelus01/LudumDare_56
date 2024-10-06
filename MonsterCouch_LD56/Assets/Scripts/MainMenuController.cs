using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
   [SerializeField] private GameObject _mainMenuPanel;
   [SerializeField] private Button _startButton;
   [SerializeField] private Button _exitButton;
   
   private void Start()
   {
	   SoundManager.Instance.PlayMusic(AudioClipType.BackgroundMusic);
	   
	   _startButton.Select();
	   
	   _startButton.onClick.AddListener(StartGame);
	   _exitButton.onClick.AddListener(ExitGame);
   }

   private void TogglePauseMenu()
   {
	   if (gameObject.activeSelf)
	   {
		   gameObject.SetActive(false);
	   }
	   else
	   {
		   _exitButton.Select();
		   
		   _startButton.gameObject.SetActive(false);
		   gameObject.SetActive(true);
	   }
   }

   private void StartGame()
   {
	   InputManager.Instance.Pause += TogglePauseMenu;
	   
	   _mainMenuPanel.SetActive(false);
	   GameController.Instance.StartGame();
   }
   
   private void ExitGame()
   {
	   Application.Quit();
   }
}
