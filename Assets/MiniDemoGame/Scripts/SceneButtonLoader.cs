using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonLoader : MonoBehaviour
{
    [Header("Escena a cargar")]
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado una escena en el Inspector.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}