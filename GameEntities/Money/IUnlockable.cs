public interface IUnlockable
{
    public static System.Action<IUnlockable> Unlocked;

    public int UclockCost { get; }

    public void Unlock();
}