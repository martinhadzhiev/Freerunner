using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private CharacterMovement player;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Constants.PlayerTag)
            audioManager.Play(Constants.CarPassingByAudioClipName + Random.Range(0, 2));
    }
}