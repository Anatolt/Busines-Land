using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyStacker
{
    private readonly Vector3 _basePosition;

    private readonly Vector3 _rawOffset;
    private readonly Vector3 _columnOffset;
    private readonly Vector3 _levelOffset;

    private readonly int _numberOfMoneyInColumn;
    private readonly int _numberOfColumnsInLayer;

    public MoneyStacker(Vector3 rawOffset, Vector3 columnOffset, int numberOfMoneyInColumn, 
        Vector3 basePosition = new Vector3(), Vector3 levelOffset = new Vector3(), int numberOfColumns = 0)
    {
        _basePosition = basePosition;

        _rawOffset = rawOffset;
        _columnOffset = columnOffset;
        _levelOffset = levelOffset;

        _numberOfMoneyInColumn = numberOfMoneyInColumn;
        _numberOfColumnsInLayer = numberOfColumns;
    }

    public Vector3 GetNextMoneyPosition(IEnumerable<Money> moneyList)
    {
        var moneyCount = moneyList.Count();

        var numberOfLevels = moneyCount / _numberOfMoneyInColumn / _numberOfColumnsInLayer;
        var numberOfColumns = moneyCount % _numberOfMoneyInColumn;
        var numberOfRaws = (moneyCount - _numberOfMoneyInColumn * _numberOfColumnsInLayer * numberOfLevels) / _numberOfMoneyInColumn;

        var rawOffset = _rawOffset * numberOfRaws;
        var columnOffset = _columnOffset * numberOfColumns;
        var levelOffset = _levelOffset * numberOfLevels;

        return _basePosition + rawOffset + columnOffset + levelOffset;
    }
}