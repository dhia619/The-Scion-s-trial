using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float zOffset = -10f;

    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(
                player.position.x,
                player.position.y + yOffset,
                zOffset
            );
        }
    }
}
