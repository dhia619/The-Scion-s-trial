using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int coins;
    public int armor;
    [SerializeField] private int maxArmor;
    //[SerializeField] private TextMeshProUGUI totalCoins;
    [SerializeField] private Slider armorSlider;
    private Health playerHealth;

    void Start()
    {
        coins = 0;
        armor = 0;
        playerHealth = GetComponent<Health>();
    }

    void Update()
    {
        //totalCoins.SetText("x" + coins);
        armorSlider.value = (float) armor / maxArmor;
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

    public void TakeArmor(int amount)
    {
        armor = Mathf.Clamp(armor+amount, 0, maxArmor);
    }
    public void TakeDamage(int damage)
    {
        int rest = armor - damage;

        if (rest < 0)
        {
            armor = 0;
            playerHealth.TakeDamage(Mathf.Abs(rest));
        }

        else
        {
            armor -= damage;
            playerHealth.TakeDamage(0);
        }
    }

}
