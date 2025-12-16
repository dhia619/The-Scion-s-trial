using System;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private Transform transform;
    [SerializeField] private AudioClip collectSound;
    private Vector3 startPos;
    public float floatSpeed = 4f;
    public float floatAmplitude = 0.1f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(collectSound);
        }
    }

    public void Start()
    {
        transform = GetComponent<Transform>();
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos +
            new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0);
    }
}
