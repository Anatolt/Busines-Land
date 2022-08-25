using System;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour, IUnlockable, IUpgrader<PlayerUpgradeType>, ISaveable<PlayerUpgradeData>
{
    [SerializeField] private int _cost = 10;
    [Space]
    [SerializeField] private PlayerUpgradeType _upgradeType;
    [SerializeField] private float _upgradeValue;

    public int UclockCost => _cost;

    public SaveableEntityData<PlayerUpgradeData> Data { get; set; }

    public event Action<PlayerUpgradeType> CurrentUpgradeTypeChanged;

    private void Start()
    {
        CurrentUpgradeTypeChanged?.Invoke(_upgradeType);

        Data = new(gameObject.GetSavedDataKey());

        if (Data.Values.IsUnlocked)
        {
            Player.Instance.Upgrade(_upgradeType, _upgradeValue);

            if (_upgradeType == PlayerUpgradeType.MovingSpeed)
                CharacterSwitcher.Instance.Switch(CharacterType.ElonGyro);

            Destroy(gameObject);
        }
    }

    public void Unlock()
    {
        Player.Instance.Upgrade(_upgradeType, _upgradeValue);

        IUnlockable.Unlocked?.Invoke(this);

        Data.Values.IsUnlocked = true;
        Data.Save();

        if (_upgradeType == PlayerUpgradeType.MovingSpeed)
            CharacterSwitcher.Instance.Switch(CharacterType.ElonGyro);

        Destroy(gameObject);
    }
}

public enum PlayerUpgradeType { MovingSpeed, CollectingSpeed, MaxMoney }

[System.Serializable]
public class PlayerUpgradeData : SaveableValues
{
    public bool IsUnlocked = false;
}