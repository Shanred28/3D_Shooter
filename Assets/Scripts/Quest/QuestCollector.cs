using UnityEngine;
using UnityEngine.Events;

public class QuestCollector : MonoBehaviour
{
    public UnityAction<Quest> QuestReceived;
    public UnityAction<Quest> QuestCompleted;
    public UnityAction LastQuestCompleted;

    [SerializeField] private Quest _currentQuest;
    public Quest CurrentQuest => _currentQuest;

    [SerializeField] private Quest[] _allQuest;

    private void Start()
    {
        if (_currentQuest != null)
        {
            AssignQuest(_currentQuest);
        }         
    }

    public void AssignQuest(Quest quest)
    {
        _currentQuest = quest;

        if (QuestReceived != null)
            QuestReceived.Invoke(_currentQuest);
        _currentQuest.SetOwner();

        _currentQuest.Complited += OnQuestCompleted;
    }

    private void OnQuestCompleted()
    {
        _currentQuest.Complited -= OnQuestCompleted;

        if(QuestCompleted != null)
           QuestCompleted.Invoke(_currentQuest);

        if (_currentQuest.NextQuest != null)
            AssignQuest(_currentQuest.NextQuest);
        else
        {
            if(LastQuestCompleted != null)
                LastQuestCompleted.Invoke();
        }          
    }

}
