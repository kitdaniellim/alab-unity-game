using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //Public Properties
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;
    // public Transform target;

    // Update is called once per frame
    void Update()
    {
        // slider.transform.position = target.position;
    }

    public void setHealth(float health, float maxHealth) {
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;

        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
}
