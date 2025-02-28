using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    public MenuPausa pauseMenu;

    private bool canTakeDamage = true;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light") && canTakeDamage)
        {
            TakeDamage();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            canTakeDamage = true;
        }
    }
    private void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth -= 1;
            healthUI.UpdateHearts(currentHealth);
            canTakeDamage = false;

            if (currentHealth <= 0)
            {
                GameOver();
            }
        }
    }
    private void GameOver()
    {
        pauseMenu.ShowPauseMenu();
    }

    
}
