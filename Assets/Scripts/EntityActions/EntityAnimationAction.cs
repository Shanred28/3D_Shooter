using UnityEngine;

public class EntityAnimationAction : EntityAction
{
    [SerializeField] private Animator animator;

    [SerializeField] private string actionAnimationName;

    [SerializeField] private float timeDuration;

    private Timer _timer;
    protected bool isPlayingAnimation;

    public override void StartAction()
    {
        base.StartAction();

        animator.CrossFade(actionAnimationName, timeDuration);

        _timer = Timer.CreateTimer(timeDuration, true);
        _timer.OnTick += OnTimerTick;
    }

    private void OnDestroy()
    {
        if(_timer != null)
           _timer.OnTick -= OnTimerTick;
    }

    public override void EndAction()
    {
        _timer.OnTick -= OnTimerTick;
        base.EndAction();       
    }

    protected override void EndActionByAnimation()
    {
        _timer.OnTick -= OnTimerTick;
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
