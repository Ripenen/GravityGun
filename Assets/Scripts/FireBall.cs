using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();
        
        gameObject.SetActive(false);
    }

    private void Disable() => gameObject.SetActive(false);

    public void Throw(Vector3 point, Vector3 direction, float force)
    {
        gameObject.SetActive(true);
        _rigidbody.velocity = Vector3.zero;
        transform.position = point;
        
        _rigidbody.AddForce(direction * force, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision other)
    {
        _rigidbody.velocity = Vector3.zero;
        
        if(other.gameObject.TryGetComponent<Enemy>(out var enemy))
            enemy.Die();
        
        Invoke(nameof(Disable), 1);
    }
}
