using TMPro;
using UnityEngine;

public class UIQuestInfo : MonoBehaviour
{

    [SerializeField] private QuestCollector _questCollector;
    [SerializeField] private TMP_Text _descriptionQuestText;
    [SerializeField] private TMP_Text _taskQuestText;

    private void Start()
    {
        _questCollector.QuestReceived += OnQuestReceived;
        _questCollector.QuestCompleted += OnQuestCompleted;
    }

    private void OnDestroy()
    {
        _questCollector.QuestReceived -= OnQuestReceived;
        _questCollector.QuestCompleted -= OnQuestCompleted;
    }


    private void OnQuestReceived(Quest quest)
    {
        _descriptionQuestText.gameObject.SetActive(true);
        _taskQuestText.gameObject.SetActive(true);

        _descriptionQuestText.text = quest.PropertiesQuest.DescriptionQuest;
        _taskQuestText.text = quest.PropertiesQuest.TaskQuest;
    }

    private void OnQuestCompleted(Quest quest)
    {
        _descriptionQuestText.gameObject.SetActive(false);
        _taskQuestText.gameObject.SetActive(false);
    }
}
