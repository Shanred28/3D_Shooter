using UnityEngine;
using UnityEngine.UI;

public class UI_IndicatorVisable : MonoBehaviour
{
    [SerializeField] private Image _indicatorVisableImage;
    [SerializeField] private Image _indicatorVisableColorAlarmImage;
    [SerializeField] private SpaceSoldier _soldier;

    private Timer timer;
    [SerializeField] private float timeDisableIndecator;
    private void Start()
    {
        _indicatorVisableImage.gameObject.SetActive(false);
        timer = Timer.CreateTimer(timeDisableIndecator);
    }

    public void Update()
    {
        if (timer.IsComplited)
        {
            _indicatorVisableImage.gameObject.SetActive(false);
            timer.Restart(timeDisableIndecator);
        }
    }

    public void Detected()
    {
        timer.Restart(timeDisableIndecator);
        timer.Play();
        _indicatorVisableImage.gameObject.SetActive(true);
        _indicatorVisableColorAlarmImage.color = Color.red;
        _indicatorVisableImage.fillAmount = 1;
    }
    public void MidleDetected()
    {
        timer.Restart(timeDisableIndecator);
        timer.Play();
        _indicatorVisableImage.gameObject.SetActive(true);
        _indicatorVisableColorAlarmImage.color = Color.yellow;
        _indicatorVisableImage.fillAmount = 1;
        
    }
    public void NonDetected()
    {
        timer.Stop();
        _indicatorVisableImage.gameObject.SetActive(false);
        _indicatorVisableColorAlarmImage.color = Color.grey;
    }

    public void SetAllarmFill(float currentTime, float timeAmount)
    {
        timer.Restart(timeDisableIndecator);
        timer.Play();
        _indicatorVisableImage.gameObject.SetActive(true);
        _indicatorVisableColorAlarmImage.color = Color.yellow;
        _indicatorVisableImage.fillAmount = 1 - currentTime / timeAmount;
    }
}
