using System;
using System.Collections.Generic;
using UnityEngine;

public class CompanyMoneyBank : MonoBehaviour, IMoneyStack
{
    public event Action<int> MoneyAmountChanged; 

    [SerializeField] private Money _moneyTemplate;

    public Stack<Money> Money { get; set; } = new();
    public MoneyStacker MoneyStacker { get; set; }

    public bool TryGetMoney(out Money money)
    {
        if (Money.Count > 0)
        {
            money = Money.Pop();
            
            MoneyAmountChanged?.Invoke(Money.Count);
            
            return true;
        }

        money = null;

        MoneyAmountChanged?.Invoke(Money.Count);

        return false;
    }

    public void SpawnMoney()
    {
        var newMoney = Instantiate(_moneyTemplate, transform);

        newMoney.transform.localPosition = MoneyStacker.GetNextMoneyPosition(Money);
        Money.Push(newMoney);

        MoneyAmountChanged?.Invoke(Money.Count);
    }
}