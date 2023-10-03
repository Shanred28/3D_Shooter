using UnityEngine;

public class EntityAnimationAction : EntityAction
{
    [SerializeField] private Animator animator;

    [SerializeField] private string actionAnimationName;

    [SerializeField] private float timeDuration;

    private Timer timer;
    protected bool isPlayingAnimation;

    public override void StartAction()
    {
        base.StartAction();

        animator.CrossFade(actionAnimationName, timeDuration);

        timer = Timer.CreateTimer(timeDuration, true);
        timer.OnTick += OnTimerTick;
    }

    public override void EndAction()
    {
        timer.OnTick -= OnTimerTick;
        base.EndAction();       
    }

    protected override void EndActionByAnimation()
    {
        timer.OnTick -= OnTimerTick;
        base.EndActionByAnimation();
    }

    private void OnTimerTick()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(actionAnimationName) == true)
        {
            isPlayingAnimation = true;
        }

        if (isPlayingAnimation == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(actionAnimationName) == false)
            {
                isPlayingAnimation = false;
                EndActionByAnimation();
            }
        }
    }
}
