using UnityEngine;

public class SpaceSoldier : Destructible
{
    [SerializeField] private EntityAnimationAction actionDeath;

    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
        actionDeath.StartAction();
        print("sdf");
    }

    public void FallHightDead()
    {
        OnDeath();
    }
}
