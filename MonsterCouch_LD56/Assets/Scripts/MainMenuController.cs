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
	   _startButton.Select();
	   
	   _startButton.onClick.AddListener(StartGame);
	   _exitButton.onClick.AddListener(ExitGame);
   }

   private void StartGame()
   {
	   _mainMenuPanel.SetActive(false);
	   GameController.Instance.StartGame();
   }
   
   private void ExitGame()
   {
	   Application.Quit();
   }
}
