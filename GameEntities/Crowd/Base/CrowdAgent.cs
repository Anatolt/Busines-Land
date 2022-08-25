using UnityEngine;

public abstract class CrowdAgent : MonoBehaviour
{
    [SerializeField] private Crowd _linkedCrowd;

    private WayNode _targetWayNode;

    protected Vector3 _targetWayNodePosition => _targetWayNode.Position;

    protected void Start()
    {
        _targetWayNode = _linkedCrowd.WayNodes.PickRandom();
        transform.position = _targetWayNode.Position;

        SetNextNode();
    }
    
    protected void SetNextNode()
    {
        _targetWayNode = _linkedCrowd.GetRandomNeighboringNodes(_targetWayNode);
    }
}