using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int coins;
    public int armor;
    [SerializeField] private int maxArmor;
    [SerializeField] private Slider armorSlider;
    private Health playerHealth;
    public Vector3 checkpoint;
    [SerializeField] public AudioClip deathSound;

    public bool hasKey;

    void Start()
    {
        checkpoint = transform.position;
        armor = 100;
        playerHealth = GetComponent<Health>();
        DialogueManager.Instance.ShowDialogue("Hi! Welcome to the dungeon. Find the key to unlock the exit door and escape. Good luck!");
    }

    void Update()
    {
        armorSlider.value = (float) armor / maxArmor;
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

    public bool HasKey()
    {
        return hasKey;
    }

    public void UseKey()
    {
        hasKey = false;
    }

}
