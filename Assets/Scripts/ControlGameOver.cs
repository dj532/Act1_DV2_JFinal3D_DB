using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlGameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
        if (audioSource != null)
        {
            audioSource.Pause(); // Pausar la música
            
        }
    }
    public void IniciarJuego()
    {
        Debug.Log("El botón de reinicio ha sido presionado.");

        Time.timeScale = 1f; // Reinicia el tiempo del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
        if (audioSource != null)
        {
            audioSource.Play();// Reproduce musica de nuevo
        }
    }
}
