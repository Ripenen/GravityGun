using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<LevelBase> _bases;
    [SerializeField] private Player _player;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private LevelGenerator _levelGenerator;

    public static int LevelNumber = 1;

    private void Awake()
    {
        if(_levelGenerator)
            _levelGenerator.Generate(_bases);
    }

    private void Start()
    {
        _gameUI.SetLevelText(LevelNumber.ToString());
        StartCoroutine(LevelStart());
    }

    private IEnumerator LevelStart()
    {
        yield return _gameUI.TapWait();
        
        _player.NextBaseReached += OnNextBaseReached;
        _player.Lose += _gameUI.ShowLose;
        
        _gameUI.SetMaxProgressValue(_bases.Sum(levelBase => levelBase.EnemiesCount));
        _player.Init();

        OnNextBaseReached();
    }

    private void OnDestroy()
    {
        _player.NextBaseReached -= OnNextBaseReached;
        _player.Lose -= _gameUI.ShowLose;
    }

    private void OnNextBaseReached()
    {
        _bases[0].Init(_player, _gameUI);
        _bases.Remove(_bases[0]);
    }
}