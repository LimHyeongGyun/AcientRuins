using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour
{
    [SerializeField]
    private Slider staminaSlider;
    
    public float maxStamina;
    public float preStamina;
    public float curStamina;

    void Update()
    {
        ChangeStamina();
    }
    public void SetStamina()
    {
        maxStamina = FindObjectOfType<Player>().stamina;
        curStamina = maxStamina;
        preStamina = curStamina;
    }
    private void ChangeStamina()
    {
        preStamina = Mathf.Lerp(preStamina, curStamina, Time.deltaTime * 5f);
        staminaSlider.value = preStamina / maxStamina;
    }
}
