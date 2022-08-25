using System.Collections.Generic;

internal interface IMoneyStack
{
    public Stack<Money> Money { get; set; }
    public MoneyStacker MoneyStacker { get; set; }
}