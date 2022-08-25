using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour, IMoneyStack
{
    public event Action<MoneyBagEventArgs> Changed;
    
    [SerializeField] private int _maxMoney = 10;

    private int _moneyCollectingSpeed = 10;

    public Stack<Money> Money { get; set; } = new();
    public MoneyStacker MoneyStacker { get; set; }

    public bool IsFilled => Money.Count >= _maxMoney;
    public bool IsEmpty => Money.Count == 0;

    private WaitForSeconds _moneyCollectingCooldown => new WaitForSeconds(1f / _moneyCollectingSpeed);


    private void Start()
    {
        InvokeChangedEvent();
    }

    public void TryCollect(Collider collider)
    {
        if (collider.TryGetComponent<CompanyMoneyBank>(out var moneyBank))
        {
            StartCollectingMoney(moneyBank);
        }

        if (collider.TryGetComponent<Unlocker>(out var opener))
        {
            StartPuttingMoney(opener);
        }

        if (collider.TryGetComponent<RoadMoney>(out var roadMoney))
        {
            if (!IsFilled)
            {
                PutIn(roadMoney);
                roadMoney.DestroyCollider();
            }
        }
    }

    public void StopCollecting()
    {
        StopAllCoroutines();
    }

    public void AddCollectingSpeed(int value)
    {
        _moneyCollectingSpeed += value;
    }

    public void AddMaxMoneyAmount(int value)
    {
        _maxMoney += value;

        InvokeChangedEvent();
    }

    private void StartCollectingMoney(CompanyMoneyBank moneyBank)
    {
        StartCoroutine(CollectingMoneyCoroutine(moneyBank));
    }
    private void StartPuttingMoney(Unlocker Unlocker)
    {
        StartCoroutine(PuttingMoneyCoroutine(Unlocker));
    }

    private void PutIn(Money money)
    {
        money.PlayCollectAnimation(transform, MoneyStacker.GetNextMoneyPosition(Money));
        Money.Push(money);

        InvokeChangedEvent();

        AudioPlayer.Instance.PlayMoneyCollectSound();
    }

    private Money PullOut()
    {
        var pulledMoney = Money.Pop();

        InvokeChangedEvent();

        AudioPlayer.Instance.PlayMoneyCollectSound();

        return pulledMoney;
    }


    private void InvokeChangedEvent()
    {
        Changed.Invoke(new MoneyBagEventArgs(Money.Count, _maxMoney));
    }

    private IEnumerator CollectingMoneyCoroutine(CompanyMoneyBank moneyBank)
    {
        while (!IsFilled)
        {
            if (moneyBank.TryGetMoney(out var money))
                PutIn(money);

            yield return _moneyCollectingCooldown;
        }
    }

    private IEnumerator PuttingMoneyCoroutine(Unlocker unlocker)
    {
        while (!IsEmpty && !unlocker.IsMoneyTargetReached)
        {
            unlocker.PutMoney(PullOut());

            yield return _moneyCollectingCooldown;
        }
    }
}

public class MoneyBagEventArgs 
{
    public MoneyBagEventArgs(int currentMoney, int maxMoney)
    {
        CurrentMoney = currentMoney;
        MaxMoney = maxMoney;
    }

    public int CurrentMoney { get; set; }
    public int MaxMoney { get; set; }
}