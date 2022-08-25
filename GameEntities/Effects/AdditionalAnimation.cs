using UnityEngine;

public abstract class AdditionalAnimation : MonoBehaviour
{
    public abstract float Duration { get; }

    public bool Enabled { get; protected set; }

    public abstract void Play();
}