using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private float delta; // step which moves healthbar
    private float healthValue;
    private float currentHealth;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>(); // it finds the first object with the script Player on it
        healthValue = player.Health.CurrentHealth / 100.0f; // health value fills from current health to 100
    }

    private void Update()
    {
        currentHealth = player.Health.CurrentHealth / 100.0f;
        if (currentHealth > healthValue) // if current health more than health value of healthbar
            healthValue += delta; // add our health step per frame until it will fill to maximum
        if (currentHealth < healthValue)
            healthValue -= delta;
        if (Mathf.Abs(currentHealth - healthValue) < delta) //  if current health equals to health value of healthbar (valid value which will fit)
            healthValue = currentHealth;
        health.fillAmount = healthValue; 
    }
}
