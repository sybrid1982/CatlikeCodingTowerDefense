using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField]
    GameBoard board = default;

    [SerializeField]
    GameTileContentFactory tileContentFactory = default;

    [SerializeField]
    EnemyFactory enemyFactory = default;

    [SerializeField]
    float spawnSpeed = 1f;

    [SerializeField]
    EnemyData enemyData = default;

    [SerializeField]
    EnemyData fastEnemyData = default;

    int enemySpawnCounter = 0;

    float spawnProgress = 0;

    EnemyCollection enemies = new EnemyCollection();

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void Awake()
    {
        board.Initialize(boardSize, tileContentFactory);
        board.ShowGrid = true;
    }
    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        // Spawn idea - an object should exist that constitutes a 'timeline'
        // as spawn progress increases, we check to see where we are on the timeline now vs before
        // anything in that time range should then be spawned
        // Spawn progress thing would hold references to enemyData
        spawnProgress += spawnSpeed * Time.deltaTime;
        while (spawnProgress >= 1)
        {
            spawnProgress -= 1f;
            if (enemySpawnCounter < 3)
            {
                SpawnEnemy(enemyData);
                enemySpawnCounter++;
            } else
            {
                SpawnEnemy(fastEnemyData);
                enemySpawnCounter = 0;
            }
        }
        enemies.GameUpdate();
    }

    private void HandleAlternativeTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleDestination(tile);
            }
            else
            {
                board.ToggleSpawnPoint(tile);
            }
        }
    }

    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            board.ToggleWall(tile);
        }
    }

    void SpawnEnemy(EnemyData enemyData)
    {
        GameTile spawnPoint = board.GetSpawnPoint(Random.Range(0, board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get();
        enemy.SetUp(enemyData);
        enemy.SpawnOn(spawnPoint);
        enemies.Add(enemy);
    }
}