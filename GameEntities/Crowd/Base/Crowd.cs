using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    [SerializeField] private List<WayNode> _wayNodes;

    public List<WayNode> WayNodes => _wayNodes;

    public WayNode GetRandomNeighboringNodes(WayNode rootNode)
    {
        var rootNodeIndex = _wayNodes.IndexOf(rootNode);

        var possibleNodes = new List<WayNode>()
        {
            _wayNodes[(rootNodeIndex + 1) % _wayNodes.Count],
            _wayNodes[(rootNodeIndex - 1) < 0 ? _wayNodes.Count - 1 : (rootNodeIndex - 1) % _wayNodes.Count]
        };

        return possibleNodes.PickRandom();
    }

    public void AddNewNode()
    {
        if (_wayNodes.Count == 0)
            _wayNodes.Add(CreateNode(Vector3.zero));

        else
            _wayNodes.Add(CreateNode(_wayNodes[_wayNodes.Count - 1].Position));

        WayNode CreateNode(Vector3 position)
        {
            position = new Vector3(position.y, transform.position.y, position.z);
            return ScriptableObject.CreateInstance<WayNode>().Initialize(position);
        }
    }
}