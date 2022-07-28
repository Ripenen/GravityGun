using System;
using System.Collections;
using FIMSpace.FProceduralAnimation;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> Died;
    public event Action MakeAttack;
    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _radiusForAttack;
    [SerializeField] private PickableItem _enemyItem;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private RagdollAnimator _ragDollAnimator;
    [SerializeField] private Collider _collider;

    private Action _currentStateUpdate = () => {};

    public void Init(Transform player)
    {
        _enemyAnimator.SetMoveState(true);
        _agent.enabled = true;
        
        _enemyItem.Captured += OnCaptured;
        _agent.SetDestination(player.position);
        _agent.stoppingDistance = 1;
        _currentStateUpdate = UpdateState;
        _enemyAnimator.Enable();
    }

    private void OnDestroy()
    {
        _enemyItem.Captured -= OnCaptured;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_enemyItem.IsThrown)
            Die();
        
        if(other.gameObject.TryGetComponent<PickableItem>(out var item) && item.IsThrown)
            Die();
    }

    public void Die()
    {
        if(_ragDollAnimator is not null)
            _ragDollAnimator.User_EnableFreeRagdoll();
        
        _enemyAnimator.SetMoveState(false);
        _enemyAnimator.SetAttackState(false);
        _enemyAnimator.SetDizzyState(false);
        
        _enemyAnimator.Disable();

        _enemyItem.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        
        if(_collider)
            _collider.enabled = false;
        
        _agent.enabled = false;
        Died?.Invoke(this);
        
        Destroy(_enemyItem);
        Destroy(this);
    }

    private void OnCaptured()
    {
        _enemyAnimator.SetMoveState(false);
        _enemyAnimator.SetAttackState(false);
        _enemyAnimator.SetDizzyState(true);
        _agent.enabled = false;
    }

    private IEnumerator Attack()
    {
        while (_enemyAnimator.GetAttackState())
        {
            MakeAttack?.Invoke();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateState()
    {
        var offset = transform.position - _agent.destination;

        if (offset.sqrMagnitude <= MathF.Sqrt(_radiusForAttack))
        {
            _enemyAnimator.SetMoveState(false);
            _enemyAnimator.SetAttackState(true);
            StartCoroutine(Attack());
            _currentStateUpdate = () => { };
        }
    }

    private void Update()
    {
        if(_agent.enabled)
            transform.localPosition = Vector3.zero;
        
        _currentStateUpdate();
    }
}
