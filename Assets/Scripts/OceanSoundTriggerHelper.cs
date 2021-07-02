using UnityEngine;

public class OceanSoundTriggerHelper : MonoBehaviour
{
    [SerializeField]
    OceanSoundController oceanSoundController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Constants.PlayerTag)
            oceanSoundController.OceanSoundTriggered();
    }
}
