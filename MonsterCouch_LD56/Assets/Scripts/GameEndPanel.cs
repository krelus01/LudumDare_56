using TMPro;
using UnityEngine;

internal class GameEndPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _gameEndText;
	[SerializeField] private TextMeshProUGUI _leveCompletedText;
	[SerializeField] private TextMeshProUGUI _gameBeatenText;

	private string nominalLoseText = "Too bad!\nTry again...";
	
	public void ShowGameOver(string message)
	{
		gameObject.SetActive(true);
		
		_gameEndText.text = nominalLoseText + "\n\n" + message;
		
		_gameEndText.gameObject.SetActive(true);
	}
	
	public void ShowLevelCompleted()
	{
		gameObject.SetActive(true);
		_leveCompletedText.gameObject.SetActive(true);
	}
	
	public void Hide()
	{
		gameObject.SetActive(false);
		_gameEndText.gameObject.SetActive(false);
		_leveCompletedText.gameObject.SetActive(false);
		_gameBeatenText.gameObject.SetActive(false);
	}

	public void ShowGameBeaten()
	{
		gameObject.SetActive(true);
		_gameBeatenText.gameObject.SetActive(true);
	}
}