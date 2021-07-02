using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    private Transform lastSpawnedObstacle;
    [SerializeField]
    private ObstacleSet[] obstacleSets;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private CharacterMovement playerMovement;
    [SerializeField]
    private TrafficSoundController trafficSoundController;
    private int obstacleLevel = 0;
    private float nextSpawnPosition;
    private float lastSpawnedObstacleZ;
    private float fieldOfView = 260;
    private int changeLevelPeriod;
    private float timePassed;
    private int levelPeriodTreshhold = 15;
    private bool leaveStaticMovingObsSpace = false;
    private Queue<int> levels = new Queue<int>(new int[] { 1, 2 });
    private Dictionary<int, int> levelPeriods = new Dictionary<int, int>() { { 0, 25 }, { 1, 40 }, { 2, 60 } };
    private Dictionary<int, int> levelPositionIncrease = new Dictionary<int, int>() { { 0, 25 }, { 1, 31 }, { 2, 40 } };
    private Dictionary<int, List<ObstacleSet>> obstacles = new Dictionary<int, List<ObstacleSet>>();

    void Start()
    {
        changeLevelPeriod = UnityEngine.Random.Range(levelPeriods[obstacleLevel] - levelPeriodTreshhold, levelPeriods[obstacleLevel] + levelPeriodTreshhold);

        LoadObstaclesInDictionary();
        SpawnObstacleFromPool();
    }

    void Update()
    {
        RemoveBehindRunner();
        SpawnObstacleFromPool();
        ChangeLevel();
    }

    private void LoadObstaclesInDictionary()
    {
        foreach (var obstacleSet in obstacleSets)
        {
            if (!obstacles.ContainsKey(obstacleSet.level))
                obstacles[obstacleSet.level] = new List<ObstacleSet>();

            obstacles[obstacleSet.level].Add(obstacleSet);
            obstacleSet.IsInUse = false;
        }
    }

    private void SpawnObstacleFromPool()
    {
        var obstaclesToPickFrom = obstacles[obstacleLevel].Where(o => !o.IsInUse).ToArray();
        lastSpawnedObstacleZ = lastSpawnedObstacle == null ? 40 : lastSpawnedObstacle.position.z;

        if (obstaclesToPickFrom.Length == 0 || player.position.z + fieldOfView <= lastSpawnedObstacleZ)
            return;

        if (leaveStaticMovingObsSpace)
        {
            nextSpawnPosition = lastSpawnedObstacleZ + (lastSpawnedObstacleZ - player.position.z);
            leaveStaticMovingObsSpace = false;
        }
        else
            nextSpawnPosition = lastSpawnedObstacleZ + levelPositionIncrease[obstacleLevel];

        var randomObstacle = UnityEngine.Random.Range(0, obstaclesToPickFrom.Length);

        var obstaclesToSpawn = obstaclesToPickFrom[randomObstacle];
        obstaclesToSpawn.IsInUse = true;

        SpawnSetOnRandomPositions(obstaclesToSpawn);
    }

    private void SpawnSetOnRandomPositions(ObstacleSet set)
    {
        foreach (var obs in set.obstacles)
        {
            var obstacleZ = nextSpawnPosition + UnityEngine.Random.Range(1, 10);
            obs.transform.position = new Vector3(obs.transform.position.x, obs.transform.position.y, obstacleZ);

            if (obstacleZ > lastSpawnedObstacleZ)
            {
                lastSpawnedObstacle = obs.transform;
                lastSpawnedObstacleZ = obstacleZ;
            }
        }
    }

    private void RemoveBehindRunner()
    {
        foreach (var obstacleLevels in obstacles.Values)
        {
            var obstaclesInUse = obstacleLevels.Where(o => o.IsInUse);

            foreach (var obstacleSet in obstaclesInUse)
            {
                if (obstacleSet.obstacles.All(o => o.transform.position.z < player.position.z - 20))
                    obstacleSet.IsInUse = false;
            }
        }
    }

    private void ChangeLevel()
    {
        if (timePassed >= changeLevelPeriod)
        {
            levels.Enqueue(obstacleLevel);
            obstacleLevel = levels.Dequeue();

            if (obstacleLevel == 2)
            {
                leaveStaticMovingObsSpace = true;
                trafficSoundController.startSoundTransform = lastSpawnedObstacle;
            }
            else if (levels.Last() == 2)
                trafficSoundController.stopSoundTransform = lastSpawnedObstacle;

            changeLevelPeriod = UnityEngine.Random.Range(levelPeriods[obstacleLevel] - levelPeriodTreshhold, levelPeriods[obstacleLevel] + levelPeriodTreshhold);
            timePassed = 0;
        }

        timePassed += Time.deltaTime;
    }
}