using UnityEngine;
using UnityEngine.UI;

public class UIHpSlider : MonoBehaviour
{
    [SerializeField] private Destructible player;
    [SerializeField] private Slider hpBarSlider;

    private void Start()
    {
        hpBarSlider.maxValue = player.MaxHitPoints;
        hpBarSlider.value = player.MaxHitPoints;
        player.ChangeHp.AddListener(UpdateHP);
    }

    private void OnDestroy()
    {
        player.ChangeHp.RemoveListener(UpdateHP);
    }

    private void UpdateHP()
    {
        hpBarSlider.value = player.CurrentHitPoints;
    }
}
