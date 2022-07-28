using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private EnemyContainer _enemyContainer;
    [SerializeField] private PlayerMovePath _pathToNextBase;
    [SerializeField] private bool _isEnd;
    [SerializeField] private LoseTrigger _loseTrigger;

    private Player _player;
    private GameUI _gameUI;

    public int EnemiesCount => _enemyContainer.Count;

    public void Init(Player player, GameUI gameUI)
    {
        _player = player;
        _gameUI = gameUI;
        
        _enemyContainer.AnyDied += OnAnyDied;
        _enemyContainer.AllDied += OnAllDied;
        _enemyContainer.AnyAttack += OnAnyAttack;
        _loseTrigger.Trigged += TriggedLose;
        
        _enemyContainer.Init(_player.PointForEnemy);
        
        _player.LookTo(_enemyContainer.GetClosestEnemy(_player.transform.position).transform.position);
    }

    private void TriggedLose()
    {
        _gameUI.ShowLose();
    }

    public void AddPlayerPathPoint(params Transform[] points) => _pathToNextBase.Add(points);
    public void MakeEnd() => _isEnd = true;

    private void OnAnyAttack()
    {
        _player.ApplyDamage(1);
    }

    private void OnDestroy()
    {
        _enemyContainer.AnyDied -= OnAnyDied;
        _enemyContainer.AllDied -= OnAllDied;
        _enemyContainer.AnyAttack -= OnAnyAttack;
        _loseTrigger.Trigged -= TriggedLose;
    }

    private void OnAnyDied()
    {
        _player.LookTo(_enemyContainer.GetClosestEnemy(_player.transform.position).transform.position);
        _gameUI.AddProgress(1);
    }

    private void OnAllDied()
    {
        if (_isEnd)
        {
            _gameUI.ShowWin();
            _gameUI.AddProgress(100);
            Level.LevelNumber++;
            return;
        }
        
        _gameUI.AddProgress(1);
        _player.MoveTo(_pathToNextBase);
    }
}