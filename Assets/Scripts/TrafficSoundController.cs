using System.Collections;
using UnityEngine;

public class TrafficSoundController : MonoBehaviour
{
    private const int SoundPlayStopDistance = 15;

    [HideInInspector]
    public Transform startSoundTransform;
    [HideInInspector]
    public Transform stopSoundTransform;

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float trafficVolume;
    private Coroutine lastCoroutine;

    void Start()
    {
        source.volume = 0;
    }

    void Update()
    {
        if (startSoundTransform != null && playerTransform.position.z - startSoundTransform.position.z > SoundPlayStopDistance)
        {
            PlaySound();
            startSoundTransform = null;
        }
        else if (stopSoundTransform != null && playerTransform.position.z - stopSoundTransform.position.z > SoundPlayStopDistance)
        {
            StopSound();
            stopSoundTransform = null;
        }
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Resume()
    {
        source.UnPause();
    }

    public void GameOver()
    {
        StopSound();
        this.enabled = false;
    }

    private void PlaySound()
    {
        source.Play();
        StopLastVolumeCoroutine();
        var increaseStep = 0.1f;
        lastCoroutine = StartCoroutine(LerpVolume(trafficVolume, increaseStep));
    }

    private void StopSound()
    {
        StopLastVolumeCoroutine();
        var decreaseStep = 0.05f;
        lastCoroutine = StartCoroutine(LerpVolume(0, decreaseStep));
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