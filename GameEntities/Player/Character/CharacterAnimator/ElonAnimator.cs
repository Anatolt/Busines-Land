public class ElonAnimator : CharacterAnimator
{
    public void OnStep()
    {
        AudioPlayer.Instance.PlayStepSound();
    }
}