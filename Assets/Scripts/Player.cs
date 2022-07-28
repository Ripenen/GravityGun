using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action NextBaseReached;
    public event Action Lose;
    public Transform PointForEnemy => _pointForEnemy;
    
    [SerializeField] private Transform _pointForEnemy;
    [SerializeField] private GravityGun _gravityGun;
    [SerializeField] private int _health = 10;

    private Coroutine _rotationCoroutine;

    public void Init()
    {
        _gravityGun.enabled = true;
    }
    
    public void LookTo(Vector3 point)
    {
        var lookRotation = Quaternion.LookRotation(point - transform.position);

        transform.DORotate(new Vector3(0, lookRotation.eulerAngles.y, 0), 3);
    }

    public void ApplyDamage(int damage)
    {
        _health -= damage;
        
        if(_health <= 0)
            Lose?.Invoke();
    }

    public void MoveTo(PlayerMovePath path)
    {
        _gravityGun.Reset();

        _gravityGun.enabled = false;
        var lastPoint = path.GetLastPoint();
        var point = path.GetPoint();
        
        //transform.DORotate(new Vector3(0, point.rotation.eulerAngles.y, 0), 2)
        
        StartCoroutine(SmoothMove());
        

        IEnumerator SmoothMove()
        {
            transform.DOMove(new Vector3(point.position.x, transform.position.y, point.position.z), 2)
                .OnComplete(() => _gravityGun.enabled = true).SetDelay(1);
            transform.DORotate(new Vector3(0, point.rotation.eulerAngles.y, 0), 2).SetDelay(1);
            
            while ((transform.position - lastPoint.position).sqrMagnitude > MathF.Sqrt(4))
            {
                if ((transform.position - point.position).sqrMagnitude <= MathF.Sqrt(4))
                {
                    point = path.GetPoint();
                    transform.DORotate(new Vector3(0, point.rotation.eulerAngles.y, 0), 2);
                    transform.DOMove(new Vector3(point.position.x, transform.position.y, point.position.z), 2);
                }

                yield return new WaitForEndOfFrame();
            }
            
            NextBaseReached?.Invoke();
        }
    }
}
