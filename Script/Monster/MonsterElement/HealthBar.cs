using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private float reduceSpeed = 2;

    private float target = 1;
    public Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;   
    }

    void Update()
    {
        if(_cam != null)
            transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        healthImage.fillAmount = Mathf.MoveTowards(healthImage.fillAmount, target, reduceSpeed * Time.deltaTime);
    }  
}