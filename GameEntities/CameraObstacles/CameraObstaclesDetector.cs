using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraObstaclesDetector : MonoBehaviour
{
    [SerializeField] private Transform _rayDirectionTarget;

    private List<CameraObstacle> _obstacles = new();

    private void LateUpdate()
    {
        foreach(var obstacle in _obstacles)
        {
            obstacle.SetVisibleState(true);
        }

        _obstacles.Clear();

        var rayDirection = (_rayDirectionTarget.position - transform.position).normalized;
        var rayOrigin = transform.position - rayDirection * 100f;

        var ray = new Ray(rayOrigin, rayDirection);

        Debug.DrawRay(ray.origin, ray.direction * 500f);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            if (hitInfo.collider.TryGetComponent<CameraObstacle>(out var obstacle))
        
            {
                obstacle.SetVisibleState(false);
                _obstacles.Add(obstacle);
            }
        }
    }
}