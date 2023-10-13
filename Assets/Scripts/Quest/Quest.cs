using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    public UnityAction Complited;

    [SerializeField] protected GameObject _owner;
    [SerializeField] private Quest _nextQuest;

    public Quest NextQuest => _nextQuest;

    [SerializeField] private QuestProperties _propertiesQuest;

    public QuestProperties PropertiesQuest => _propertiesQuest;

    [SerializeField] private Transform _reachedPoint;
    public Transform ReachedPoint => _reachedPoint;

    private void Update()
    {
        UpdateCompletedCondition();
    }

    protected virtual void UpdateCompletedCondition() { }

    public void SetOwner()
    {
        _owner = Player.Instance.gameObject;
    }
}
