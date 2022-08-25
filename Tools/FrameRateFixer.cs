using UnityEngine;

public static class FrameRateFixer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Fix()
    {
        Application.targetFrameRate = 90;
    }
}