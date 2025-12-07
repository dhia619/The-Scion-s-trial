using UnityEngine;

public class ArmorController : MonoBehaviour
{
    [SerializeField] private int armorToAdd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeArmor(armorToAdd);
                gameObject.SetActive(false);
            }
        }
    }
}
