using UnityEngine;

public class MoneyParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlesTemplate;

    [SerializeField] private Vector3 _spawnOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Money>(out var money))
        {
            var newParticles = Instantiate(_particlesTemplate, money.transform.position + _spawnOffset, Quaternion.identity);

            newParticles.Play();
            Destroy(newParticles.gameObject, _particlesTemplate.main.duration);
        }
    }
}