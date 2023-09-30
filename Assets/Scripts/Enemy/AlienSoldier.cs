using UnityEngine;

public class AlienSoldier : Destructible
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private SpreadShootRig _spreadShootRig;

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
}
