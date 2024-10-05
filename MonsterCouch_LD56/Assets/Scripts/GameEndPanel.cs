using TMPro;
using UnityEngine;

internal class GameEndPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _gameEndText;
	[SerializeField] private TextMeshProUGUI _leveCompletedText;
	
	public void ShowGameOver()
	{
		gameObject.SetActive(true);
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
	}
}