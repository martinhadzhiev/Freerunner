using UnityEngine;

public class ColliderHelper : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement movement;

    public void SetJumpCollider()
    {
        movement.SetJumpCollider();
    }

    public void SetRollCollider()
    {
        movement.SetRollCollider();
    }

    public void ResetCollider()
    {
        movement.ResetCollider();
    }
}
