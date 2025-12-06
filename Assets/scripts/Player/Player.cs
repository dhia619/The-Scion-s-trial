using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int coins;
    [SerializeField] private TextMeshProUGUI totalCoins;
    void Start()
    {
        coins = 0;
    }

    void Update()
    {
        totalCoins.SetText("x" + coins);
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetCoins(int amount)
    {
        coins = amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

}
