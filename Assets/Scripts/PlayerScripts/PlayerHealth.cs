using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;  // Salud máxima del jugador
    private int currentHealth;  // Salud actual del jugador
    public HealthUI healthUI;   // UI para mostrar la salud
    public GameObject gameOver; // Game over panel

    private bool canTakeDamage = true; // Determina si el jugador puede recibir daño
    public bool gameover = false; // Si el juego ha terminado
    public float invulnerabilityTime = 2f; // Tiempo de invulnerabilidad en segundos

    public static event Action OnPlayerDamaged; // Evento que notifica daño

    void Start()
    {
        currentHealth = maxHealth; // Inicializa la salud
        healthUI.SetMaxHearts(maxHealth); // Configura la UI
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en el collider de "Light", y puede recibir daño, toma daño
        if (other.CompareTag("Light") && canTakeDamage)
        {
            TakeLightDamage();
        }
        // Si el jugador entra en el collider de "car", toma daño sin invulnerabilidad
        else if (other.CompareTag("car"))
        {
            TakeDamage(); // El daño de "car" ignora la invulnerabilidad
        }
    }

    private void TakeLightDamage()
    {
        currentHealth -= 1; // Resta una vida por daño ligero
        healthUI.UpdateHearts(currentHealth); // Actualiza la UI de salud
        OnPlayerDamaged?.Invoke(); // Invoca el evento de daño para notificar a los niños

        StartCoroutine(InvulnerabilityCooldown()); // Activa la invulnerabilidad temporal

        if (currentHealth <= 0)
        {
            GameOver(); // Si la salud es 0 o menos, el juego termina
        }
    }

    private void TakeDamage()
    {
        currentHealth -= 3; // Resta 3 vidas por daño más severo
        healthUI.UpdateHearts(currentHealth); // Actualiza la UI de salud
        OnPlayerDamaged?.Invoke(); // Invoca el evento de daño para notificar a los niños

        if (currentHealth <= 0)
        {
            GameOver(); // Si la salud es 0 o menos, el juego termina
        }
    }

    private IEnumerator InvulnerabilityCooldown()
    {
        canTakeDamage = false; // Desactiva la capacidad de recibir más daño
        yield return new WaitForSeconds(invulnerabilityTime); // Espera el tiempo de invulnerabilidad
        canTakeDamage = true; // Vuelve a permitir que el jugador reciba daño
    }

    private void GameOver()
    {
        gameOver.SetActive(true); // Muestra la pantalla de Game Over
        gameover = true;
        Time.timeScale = 0f; // Detiene el tiempo (pausa el juego)
        Cursor.visible = true; // Muestra el cursor
        Cursor.lockState = CursorLockMode.None; // Libera el cursor
    }
}
