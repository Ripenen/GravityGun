using System.Collections;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    [SerializeField] private float _repulsionForce;
    [SerializeField] private float _gravityForce;
    [SerializeField] private ParticleSystem _gravityEffect;
    [SerializeField] private GameObject _gravityGun;
    [SerializeField] private FireBall _fireBallPrefab;

    private PickableItem _capturedItem;
    private Ray _ray;
    private FireBall _fireBall;
    private bool _fireballUsed;
    private RaycastHit _lastHit;
    private Coroutine _capture;

    private void Awake()
    {
        _fireBall = Instantiate(_fireBallPrefab);
    }

    public void Reset()
    {
        _capturedItem = null;
    }

    private void Update()
    {
        foreach(var touch in Input.touches) 
        {
            if (touch.tapCount == 2 && !_fireballUsed) 
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);
                _fireBall.Throw(_ray.GetPoint(0),_ray.direction, 5);
                _fireballUsed = true;
            }    
        }

        if (Input.GetMouseButton(0))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if(_gravityEffect.isStopped)
                _gravityEffect.Play();

            _gravityEffect.transform.rotation = Quaternion.LookRotation(_ray.direction);
            _gravityGun.transform.rotation = Quaternion.LookRotation(_ray.direction);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if(_capturedItem is not null)
                _capturedItem.Throw(_ray.direction, _repulsionForce);
            
            if(_capture is not null)
                StopCoroutine(_capture);
            
            _capturedItem = null;
            _gravityEffect.Stop();
        }

        if (_capturedItem is not null)
        {
            var gravityPoint = _ray.GetPoint(1.5f);

            _capturedItem.AttractToPoint(gravityPoint, _gravityForce * Time.deltaTime);
            
            _gravityGun.transform.rotation = Quaternion.LookRotation(_ray.direction);
            _gravityEffect.transform.rotation = Quaternion.LookRotation(_ray.direction);
        }
    }
    
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && _capturedItem is null)
        {
            if (!Physics.Raycast(_ray, out var hitInfo))
                return;
            
            if (_lastHit.transform != hitInfo.transform)
            {
                if (_capture is not null)
                {
                    StopCoroutine(_capture);
                    _capture = null;
                }
                
                _lastHit = hitInfo;
            }

            _lastHit = hitInfo;
            
            if (hitInfo.transform.TryGetComponent(out PickableItem item) && item.Pickable && _capture is null)
            {
                _capture = StartCoroutine(Capture(item));
            }
        }
    }

    private IEnumerator Capture(PickableItem captureItem)
    {
        yield return new WaitForSeconds(0.25f);

        captureItem.Capture();

        _capturedItem = captureItem;
    }
}