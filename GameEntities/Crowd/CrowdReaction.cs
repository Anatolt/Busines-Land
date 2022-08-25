using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crowd))]
public class CrowdReaction : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _reactionsTemplates;
    [Space]
    [SerializeField] private Vector3 _spawnOffset;

    private CrowdAgent[] _agents;

    private void Start()
    {
        _agents = GetComponentsInChildren<CrowdAgent>();

        IUnlockable.Unlocked += React;
    }
    private void OnDisable()
    {
        IUnlockable.Unlocked -= React;
    }

    private void React(IUnlockable unlockable)
    {
        var someAgents = _agents;

        foreach (var agent in someAgents)
        {
            var particles = Instantiate(_reactionsTemplates.PickRandom(), agent.transform);
            
            particles.transform.localPosition = _spawnOffset;

            Destroy(particles, particles.main.duration + 3);
        }
    }
}