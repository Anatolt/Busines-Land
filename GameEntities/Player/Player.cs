using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private PlayerMover _playerMover;
    [Space]
    [SerializeField] private MoneyBag _moneyBag;
    
    private void Update()
    {
        _playerMover.Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        _moneyBag.TryCollect(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _moneyBag.StopCollecting();
    }

    public void Upgrade(PlayerUpgradeType upgradeType, float value)
    {
        switch (upgradeType)
        {
            case PlayerUpgradeType.MovingSpeed:
                _playerMover.AddSpeed(value);
                break;

            case PlayerUpgradeType.CollectingSpeed:
                _moneyBag.AddCollectingSpeed((int)value);
                break;

            case PlayerUpgradeType.MaxMoney:
                _moneyBag.AddMaxMoneyAmount((int)value);
                break;
        }
    }
}