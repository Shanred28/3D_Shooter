using UnityEngine;

public class AlienSoldier : Destructible
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private SpreadShootRig _spreadShootRig;

    [SerializeField] private AiAlienSoldier _aiAlienSoldier;
    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }

    public void Fire(Vector3 target)
    { 
        if(_weapon.CanFire == false) return;

        _weapon.FirePointLookAt(target);
        
        _weapon.Fire();

        _spreadShootRig.Spread();
    }

    [System.Serializable]
    public class AIAlienState
    {
        public Vector3 position;
        public int hitPoins;
        public int behaviour;

        public AIAlienState() { }
    }

    public override string SerializeState()
    {
        AIAlienState s = new AIAlienState();

        s.position = transform.position;
        s.hitPoins = CurrentHitPoints;
        s.behaviour = (int)_aiAlienSoldier.AiBehaviour;

        return JsonUtility.ToJson(s);
    }

    public override void DeserializeState(string state)
    {
        AIAlienState s = JsonUtility.FromJson<AIAlienState>(state);

        _aiAlienSoldier.SetPosition(s.position);
        SetHitPoint(s.hitPoins);
        _aiAlienSoldier.AiBehaviour =(AiAlienSoldier.AIBehaviour) s.behaviour;
    }
}
