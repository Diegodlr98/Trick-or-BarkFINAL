using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    public GameObject gameOver;

    private bool canTakeDamage = true;
    public bool gameover = false;

    
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
            TakeLightDamage();
        }
        if (other.CompareTag("car") && canTakeDamage)
        {
            TakeDamage();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CarLight"))
        {
            canTakeDamage = true;
        }
        if (other.CompareTag("car"))
        {
            canTakeDamage = true;
        }
    }
    
    private void TakeLightDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth -= 1;
            healthUI.UpdateHearts(currentHealth);            

            if (currentHealth <= 0)
            {
                GameOver();
            }
        }
    }
    private void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth -= 3;
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
        gameOver.SetActive(true);
        gameover = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
}
