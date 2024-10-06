using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SocksInRoomController : MonoBehaviour
{
	private CancellationTokenSource _cts;
	
	private void Awake()
	{
		_cts = new CancellationTokenSource();
	    FloatingAnimation(_cts.Token).Forget();
	}
	
	private async UniTaskVoid FloatingAnimation(CancellationToken ctsToken)
	{
	    Vector3 startPosition = transform.position;
	    float amplitude = 0.1f;
	    float frequency = 1f;
	
	    await transform.DOMoveY(startPosition.y + amplitude, frequency)
	        .SetLoops(-1, LoopType.Yoyo)
	        .SetEase(Ease.InOutSine)
			.ToUniTask(cancellationToken: ctsToken);
	}

	public void Clear()
	{
		_cts.Cancel();
		Destroy(gameObject);
	}
}
