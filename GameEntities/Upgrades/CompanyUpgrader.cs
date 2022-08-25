using System;
using System.Collections.Generic;
using UnityEngine;

public class CompanyUpgrader : MonoBehaviour, IUnlockable, IUpgrader<CompanyUpgradeType>, ISaveable<CompanyUpgraderData>
{
    public event Action<CompanyUpgradeType> CurrentUpgradeTypeChanged;

    [SerializeField] private Company _linkedCompany;
    [Space]
    [SerializeField] private List<Upgrade> _upgrades;

    private int _currentUpgradeIndex = 0;

    private int _level = 1;

    public int UclockCost => _upgrades[_currentUpgradeIndex].Cost * _level;

    public SaveableEntityData<CompanyUpgraderData> Data { get; set; }

    [Serializable]
    private struct Upgrade
    {
        public CompanyUpgradeType Type;
        public int Value;
        public int Cost;
    }

    private void Start()
    {
        CurrentUpgradeTypeChanged?.Invoke(_upgrades[_currentUpgradeIndex].Type);

        Data = new(gameObject.GetSavedDataKey());

        if (Data.Values.Upgraded)
        {
            var upgradesCount = (Data.Values.Level - 1) * 2 + Data.Values.CurrentUpgradeIndex;

            for (int i = 0; i < upgradesCount; i++)
            {
                var unlockedUpgrade = _upgrades[_currentUpgradeIndex];

                _linkedCompany.Upgrade(unlockedUpgrade.Type, unlockedUpgrade.Value, _level, false);

                _currentUpgradeIndex = (_currentUpgradeIndex + 1) % _upgrades.Count;

                if (_currentUpgradeIndex == 0)
                    _level += 1;

                CurrentUpgradeTypeChanged?.Invoke(_upgrades[_currentUpgradeIndex].Type);
            }
        }
    }

    [ContextMenu("Unlock current upgrade")]
    public void Unlock()
    {
        var unlockedUpgrade = _upgrades[_currentUpgradeIndex];

        _linkedCompany.Upgrade(unlockedUpgrade.Type, unlockedUpgrade.Value, (_level - 1) * 2 + _currentUpgradeIndex);

        _currentUpgradeIndex = (_currentUpgradeIndex + 1) % _upgrades.Count;

        if (_currentUpgradeIndex == 0)
            _level += 1;

        CurrentUpgradeTypeChanged?.Invoke(_upgrades[_currentUpgradeIndex].Type);

        IUnlockable.Unlocked?.Invoke(this);

        Data.Values.CurrentUpgradeIndex = _currentUpgradeIndex;
        Data.Values.Level = _level;
        
        Data.Save();
    }
}

public enum CompanyUpgradeType { MaxMoney, MoneySpawningSpeed }

public class CompanyUpgraderData : SaveableValues
{
    public int CurrentUpgradeIndex = 0;
    public int Level = 0;
    public bool Upgraded => CurrentUpgradeIndex != 0 || Level != 0;
}