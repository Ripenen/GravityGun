using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovePath : MonoBehaviour
{
    [SerializeField] private List<Transform> _movePoints;

    public void Add(params Transform[] points) => _movePoints.AddRange(points);

    public Transform GetPoint()
    {
        if (PathIsEnd())
            return null;
        
        var result = _movePoints[0];
        
        _movePoints.RemoveAt(0);

        return result;
    }

    public bool PathIsEnd() => _movePoints.Count == 0;
    public Transform GetLastPoint() => _movePoints.Last();
}
