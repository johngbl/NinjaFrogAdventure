using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip portalSound;
    public float loadDelay = 0.3f;

    private bool used;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (used)
            return;

        if (!collision.CompareTag("Player"))
            return;

        used = true;

        if (audioSource != null && portalSound != null)
            audioSource.PlayOneShot(portalSound);

        Invoke(nameof(LoadNextScene), loadDelay);
    }

    void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("Next Scene Name não foi configurado no portal.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}