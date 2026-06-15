using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;

    [Header("Nombre de escenas")]
    [SerializeField] private string scene1Name;
    [SerializeField] private string scene2Name;
    [SerializeField] private string scene3Name;
    [SerializeField] private string scene4Name;

    [Header("Música por escena")]
    [SerializeField] private AudioClip scene1Music;
    [SerializeField] private AudioClip scene2Music;
    [SerializeField] private AudioClip scene3Music;
    [SerializeField] private AudioClip scene4Music;

    [Header("Opciones")]
    [SerializeField] private bool dontDestroyOnLoad = true;
    [SerializeField] private float volume = 1f;

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        musicSource.volume = volume;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        if (sceneName == scene1Name)
            clipToPlay = scene1Music;
        else if (sceneName == scene2Name)
            clipToPlay = scene2Music;
        else if (sceneName == scene3Name)
            clipToPlay = scene3Music;
        else if (sceneName == scene4Name)
            clipToPlay = scene4Music;

        if (clipToPlay == null)
            return;

        if (musicSource.clip == clipToPlay)
            return;

        musicSource.clip = clipToPlay;
        musicSource.loop = true;
        musicSource.volume = volume;
        musicSource.Play();
    }
}