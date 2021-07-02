using UnityEngine;

public class StepSounds : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    public void Step()
    {
        audioManager.Play(Constants.StepAudioClipName);
    }
}
