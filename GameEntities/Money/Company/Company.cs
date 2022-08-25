using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Company : MonoBehaviour, IUnlockable, ISaveable<CompanyData>
{
    public class CompanyStatsEventArgs : EventArgs
    { 
        public int Level { get; set; }
        public int MoneyPerSecond { get; set; }
        public int MaxMoney { get; set; }
    }

    public event EventHandler<CompanyStatsEventArgs> CompanyStatsChanged;

    [SerializeField] private int _cost = 10;
    [Space]
    [SerializeField] [Min(1)] private int _moneyPerSecond = 2;
    [SerializeField] [Min(10)] private int _maxMoney = 30;
    [Space]
    [SerializeField] private CompanyMoneyBank _moneyBank;
    [SerializeField] private GameObject _openedCompanyGameObject;
    [SerializeField] private GameObject _unlockerGameObject;
    [Space]
    [SerializeField] private Transform _modelTransform;
    [SerializeField] private ParticleSystem _moneyParticles;
    [SerializeField] private Camera _eventCamera;

    [Space]
    [SerializeField] private AdditionalAnimation _additionalUpgradeAnimation;

    private CompanyAnimator _animator;

    public int UclockCost => _cost;

    public SaveableEntityData<CompanyData> Data { get; set; }

    private void Start()
    {
        _animator = new CompanyAnimator(_modelTransform, _additionalUpgradeAnimation);

        Data = new(gameObject.GetSavedDataKey());

        if (Data.Values.IsUnlocked)
        {
            _openedCompanyGameObject.SetActive(true);
            _unlockerGameObject.SetActive(false);

            StartCoroutine(SpawningMoneyCoroutine());
        }
    }
    public void Unlock()
    {
        _openedCompanyGameObject.SetActive(true);

        StartCoroutine(SpawningMoneyCoroutine());

        IUnlockable.Unlocked?.Invoke(this);

        CameraSwitcher.Instance.PlayEventAnimation(_eventCamera, _animator.BuildAnimationDuration);
        _animator.PlayBuildAnimation();

        Data.Values.IsUnlocked = true;
        Data.Save();

        CompanyStatsChanged?.Invoke(this, new CompanyStatsEventArgs
        {
            MaxMoney = _maxMoney,
            MoneyPerSecond = _moneyPerSecond,
            Level = 0,
        });
    }
    public void Upgrade(CompanyUpgradeType upgradeType, int value, int level, bool animations = true)
    {
        switch (upgradeType)
        {
            case CompanyUpgradeType.MoneySpawningSpeed:
                _moneyPerSecond += value;
                break;

            case CompanyUpgradeType.MaxMoney:
                _maxMoney += value;
                break;
        }

        CompanyStatsChanged?.Invoke(this, new CompanyStatsEventArgs
        {
            MaxMoney = _maxMoney,
            MoneyPerSecond = _moneyPerSecond,
            Level = level,
        });

        if (animations)
        {
            _animator.PlayUpgradeAnimation(_moneyParticles);

            CameraSwitcher.Instance.PlayEventAnimation(_eventCamera, _animator.UpgradeAnimationDuration);
        }
    }
    private IEnumerator SpawningMoneyCoroutine()
    {
        while (true)
        {
            if (_moneyBank.Money.Count >= _maxMoney)
            {
                yield return null;

                continue;
            }

            _moneyBank.SpawnMoney();
            yield return new WaitForSeconds(1f / _moneyPerSecond);
        }
    }
}

public class CompanyData : SaveableValues
{
    public bool IsUnlocked = false;
}