using UnityEngine;

public class Player : SingletonBase<Player>
{
    [SerializeField] private Weapon _weapon;
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
