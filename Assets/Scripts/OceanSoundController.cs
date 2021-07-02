using UnityEngine;
using System.Collections;

public class OceanSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private CharacterMovement player;
    [SerializeField]
    private float oceanVolume;
    private bool isPlaying;
    private Coroutine lastCoroutine;

    void Start()
    {
        source.volume = 0;
        isPlaying = false;
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Resume()
    {
        source.Play();
    }

    public void GameOver()
    {
        StopSound();
        this.enabled = false;
    }

    public void OceanSoundTriggered()
    {
        if (isPlaying)
            StopSound();
        else
            PlaySound();
    }

    private void PlaySound()
    {
        StopLastVolumeCoroutine();
        var increaseStep = 0.1f + player.speed / 100;
        lastCoroutine = StartCoroutine(LerpVolume(oceanVolume, increaseStep));
        isPlaying = true;
    }

    private void StopSound()
    {
        StopLastVolumeCoroutine();
        var decreaseStep = 0.05f + player.speed / 100;
        lastCoroutine = StartCoroutine(LerpVolume(0, decreaseStep));
        isPlaying = false;
    }

    private void StopLastVolumeCoroutine()
    {
        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);
    }

    private IEnumerator LerpVolume(float desiredVolume, float step)
    {
        while (Mathf.Abs(source.volume - desiredVolume) > 0.1)
        {
            source.volume = Mathf.Lerp(source.volume, desiredVolume, step);

            yield return null;
        }

        source.volume = desiredVolume;
    }
}
