using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Dizzy = Animator.StringToHash("Dizzy");

    public void SetMoveState(bool value) => _animator.SetBool(Move, value);
    public void SetAttackState(bool value) => _animator.SetBool(Attack, value);
    public bool GetAttackState() => _animator.GetBool(Attack);
    public void SetDizzyState(bool value) => _animator.SetBool(Dizzy, value);
    
    public void Disable() => _animator.enabled = false;
    public void Enable() => _animator.enabled = true;
}
