using UnityEngine;

public class Philadelphia : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer boatVisability;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Constants.PlayerTag)
            boatVisability.enabled = Random.value > 0.5f;
    }
}
