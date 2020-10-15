using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private float delta; //шаг, на который мы сдвигаем шкалу здоровья
    private float healthValue;
    private float currentHealth;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>(); //находит первый объект, на котором находится скрипт Player
        healthValue = player.Health.CurrentHealth / 100.0f; //значение здоровья заполняется от текущего количества здоровья до ста
    }

    private void Update()
    {
        currentHealth = player.Health.CurrentHealth / 100.0f;
        if (currentHealth > healthValue) //если текущее значение больше прогресс-бара
            healthValue += delta; //каждый кадр прибавляем по нашему шагу, пока не упремся в конец
        if (currentHealth < healthValue)
            healthValue -= delta;
        if (Mathf.Abs(currentHealth - healthValue) < delta) //если текущее значение здоровья равно прогресс-бару (допустимому значению, которое влезет)
            healthValue = currentHealth;
        health.fillAmount = healthValue; 
    }
}
