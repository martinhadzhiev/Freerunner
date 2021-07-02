using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeScaleHelper : MonoBehaviour
{
    public const int DefaultTimeScale = 1;

    public static void ResetTimeScaleOnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        Time.timeScale = DefaultTimeScale;
    }
}
