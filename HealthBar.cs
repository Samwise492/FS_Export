using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private float delta; // step which moves healthbar
    [SerializeField] private float healthNumber;
    private float healthValue;
    private float currentHealth;
    [SerializeField] private Health health;

    private void Start()
    {
        healthValue = health.CurrentHealth / 100f; // health value fills from current health to 100
    }

    private void Update()
    {      
        currentHealth = health.CurrentHealth / healthNumber;

        if (currentHealth > healthValue) // if current health more than health value of healthbar
            healthValue += delta; // add our health step per frame until it will fill to maximum
        if (currentHealth < healthValue)
            healthValue -= delta;
        if (Mathf.Abs(currentHealth - healthValue) < delta) //  if current health equals to health value of healthbar (valid value which will fit)
            healthValue = currentHealth;
        healthImage.fillAmount = healthValue; 
    }
}
