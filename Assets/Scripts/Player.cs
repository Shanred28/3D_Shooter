using UnityEngine;

public class Player : SingletonBase<Player>
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private UI_IndicatorVisable _indicatorVisable;
    public UI_IndicatorVisable IndicatorVisable => _indicatorVisable;

    [SerializeField] private Camera m_Camera;
    public Camera MainCamera => m_Camera;

    [SerializeField] private Transform _aim;
    public Transform Aim => _aim;
    private bool _cardRed;
    public bool CardRed => _cardRed;
    private bool _cardBlue;
    public bool CardBlue => _cardBlue;

    private bool _cardYellow;
    public bool CardYellow => _cardYellow;

    public void AddCardRed()
    {
        _cardRed = true;
    }

    public void AddCardBlue()
    {
        _cardBlue = true;
    }

    public void AddCardYellow()
    {
        _cardYellow = true;
    }

    public void AddEnergy()
    {
        _weapon.AddFullEnergy();
    }
}
