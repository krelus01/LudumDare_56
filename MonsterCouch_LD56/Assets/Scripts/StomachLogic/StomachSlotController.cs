using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class StomachSlotController : MonoBehaviour
{
	[SerializeField] private int _slotId;
	[SerializeField] private List<StomachSlotController> _neighbours;
	
	private StomachSlotData _stomachSlotData;

	public int Id => _slotId;
	public bool IsEmpty => _stomachSlotData.SockType == StomachSockType.Empty;
	public StomachSockType SockType => _stomachSlotData.SockType;

	public void Initialize(StomachSlotData stomachSlot)
	{
		_stomachSlotData = stomachSlot;
		
		if (stomachSlot.SockType != StomachSockType.Empty)
		{
			Instantiate(stomachSlot.StomachElementPrefab, transform);
		}
	}
	
	public void Clear()
	{
		_stomachSlotData = ScriptableObject.CreateInstance<StomachSlotData>();
		_stomachSlotData.SockType = StomachSockType.Empty;
		
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}
	
	public StomachSlotData GetStomachSlotData()
	{
		return _stomachSlotData;
	}
	
	public void SetNeighbours(List<StomachSlotController> neighbours)
	{
		_neighbours = neighbours;
	}
	
	public void MoveElementFrom(StomachSlotController sourceSlot)
	{
		// Update the target slot's data with the source slot's data
		_stomachSlotData = sourceSlot._stomachSlotData;

		// Set the source slot's data to empty
		StomachSlotData emptySlot = ScriptableObject.CreateInstance<StomachSlotData>();
		emptySlot.SockType = StomachSockType.Empty;
		sourceSlot._stomachSlotData = emptySlot;

		// Update the visual representation
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		if (_stomachSlotData.SockType != StomachSockType.Empty)
		{
			Instantiate(_stomachSlotData.StomachElementPrefab, transform);
		}

		foreach (Transform child in sourceSlot.transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void SetSock(StomachSlotData stomachSlot)
	{
		_stomachSlotData = stomachSlot;
		
		Instantiate(stomachSlot.StomachElementPrefab, transform);
	}

	public async UniTask CompleteSlotAsync(StomachSlotData emptyTinyCreaturePrefab, CancellationToken cancellationToken)
	{
		// Simple disappearing animation using DOTween
		float duration = 0.4f; // duration of the animation
	    await transform.DOScale(Vector3.zero, duration).ToUniTask(cancellationToken: cancellationToken);
	
		_stomachSlotData = emptyTinyCreaturePrefab;
		// Set empty tiny creature
		
		// Update the visual representation
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	
		// Restore the original scale
	    await transform.DOScale(Vector3.one, 0f).ToUniTask(cancellationToken: cancellationToken);
	}
}