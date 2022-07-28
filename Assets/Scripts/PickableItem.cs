using System;
using RayFire;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickableItem : MonoBehaviour
{
    public event Action Captured;
    
    [SerializeField] private RayfireRigid _rayFireRigidbody;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private bool _destroyable = true;
    public bool Pickable = true;


    public bool IsThrown { get; private set; }

    public void Throw(Vector3 direction, float force)
    {
        GetComponent<Collider>().enabled = true;
        _rigidbody.velocity = Vector3.zero;
        
        _rigidbody.AddForce(direction * force, ForceMode.VelocityChange);

        IsThrown = true;
    }

    public void AttractToPoint(Vector3 point, float force)
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = Vector3.MoveTowards(transform.position, point + _offset, force);
    }

    public void Capture()
    {
        GetComponent<Collider>().enabled = false;
        Captured?.Invoke();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(IsThrown && _destroyable)
            _rayFireRigidbody.Demolish();
    }
}