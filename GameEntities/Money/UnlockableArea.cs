using UnityEngine;

public class UnlockableArea : MonoBehaviour, IUnlockable, ISaveable<UnlockableAreaData>
{
    [SerializeField] private Transform _areaParentTransform;
    [SerializeField] private int _cost = 10;

    public int UclockCost => _cost;

    public SaveableEntityData<UnlockableAreaData> Data { get; set; }

    public void Start()
    {
        Data = new(gameObject.GetSavedDataKey());

        if (Data.Values.IsUnlocked)
        {
            _areaParentTransform.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void Unlock()
    {
        _areaParentTransform.gameObject.SetActive(true);

        IUnlockable.Unlocked?.Invoke(this);

        Data.Values.IsUnlocked = true;
        Data.Save();

        Destroy(gameObject);
    }
}

public class UnlockableAreaData : SaveableValues
{
    public bool IsUnlocked = false;
}