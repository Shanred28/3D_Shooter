using UnityEngine;

public class Player : SingletonBase<Player>
{
    private bool _cardRed;
    public bool CardRed => _cardRed;

    public void AddCard()
    {
        _cardRed = true;
    }
}
