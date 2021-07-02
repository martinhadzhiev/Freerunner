using System.Collections.Generic;
using UnityEngine;

public class EndlessWorld : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private List<GameObject> groundPlanes;
    [SerializeField]
    private List<GameObject> surroundings;
    private int planeCount;
    private int movePlaneTreshold = 40;
    private int groundMoveDistance;
    private int surrMoveDistance = 520;

    void Start()
    {
        planeCount = groundPlanes.Count;
        groundMoveDistance = planeCount * movePlaneTreshold;
    }

    void Update()
    {
        MoveGround();
        MoveSurroundings();
    }

    private void MoveGround()
    {
        if (groundPlanes.Count < planeCount)
            return;

        var firstGoundPlane = groundPlanes[0];

        if (player.position.z > firstGoundPlane.transform.position.z + movePlaneTreshold)
        {
            firstGoundPlane.transform.position = new Vector3(firstGoundPlane.transform.position.x, firstGoundPlane.transform.position.y, firstGoundPlane.transform.position.z + groundMoveDistance);

            groundPlanes.Remove(firstGoundPlane);
            groundPlanes.Add(firstGoundPlane);
        }
    }

    private void MoveSurroundings()
    {
        foreach (var surrounding in surroundings)
        {
            if (player.position.z > surrounding.transform.position.z + movePlaneTreshold)
                surrounding.transform.position = new Vector3(surrounding.transform.position.x, surrounding.transform.position.y, surrounding.transform.position.z + surrMoveDistance);
        }
    }
}