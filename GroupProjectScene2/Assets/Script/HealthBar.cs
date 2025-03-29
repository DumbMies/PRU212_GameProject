using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (fill == null && slider != null)
        {
            fill = slider.fillRect.GetComponent<Image>();
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        if (fill != null && gradient != null)
        {
            fill.color = gradient.Evaluate(1f);
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        if (fill != null && gradient != null)
        {
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}