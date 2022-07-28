using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<LevelBasePrefab> _basePrefabs;
    [SerializeField] private LevelBasePrefab _bridge;
    [SerializeField] private Player _player;


    public void Generate(List<LevelBase> bases)
    {
        var firstLevel = SpawnRandomBase(_basePrefabs, _player.transform.position - Vector3.up, _player.transform.forward);
        var firstBridge = SpawnBridge(_bridge, firstLevel.Exit.position, firstLevel.Exit.forward);

        var secondLevel = SpawnRandomBase(_basePrefabs, firstBridge.Exit.position, firstBridge.Exit.forward);
        var secondBridge = SpawnBridge(_bridge, secondLevel.Exit.position, secondLevel.Exit.forward);
        
        var thirdLevel = SpawnRandomBase(_basePrefabs, secondBridge.Exit.position, secondBridge.Exit.forward);
        
        firstLevel.Base.AddPlayerPathPoint(firstLevel.Exit, firstBridge.Exit);
        bases.Add(firstLevel.Base);
        
        secondLevel.Base.AddPlayerPathPoint(secondLevel.Exit, secondBridge.Exit);
        bases.Add(secondLevel.Base);
        
        thirdLevel.Base.MakeEnd();
        bases.Add(thirdLevel.Base);
    }

    private LevelBasePrefab SpawnRandomBase(IReadOnlyList<LevelBasePrefab> bases, Vector3 enterPoint, Vector3 forward)
    {
        var prefab = bases[Random.Range(0, bases.Count)];
        
        var level = Instantiate(prefab);
        level.transform.rotation = Quaternion.LookRotation(forward);

        PlaceParentByChildren(level.gameObject, prefab.Enter, enterPoint);


        return level;
    }

    private LevelBasePrefab SpawnBridge(LevelBasePrefab bridgePrefab, Vector3 enterPoint, Vector3 forward)
    {
        var bridge = Instantiate(bridgePrefab);
        
        PlaceParentByChildren(bridge.gameObject, bridge.Enter, enterPoint);
        bridge.transform.rotation = Quaternion.LookRotation(forward);

        return bridge;
    }

    private void PlaceParentByChildren(GameObject parent, Transform childPosition, Vector3 point)
    {
        var offset = point - parent.transform.TransformPoint(childPosition.localPosition);
        parent.transform.position += offset;
    }
}