using UnityEngine;
using System.Collections;
using System;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public HealthUI healthUI;
    public GameObject gameOver;

    public PlayerMovement PlayerMovement;
    public CharacterController characterController;
    private Animator anim;
    private bool canTakeDamage = true;
    public bool gameover = false;
    public float invulnerabilityTime = 2f;

    public static event Action OnPlayerDamaged;

    [Header("Light Feedback")]
    public Light playerLight; //  Referencia a la luz
    public Color damageColor = Color.red; //  Color al recibir da�o
    private Color originalLightColor; //  Guardamos el color original

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);

        if (playerLight != null)
        {
            originalLightColor = playerLight.color; // Guardamos el color original al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light") && canTakeDamage)
        {
            TakeLightDamage();
        }
        else if (other.CompareTag("car"))
        {
            TakeDamage();
        }
    }

    private void TakeLightDamage()
    {
        currentHealth -= 1;
        healthUI.UpdateHearts(currentHealth);
        OnPlayerDamaged?.Invoke();

        StartCoroutine(InvulnerabilityCooldown());

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    private void TakeDamage()
    {
        currentHealth -= 3;
        healthUI.UpdateHearts(currentHealth);
        OnPlayerDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }
    private IEnumerator HandleDeath()
    {
        anim.SetBool("isDead", true);

        // Espera un breve momento antes de desactivar componentes, para asegurar que arranque la animaci�n
        yield return new WaitForSeconds(0.2f);

        // Alternativa: desactiva el movimiento sin desactivar el script del todo a�n
        PlayerMovement.enabled = false;
        characterController.enabled = false;

        // Espera la duraci�n completa de la animaci�n (ajusta si es m�s corta)
        yield return new WaitForSeconds(3.5f);

        GameOver();
    }

    private IEnumerator InvulnerabilityCooldown()
    {
        canTakeDamage = false;

        // Cambiamos el color de la luz al de da�o
        if (playerLight != null)
        {
            playerLight.color = damageColor;
        }

        yield return new WaitForSeconds(invulnerabilityTime);

        //  Restauramos el color original
        if (playerLight != null)
        {
            playerLight.color = originalLightColor;
        }

        canTakeDamage = true;
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
