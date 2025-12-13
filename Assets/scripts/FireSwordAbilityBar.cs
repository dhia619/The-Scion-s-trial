using UnityEngine;
using UnityEngine.UI;

public class FireSwordAbilityBar : MonoBehaviour
{
    public Image fireSwordBar;
    public GameObject player;
    public float currentFireSwordValue;
    void Start()
    {
        
    }

    void Update()
    {
        currentFireSwordValue = player.GetComponent<FireSword>().GetRemainingFireSword();
        fireSwordBar.fillAmount = Mathf.Clamp(currentFireSwordValue / 100, 0, 100);

    }
}
