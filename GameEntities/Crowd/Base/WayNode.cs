using UnityEngine;

public class WayNode : ScriptableObject
{
    public Vector3 Position;

    public WayNode Initialize(Vector3 position)
    {
        Position = position;

        return this;
    }

    public void SetPosition(Vector3 newPosition, bool overrideY = false)
    {
        Position = new Vector3(newPosition.x, overrideY ? newPosition.y : Position.y, newPosition.z);
    }
}