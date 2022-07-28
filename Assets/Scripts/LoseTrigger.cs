using System;
using UnityEngine;

public class LoseTrigger : MonoBehaviour
{
    public event Action Trigged;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<PickableItem>(out var item) && item.IsThrown)
            Trigged?.Invoke();
    }
}