using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private CharacterMovement player;
    [SerializeField]
    private GameObject resumeButton;
    [SerializeField]
    private GameObject playAgainButton;
    [SerializeField]
    private OceanSoundController oceanSoundController;
    [SerializeField]
    private TrafficSoundController trafficSoundController;

    void Start()
    {
        GameIsPaused = false;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !player.isDead)
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        if (player.isDead)
            GameOver();
    }

    private void Pause()
    {
        oceanSoundController.Pause();
        trafficSoundController.Pause();
        resumeButton.SetActive(true);
        playAgainButton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
        Cursor.visible = true;
    }

    private void GameOver()
    {
        oceanSoundController.GameOver();
        trafficSoundController.GameOver();
        playAgainButton.SetActive(true);
        resumeButton.SetActive(false);
        pauseMenu.SetActive(true);
        GameIsPaused = true;
        Cursor.visible = true;
    }

    public void Resume()
    {
        oceanSoundController.Resume();
        trafficSoundController.Resume();
        pauseMenu.SetActive(false);
        Time.timeScale = TimeScaleHelper.DefaultTimeScale;
        GameIsPaused = false;
        Cursor.visible = false;
    }

    public void PlayAgain()
    {
        GameIsPaused = false;
        SceneManager.LoadScene(Constants.GameSceneName);

        SceneManager.sceneLoaded += TimeScaleHelper.ResetTimeScaleOnSceneLoad;
    }

    public void Menu()
    {
        SceneManager.LoadScene(Constants.StartMenuSceneName);
        GameIsPaused = false;
        SceneManager.sceneLoaded += TimeScaleHelper.ResetTimeScaleOnSceneLoad;
    }

    public void Quit()
    {
        Application.Quit();
    }
}