using UnityEngine;

public class SpaceSoldier : Destructible
{
    [SerializeField] private EntityAnimationAction _actionDeath;
    [SerializeField] private float _damageFallFactor;
    [SerializeField] private QuestCollector questCollector;

    private CharacterMovement _characterMovement;

    protected override void Start()
    {
        base.Start();
        _characterMovement = GetComponent<CharacterMovement>();
        _characterMovement.Land += OnLand;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _characterMovement.Land -= OnLand;
    }

    protected override void OnDeath()
    {
        _characterMovement.Land -= OnLand;
        EventOnDeath?.Invoke();
        _actionDeath.StartAction();
    }

    public void FallHightDead()
    {
        OnDeath();
    }

    private void OnLand(Vector3 vel)
    {
        if (Mathf.Abs( vel.y) < 6) return;
        ApplyDamage((int)(Mathf.Abs(vel.y) * _damageFallFactor),this);
    }

    [System.Serializable]
    public class SpaceSoldre
    {
        public Vector3 position;
        public int hitPoins;
        public Quest quest;


        public SpaceSoldre() { }
    }

    public override string SerializeState()
    {
        SpaceSoldre s = new SpaceSoldre();

        s.position = transform.position;
        s.hitPoins = CurrentHitPoints;
        s.quest = questCollector.CurrentQuest;

        return JsonUtility.ToJson(s);
    }

    public override void DeserializeState(string state)
    {
        SpaceSoldre s = JsonUtility.FromJson<SpaceSoldre>(state);

        transform.position = s.position;
        SetHitPoint(s.hitPoins);
        questCollector.AssignQuest(s.quest);
    }
}
