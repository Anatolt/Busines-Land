using System;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : MonoBehaviour, IMoneyStack, ISaveable<UnlockerData>
{
    public event Action<UnlockerEventArgs> ProgressUpdated;

    [SerializeField] private MonoBehaviour _linkedUnlockableEntity; // IUnlockable
    [Space]
    [SerializeField] private bool _destroyAfterUnlcock = true;

    private int _moneyTarget;

    public Stack<Money> Money { get; set; } = new();
    public MoneyStacker MoneyStacker { get; set; }
    public bool IsMoneyTargetReached { get; private set; } = false;

    private IUnlockable _linkedUnlockable => _linkedUnlockableEntity as IUnlockable;
    private int CurrentMoney => Money.Count;

    public SaveableEntityData<UnlockerData> Data { get; set; }

    [SerializeField] private Money _moneyTemplate;

    private void Start()
    {
        _moneyTarget = _linkedUnlockable.UclockCost;

        Data = new(gameObject.GetSavedDataKey());

        if (Data.Values.CurrentMoney != 0 && Data.Values.CurrentMoney < _moneyTarget)
        {
            for (int i = 0; i < Data.Values.CurrentMoney; i++)
            {
                var newMoney = Instantiate(_moneyTemplate, transform);
                newMoney.transform.localPosition = MoneyStacker.GetNextMoneyPosition(Money);

                Money.Push(newMoney);
            }
        }

        UpdateProgress();
    }

    public void PutMoney(Money money)
    {
        money.PlayCollectAnimation(transform, MoneyStacker.GetNextMoneyPosition(Money));
        Money.Push(money);

        UpdateProgress();
    }

    private void UpdateProgress()
    {
        ProgressUpdated?.Invoke(new UnlockerEventArgs(_moneyTarget, CurrentMoney));

        Data.Values.CurrentMoney = CurrentMoney;
        Data.Save();

        if (_moneyTarget <= CurrentMoney)
            UnlockLinkedEntity();
    }

    private void UnlockLinkedEntity()
    {
        _linkedUnlockable.Unlock();

        if (_destroyAfterUnlcock)
        {
            IsMoneyTargetReached = true;
            Destroy(gameObject);
        }

        else
        {
            RefreshMoneyTarget(_linkedUnlockable.UclockCost);
        }
    }

    private void RefreshMoneyTarget(int newMoneyTarget)
    {
        _moneyTarget = newMoneyTarget;

        while (CurrentMoney > 0)
            Destroy(Money.Pop().gameObject);

        UpdateProgress();
    }
}

public class UnlockerData : SaveableValues
{
    public int CurrentMoney = 0;
}

public class UnlockerEventArgs
{
    public UnlockerEventArgs(int unlockCost, int currentProgress)
    {
        UnlockCost = unlockCost;
        CurrentProgress = currentProgress;
    }

    public int UnlockCost { get; set; }
    public int CurrentProgress { get; set; }
}