using UnityEngine;

public class SunMovement : MonoBehaviour
{
    [SerializeField]
    private float SunDistanceFromPlayer = 250;
    [SerializeField]
    private Transform player;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + SunDistanceFromPlayer);
    }
}
