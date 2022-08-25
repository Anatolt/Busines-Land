using UnityEngine;

public class RoadMoney : Money
{
    [SerializeField] protected Collider _collider;

    public virtual void DestroyCollider() => Destroy(_collider);
}