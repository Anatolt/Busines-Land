using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(IMoneyStack))]
public class MoneyStackerHelper : MonoBehaviour
{
    [SerializeField] private Money _moneyTemplate;

    private MoneyStacker _moneyStacker;

    private Stack<Money> _money = new();

    [SerializeField] private bool _enabled;
    [Space]
    [SerializeField] private Vector3 _base;
    [Space]
    [SerializeField] private Vector3 _raw;
    [SerializeField] private Vector3 _column;
    [SerializeField] private Vector3 _level;
    [Space]
    [Min(1)][SerializeField] private int _rawsInColumn = 1;
    [Min(1)][SerializeField] private int _columnsInLevel = 1;
    [Space]
    [Min(1)][SerializeField] private int _moneyNum = 10;

    private void Awake()
    {
        if (!Application.isPlaying)
            return;

        ResetChilds();

        var moneyStacker = GetComponent<IMoneyStack>();

        moneyStacker.MoneyStacker = new MoneyStacker(_raw, _column, _rawsInColumn, _base, _level, _columnsInLevel);

        Destroy(this);
    }

    private void Update()
    {
        if (!Application.isPlaying)
            Refresh();
    }

    [ContextMenu("Clear")]
    private void ResetChilds()
    {
        _money = new();

        foreach (var child in transform.GetComponentsInChildren<Money>())
            DestroyImmediate(child.gameObject);
    }

    public void Refresh()
    {
        if (!_enabled)
            return;

        _moneyStacker = new MoneyStacker(_raw, _column, _rawsInColumn, _base, _level, _columnsInLevel);

        while (_money.Count > 0)
            DestroyImmediate(_money.Pop().gameObject);

        _money = new();

        for (int i = 0; i < _moneyNum; i++)
        {
            var newMoney = Instantiate(_moneyTemplate, transform);
                
            newMoney.transform.localPosition = _moneyStacker.GetNextMoneyPosition(_money);
                
            _money.Push(newMoney);
        }
    }
}