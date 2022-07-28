using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public event Action AnyDied;
    public event Action AllDied;
    public event Action AnyAttack;

    [SerializeField] private List<Enemy> _enemies;

    public int Count => _enemies.Count;

    public void Init(Transform player)
    {
        foreach (var enemy in _enemies.ToList())
        {
            enemy.Init(player);
            enemy.Died += OnEnemyDied;
            enemy.MakeAttack += OnMakeAttack;
        }
    }

    private void OnMakeAttack()
    {
        AnyAttack?.Invoke();
    }

    private void OnDestroy()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Died -= OnEnemyDied;
            enemy.MakeAttack -= OnMakeAttack;
        }
    }

    private void OnEnemyDied(Enemy enemy)
    {
        _enemies.Remove(enemy);

        if (_enemies.Count == 0)
        {
            AllDied?.Invoke();
            return;
        }
            

        AnyDied?.Invoke();
    }

    public Enemy GetClosestEnemy(Vector3 point)
    {
        if (_enemies.Count == 0)
            return null;
        var result = _enemies[0];
        
        foreach (var enemy in _enemies)
        {
            if ((result.transform.position - point).sqrMagnitude > (enemy.transform.position - point).sqrMagnitude)
                result = enemy;
        }

        return result;
    }
}
