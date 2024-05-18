using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    string currentScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update() => currentScene = SceneManager.GetActiveScene().name;

    public void GoToScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

    public void Play() => GoToScene("_Game");

    public void Quit() => Application.Quit();

    public void RestartScene() => GoToScene(CurrentScene());

    public void FinishLevel()
    {
        if (CurrentScene() == "_Game")
            GoToScene("_Win");
        else
            Debug.Log("No Scenes Available Next");
    }
    
    public string CurrentScene() => currentScene;
}
