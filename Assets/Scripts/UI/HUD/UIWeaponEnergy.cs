using UnityEngine;
using UnityEngine.UI;

public class UIWeaponEnergy : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Slider slider;
    [SerializeField] private Image[] images;

    private void Start()
    {
        slider.maxValue = weapon.PrimaryMaxEnergy;
    }

    private void Update()
    {
        slider.value = weapon.CurrentPrimaryEnergy;

        SetActiveImage(weapon.CurrentPrimaryEnergy != weapon.PrimaryMaxEnergy);
    }

    private void SetActiveImage(bool active)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = active;
        }
    }
}
