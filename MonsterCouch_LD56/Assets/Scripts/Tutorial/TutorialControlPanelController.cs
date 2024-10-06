using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TutorialControlPanelController : MonoBehaviour
{
	[SerializeField] private GameObject _beforePress;
	[SerializeField] private GameObject _afterPress;

	public async UniTask InvokePressing()
	{
		_beforePress.SetActive(false);
		await _afterPress.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).ToUniTask();
		_afterPress.SetActive(true);
	}
}
