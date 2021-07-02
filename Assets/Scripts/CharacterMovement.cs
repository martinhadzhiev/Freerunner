using UnityEngine;
using System.Collections;
public class CharacterMovement : MonoBehaviour
{
    public float speed = 4.0f;

    [SerializeField]
    private float maxPlayerSpeed = 16;
    [SerializeField]
    private float maxRunAnimationSpeed = 1.6f;
    public bool isDead = false;
    private float runAnimationSpeed = 1;
    [SerializeField]
    private float jumpHeight = 8.0f;
    private bool jump;
    [SerializeField]
    private float earlyJumpTreshhold = 0.3f;
    private float timePassedSinceJumpKeyPressed;
    [SerializeField]
    private float crouchForce = 8.0f;
    [SerializeField]
    private float turnSpeed = 5;
    [SerializeField]
    private float laneDistance = 3;
    [SerializeField]
    private float gravityValue = 2.0f;
    [SerializeField]
    private float moveDistance = 4;
    [SerializeField]
    private int desiredLane;
    [SerializeField]
    private float characterHeight = 1.85f;
    [SerializeField]
    private float characterRollHeight = 1.1f;
    [SerializeField]
    private float jumpColliderHeigthAndCenter = 1.2f;
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    private LayerMask jumpMask;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private Transform hips;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private float groundCheckRadius = 0.5f;
    [SerializeField]
    private Transform groundCheck;

    private float defaultControllerCenter = 0.93f;
    private float rollControllerCenter = 0.555f;
    private bool canRoll = true;
    private Vector3 velocity;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isDead)
        {
            velocity.y += gravityValue * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            return;
        }

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2;

        if (Physics.CheckSphere(hips.transform.position, 0.25f, obstacleMask.value) ||
         (Physics.CheckSphere(head.transform.position, 0.15f, obstacleMask.value) && canRoll))
            Die();

        SetAnimationParameters();

        Roll();

        Jump();

        SwitchLanes();

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        IncreaseSpeed();
    }

    public void SetJumpCollider()
    {
        controller.height = jumpColliderHeigthAndCenter;
        controller.center = new Vector3(controller.center.x, jumpColliderHeigthAndCenter, controller.center.z);
    }

    public void SetRollCollider()
    {
        canRoll = false;
        controller.height = characterRollHeight;
        controller.center = new Vector3(controller.center.x, rollControllerCenter, controller.center.z);
    }

    public void ResetCollider()
    {
        controller.height = characterHeight;
        controller.center = new Vector3(controller.center.x, defaultControllerCenter, controller.center.z);
        canRoll = true;
    }

    private void Die()
    {
        playerAnimator.SetTrigger(Constants.DieAnimationTriggerName);
        audioManager.Play(Constants.DeathAudioClipName);
        isDead = true;
        Time.timeScale = 0.4f;

        StartCoroutine(ScaleTime(0.4f, 0.0f, 4.0f));
    }

    private void SetAnimationParameters()
    {
        playerAnimator.SetFloat(Constants.AnimationSpeedKey, runAnimationSpeed);

        if (controller.isGrounded)
            playerAnimator.SetBool(Constants.AnimationIsGroundedKey, controller.isGrounded);
    }

    private void IncreaseSpeed()
    {
        if (speed < maxPlayerSpeed)
            speed += 0.1f * Time.deltaTime;

        if (runAnimationSpeed < maxRunAnimationSpeed)
            runAnimationSpeed += 0.004f * Time.deltaTime;
    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            jump = true;

        if (jump)
            timePassedSinceJumpKeyPressed += Time.deltaTime;

        if (timePassedSinceJumpKeyPressed > earlyJumpTreshhold)
        {
            jump = false;
            timePassedSinceJumpKeyPressed = 0;
        }

        if (jump && Physics.CheckSphere(groundCheck.position, groundCheckRadius, jumpMask))
        {
            jump = false;
            timePassedSinceJumpKeyPressed = 0;

            audioManager.Play(Constants.JumpAudioClipName);
            playerAnimator.SetBool(Constants.AnimationIsGroundedKey, false);
            playerAnimator.SetTrigger(Constants.JumpAnimationTriggerName);

            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityValue);
            canRoll = true;
        }
    }

    private void Roll()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && canRoll)
        {
            audioManager.Play(Constants.RollAudioClipName);
            playerAnimator.SetTrigger(Constants.RollAnimationTriggerName);

            jump = false;
            velocity.y = -Mathf.Sqrt(crouchForce * -2 * gravityValue);
        }
    }

    private void SwitchLanes()
    {
        var oldDesiredLane = desiredLane;

        Vector3 move = new Vector3(0, 0, moveDistance * speed);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            desiredLane = Mathf.Clamp(--desiredLane, -1, 1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            desiredLane = Mathf.Clamp(++desiredLane, -1, 1);

        if (desiredLane == -1)
            move.x = (-1 * laneDistance - transform.position.x) * turnSpeed;
        else if (desiredLane == 1)
            move.x = (1 * laneDistance - transform.position.x) * turnSpeed;
        else
            move.x = (0 * laneDistance - transform.position.x) * turnSpeed;

        if (oldDesiredLane != desiredLane)
            audioManager.Play(Constants.TurnAudioClipName);

        controller.Move(move * Time.deltaTime);
    }

    public IEnumerator ScaleTime(float start, float end, float time)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < time)
        {
            Time.timeScale = Mathf.Lerp(start, end, timer / time);
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;

            yield return null;
        }

        Time.timeScale = end;
        this.enabled = false;
    }
}