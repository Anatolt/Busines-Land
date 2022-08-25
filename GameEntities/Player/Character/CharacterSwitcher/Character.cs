using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterType _type;
    [Space]
    [SerializeField] private Transform _moneyBagTransformTarget;

    public CharacterType Type => _type;
    public Transform MoneyBagTransformTarget => _moneyBagTransformTarget;
}