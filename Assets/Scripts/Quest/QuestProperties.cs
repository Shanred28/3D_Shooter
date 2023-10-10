using UnityEngine;

[CreateAssetMenu]
public class QuestProperties : ScriptableObject
{
    [TextArea]
    [SerializeField] private string _descriptionQuest;

    public string DescriptionQuest => _descriptionQuest;

    [TextArea]
    [SerializeField] private string _taskQuest;

    public string TaskQuest => _taskQuest;
}
