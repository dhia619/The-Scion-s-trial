using UnityEngine;

public class FireSword : MonoBehaviour
{
    private bool fireSword;
    [SerializeField] private float remainingFireSword;
    private float fireSwordIncrement;
    private Animator playerAnimation;

    void Start()
    {
        fireSword = false;
        remainingFireSword = 0;
        fireSwordIncrement = 0.08f;
        playerAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        if (!fireSword && remainingFireSword < 100) 
        {
            remainingFireSword = remainingFireSword + fireSwordIncrement;
        }
        if (remainingFireSword >= 100)
        {
            if (!fireSword)
            {
                // enable the fire sword ability
                fireSword = true;
                playerAnimation.SetTrigger("fire_sword_trigger");
                playerAnimation.SetBool("fire_sword", true);
            }
        }
    }

    public bool GetFireSword()
    {
        return fireSword;
    }

    public float GetRemainingFireSword()
    {
        return remainingFireSword;
    }
}
