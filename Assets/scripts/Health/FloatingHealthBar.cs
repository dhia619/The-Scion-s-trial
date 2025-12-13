using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;


    void Start()
    {
        fill.color = Color.green;
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        if (!gameObject.CompareTag("Player"))
        {
            slider.transform.rotation = camera.transform.rotation;
            slider.transform.position = target.position + offset;
        }

        if (slider.value < 0.6 && slider.value > 0.3)
        {
            fill.color = Color.yellow;
        }
        else if (slider.value < 0.3)
        {
            fill.color = Color.red;
        }
    }
}
