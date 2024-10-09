using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private Animator fadeTransition;

    private string sceneToSwitchTo;

    void Awake()
    {
        Instance = this;
    }

    public void StartTransition()
    {
        if (fadeTransition == null)
            return;

        fadeTransition.SetTrigger("transition");
    }

    public void LoadCookingSceneBasedOnGeneration()
    {
        switch (PlayerStats.playerStatsInstance.currentGeneration.generation)
        {
            case GenerationData.Generation.Origins:
                LoadScene("OriginScene");
                break;
            case GenerationData.Generation.Renaissance:
                break;
            case GenerationData.Generation.Morden:
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        sceneToSwitchTo = sceneName;
        StartTransition();
    }

    public void ChangeScene()
    {
        SceneManager.LoadSceneAsync(sceneToSwitchTo);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}