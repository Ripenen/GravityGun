using UnityEngine;

public class LevelBasePrefab : MonoBehaviour
{
    [SerializeField] private GameObject _enterKey;
    [SerializeField] private GameObject _exitKey;
    [SerializeField] private LevelBase _levelBase;

    public Transform Enter => _enterKey.transform;
    public Transform Exit => _exitKey.transform;
    public LevelBase Base => _levelBase;
}