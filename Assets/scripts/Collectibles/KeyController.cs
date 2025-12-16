using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.hasKey = true;
                gameObject.SetActive(false);
                DialogueManager.Instance.ShowDialogue("Great! We need this key!"); 
            }
        }
    }
}
