using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Settings")]
    public float upPosition = 0f;
    public float downPosition = -2f;
    public float riseSpeed = 3f;
    public float fallSpeed = 2f;
    public float timeUp = 1f;
    public float timeDown = 2f;
    
    private Vector3 targetUp;
    private Vector3 targetDown;
    private bool isActive = true;

    void Start()
    {
        Vector3 currentPos = transform.position;
        targetUp = new Vector3(currentPos.x, currentPos.y + upPosition, currentPos.z);
        targetDown = new Vector3(currentPos.x, currentPos.y + downPosition, currentPos.z);
        
        StartCoroutine(SpikeCycle());
    }

    IEnumerator SpikeCycle()
    {
        while (isActive)
        {
            // Rise up
            yield return StartCoroutine(MoveToPosition(targetUp, riseSpeed));
            
            // Stay up
            yield return new WaitForSeconds(timeUp);
            
            // Go down
            yield return StartCoroutine(MoveToPosition(targetDown, fallSpeed));
            
            // Stay down
            yield return new WaitForSeconds(timeDown);
        }
    }

    IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}