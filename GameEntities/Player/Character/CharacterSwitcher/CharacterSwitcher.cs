using System;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterSwitcher : Singleton<CharacterSwitcher>
{
    public event Action<Character> CharacterChanged;

    [SerializeField] private Character[] _characters;
    [Space] 
    [SerializeField] private MoneyBag _moneyBag;

    private IConstraint[] _moneyBagConstraints;

    private void Awake()
    {
        _moneyBagConstraints = _moneyBag.GetComponents<IConstraint>();
    }

    public void Switch(CharacterType characterType)
    {
        foreach (Character character in _characters)
        {
            var isNeededCharacter = character.Type == characterType;

            character.gameObject.SetActive(isNeededCharacter);

            if (isNeededCharacter)
                RefreshMoneyBagConstraints(character);
        }
    }

    private void RefreshMoneyBagConstraints(Character character)
    {
        foreach (var constraints in _moneyBagConstraints)
            constraints.SetSource(0, new ConstraintSource { sourceTransform = character.MoneyBagTransformTarget, weight = 1f });
    }

    [Header("Test")]
    [SerializeField] private CharacterType _testCharacterType;
    [ContextMenu("Test switch")]
    private void TestSwitch()
    {
        Switch(_testCharacterType);
    }
}