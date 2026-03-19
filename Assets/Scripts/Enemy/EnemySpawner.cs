using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject Prefab_Enemy;
    public GameObject Prefab_SpecialEnemy;

    [Header("References")]
    public GameObject Player;
    public Pathfinder Pathfinder;
    public Grid Grid;
    public Transform[] Objectives;

    [Header("Base Enemy Stats")]
    [SerializeField] private float EnemyHit = 1f;
    [SerializeField] private float EnemySpeed = 5f;
    [SerializeField] private float EnemyHeath = 2f;

    [Header("Spawn Interval")]
    [SerializeField] private float IntervalMin = 2f;
    [SerializeField] private float IntervalMax = 3f;

    [Header("Special Enemy")]
    [Range(0f, 1f)]
    [SerializeField] private float specialEnemyChance = 0.1f;
    [SerializeField] private float specialHitMultiplier = 1.25f;
    [SerializeField] private float specialSpeedMultiplier = 1.1f;
    [SerializeField] private float specialHealthMultiplier = 1.25f;

    private float m_Interval;
    private float m_LastSpawn;

    private void Start()
    {
        m_LastSpawn = Time.time;
        m_Interval = Random.Range(IntervalMin, IntervalMax);
    }

    private void Update()
    {
        if (Time.time - m_LastSpawn >= m_Interval)
        {
            m_LastSpawn = Time.time;
            m_Interval = Random.Range(IntervalMin, IntervalMax);

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 t_SpawnPos = Grid.GridToWorld(Grid.WorldToGrid(transform.position));

        bool isSpecialEnemy = Prefab_SpecialEnemy != null && Random.value < specialEnemyChance;
        GameObject prefabToSpawn = isSpecialEnemy ? Prefab_SpecialEnemy : Prefab_Enemy;

        GameObject t_NewEnemy = Instantiate(prefabToSpawn, t_SpawnPos, Quaternion.identity);

        Enemy enemy = t_NewEnemy.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("Spawned prefab does not contain an Enemy component.");
            return;
        }

        enemy.Player = Player;
        enemy.Pathfinder = Pathfinder;
        enemy.Grid = Grid;

        if (Objectives != null && Objectives.Length > 0)
        {
            enemy.Objective = Objectives[Random.Range(0, Objectives.Length)];
        }
        else
        {
            Debug.LogWarning("EnemySpawner has no objectives assigned.");
        }

        if (isSpecialEnemy)
        {
            enemy.Hit = EnemyHit * specialHitMultiplier;
            enemy.Speed = EnemySpeed * specialSpeedMultiplier;
            enemy.Health = EnemyHeath * specialHealthMultiplier;
        }
        else
        {
            enemy.Hit = EnemyHit;
            enemy.Speed = EnemySpeed;
            enemy.Health = EnemyHeath;
        }
    }

    public void ScaleEnemy(float Scale, float ScaleHit, float ScaleSpeed, float ScaleHealth, float ScaleIntervalMin, float ScaleIntervalMax)
    {
        EnemyHit = ScaleHit * Scale;
        EnemySpeed = ScaleSpeed * Scale;
        EnemyHeath = ScaleHealth * Scale;
        IntervalMin = ScaleIntervalMin;
        IntervalMax = ScaleIntervalMax;
        m_Interval = Random.Range(IntervalMin, IntervalMax);
    }
}