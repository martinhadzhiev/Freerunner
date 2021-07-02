using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private const float CarAccelerateTreshhld = 200;
    private const int CarSpeedIncrease = 10;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float beforeTriggerSpeed;
    [SerializeField]
    private float accelerateStep = 0.2f;
    [SerializeField]
    private CharacterMovement player;
    private float currentSpeed;
    private float randomSpeedIncrease;

    void Start()
    {
        randomSpeedIncrease = Random.Range(1, 2);
    }

    void Update()
    {
        speed = player.speed + CarSpeedIncrease + randomSpeedIncrease;

        if (player.transform.position.z + CarAccelerateTreshhld > transform.position.z)
        {
            if (currentSpeed < speed)
                currentSpeed += accelerateStep;
        }
        else
            currentSpeed = player.speed;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - currentSpeed * Time.deltaTime);
    }
}
