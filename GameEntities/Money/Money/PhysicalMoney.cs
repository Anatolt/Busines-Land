using UnityEngine;

public class PhysicalMoney : RoadMoney
{
    private Rigidbody _rigidBody;

    private void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _) || collision.gameObject.TryGetComponent<Player>(out _))
        {
            _collider.isTrigger = true;
            _rigidBody.isKinematic = true;
        }
    }

    public void AddForce(float force, Vector3 direction) => _rigidBody.AddForce(direction * force, ForceMode.Impulse);
}