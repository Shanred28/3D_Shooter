using UnityEngine;
using UnityEngine.UI;

public class UIQuestIndicator : MonoBehaviour
{
    [SerializeField] private QuestCollector _questCollector;
    [SerializeField] private Camera _camera;
    [SerializeField] private Image _imageTargetIndicatorQuest;

    private Transform _targetQuest;
    private void Start()
    {
        _questCollector.QuestReceived += OnQuestReceivedQuest;
        _questCollector.QuestCompleted += OnQuestCompleted;
    }

    private void Update()
    {
        if (_targetQuest == null) return;

        Vector3 pos = _camera.WorldToScreenPoint(_targetQuest.position);

        if (pos.z > 0)
        {
            if (pos.x < 0) pos.x = 0;
            if (pos.x > Screen.width) pos.x = Screen.width;

            if (pos.y < 0) pos.y = 0;
            if (pos.y > Screen.width) pos.y = Screen.width;
            _imageTargetIndicatorQuest.transform.position = pos;
        }             
    }

    private void OnDestroy()
    {
        _questCollector.QuestReceived -= OnQuestReceivedQuest;
        _questCollector.QuestCompleted -= OnQuestCompleted;
    }

    private void OnQuestReceivedQuest(Quest quest)
    {
        _imageTargetIndicatorQuest.gameObject.SetActive(true);
        _targetQuest = quest.ReachedPoint;
    }

    private void OnQuestCompleted(Quest quest)
    { 
       _imageTargetIndicatorQuest.gameObject.SetActive(false);
    }
}
