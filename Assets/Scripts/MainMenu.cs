using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(Constants.GameSceneName);
        SceneManager.sceneLoaded += TimeScaleHelper.ResetTimeScaleOnSceneLoad;
        Cursor.visible = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
