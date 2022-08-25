public interface IUpgrader<T> where T : System.Enum
{
    public event System.Action<T> CurrentUpgradeTypeChanged;
}