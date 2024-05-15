using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    public static Player_Health Instance; // Access this class from the Instace

    [Header("HEALTH VALUE")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] Image healthUI;

    [Header("STAMINA VALUE")]
    [SerializeField] float maxStamina = 100;
    public float stamina = 100;
    [SerializeField] float staminaRegenRate = 15;
    [SerializeField] Image staminahUI;

    private void Awake()
    {
        Instance = this;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        healthUI.fillAmount = (float)health / maxHealth;
        staminahUI.fillAmount = stamina / maxStamina;

        if (stamina < maxStamina)
        {
            stamina = Mathf.MoveTowards(stamina, maxStamina, staminaRegenRate * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Heal(int heal)
    {
        health += heal;
    }

    public bool SpendStamina(float exhaust)
    {
        if (exhaust > stamina)
        {
            return false;
        }
        else
        {
            stamina -= exhaust;
            return true;
        }
    }

    [ContextMenu("KILL")]
    void Die()
    {
        print("Player Died");
    }
}
