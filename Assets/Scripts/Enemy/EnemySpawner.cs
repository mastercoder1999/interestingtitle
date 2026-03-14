using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Prefab_Enemy;
    public GameObject Player;
    public Pathfinder Pathfinder;
    public Grid Grid;
    public Transform[] Objectives;

    private float EnemyHit = 1f;
    private float EnemySpeed = 5f;
    private float EnemyHeath = 2f;

    private float IntervalMin = 2f;
    private float IntervalMax = 3f;

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

            Vector3 t_SpawnPos = Grid.GridToWorld(Grid.WorldToGrid(transform.position));
            GameObject t_NewEnemy = Instantiate(Prefab_Enemy, t_SpawnPos, Quaternion.identity);

            Enemy enemy = t_NewEnemy.GetComponent<Enemy>();
            enemy.Player = Player;
            enemy.Pathfinder = Pathfinder;
            enemy.Grid = Grid;
            enemy.Objective = Objectives[Random.Range(0, Objectives.Length)];
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