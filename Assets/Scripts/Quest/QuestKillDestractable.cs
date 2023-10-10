using UnityEngine;

public class QuestKillDestractable : Quest
{
    [SerializeField] private Destructible[] _destructibles;

    private int AmountDestructableDead = 0;

    private void Start()
    {
        if (_destructibles == null) return;

        for (int i = 0; i < _destructibles.Length; i++)
        {
            _destructibles[i].EventOnDeath.AddListener(OnDestructable);
        }
    }

    private void OnDestructable()
    {
        AmountDestructableDead++;

        if (AmountDestructableDead == _destructibles.Length)
        {
            for (int i = 0; i < _destructibles.Length; i++)
            {
                if(_destructibles[i] != null)
                   _destructibles[i].EventOnDeath.RemoveListener(OnDestructable);
            }

            Complited?.Invoke();
        }
    }
}
