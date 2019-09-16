using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrder
{
    float _time, _spawnRate;
    public float time { get => _time; }
    public float spawnRate { get => _spawnRate; }

    public SpawnOrder(float time, float spawnRate)
    {
        _time = time;
        _spawnRate = spawnRate;
    }
}
