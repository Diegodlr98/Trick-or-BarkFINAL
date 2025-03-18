using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    public GameObject gameOver;

    private bool canTakeDamage = true;
    public bool gameover = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (canTakeDamage)
        {
            if (other.CompareTag("Light"))
            {
                TakeLightDamage();
            }
            else if (other.CompareTag("car"))
            {
                TakeDamage();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light") || other.CompareTag("car"))
        {
            canTakeDamage = true; 
        }
    }
    private void TakeLightDamage()
    {
        currentHealth -= 1;
        healthUI.UpdateHearts(currentHealth);
        canTakeDamage = false;

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    private void TakeDamage()
    {
        currentHealth -= 3;
        healthUI.UpdateHearts(currentHealth);
        canTakeDamage = false;

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        gameOver.SetActive(true);
        gameover = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
