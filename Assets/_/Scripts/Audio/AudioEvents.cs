using System;

public static class AudioEvents
{
    public static event Action OnPlayBallHitSound;
    public static event Action OnPlayBlockDestructionSound;

    public static void PlayBallHitSound()
    {
        OnPlayBallHitSound?.Invoke();
    }

    public static void PlayBlockDestructionSound()
    {
        OnPlayBlockDestructionSound?.Invoke();
    }
}