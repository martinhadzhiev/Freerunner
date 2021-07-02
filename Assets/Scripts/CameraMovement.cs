using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement player;
    [SerializeField]
    private float cameraDistance = 10f;
    [SerializeField]
    private float cameraHeight = 3;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float shakeDuration = 0.2f;
    [SerializeField]
    private float shakeAmount = 0.2f;
    [SerializeField]
    private float decreaseFactor = 1.0f;
    private Transform playerTransform;
    private Vector3 originalPos;

    void Start()
    {
        playerTransform = player.transform;
    }

    void Update()
    {
        var newPos = new Vector3(playerTransform.position.x, playerTransform.position.y + cameraHeight, playerTransform.position.z - cameraDistance);

        if (playerTransform.position.z - cameraDistance > cameraDistance)
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * speed);

        if (player.isDead)
        {
            originalPos = transform.localPosition;

            Shake();
        }
    }

    private void Shake()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }
}
