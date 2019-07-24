﻿using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;

    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress, progressFactor, speed;
    int health;

    Direction direction;
    DirectionChange directionChange;
    float directionAngleFrom, directionAngleTo;
    float Health { get; set; }

    [SerializeField]
    Transform model = default;

    [SerializeField]
    Transform model = default;

    EnemyData enemyData;

    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void SpawnOn (GameTile tile)
    {
        Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);
        tileFrom = tile;
        tileTo = tile.NextTileOnPath;
        progress = 0f;
        Health = 100f;  // Should be set to the value from EnemyData
        PrepareIntro();
    }

    public bool GameUpdate()
    {
        if(Health <= 0f)
        {
            OriginFactory.Reclaim(this);
            return false;
        }

        progress += Time.deltaTime * progressFactor;
        while(progress >= 1f)
        {
            if (tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            progress = (progress - 1f) / progressFactor;
            PrepareNextState();
            progress *= progressFactor;
        }
        if (directionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
        return true;
    }

    public void ApplyDamage (float damage)
    {
        Debug.Assert(damage >= 0f, "Negative Damage Applied");
        Health -= damage;
    }

    // State-ish stuff
    void PrepareIntro()
    {
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        direction = tileFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }

    internal void SetUp(EnemyData enemyData)
    {
        speed = enemyData.speed;
        health = enemyData.health;
        model.GetComponent<MeshRenderer>().material = enemyData.material;
        model.GetComponent<MeshFilter>().mesh = enemyData.mesh;
    }

    void PrepareNextState()
    {
        tileFrom = tileTo;
        tileTo = tileTo.NextTileOnPath;
        positionFrom = positionTo;
        if (tileTo == null)
        {
            PrepareOutro();
            return;
        }
        positionTo = tileFrom.ExitPoint;
        directionChange = direction.GetDirectionChangeTo(tileFrom.PathDirection);
        direction = tileFrom.PathDirection;
        directionAngleFrom = directionAngleTo;
        switch (directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    void PrepareOutro()
    {
        positionTo = tileFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngleTo = direction.GetAngle();
        model.localPosition = Vector3.zero;
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }

    void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(0f, model.localPosition.y, model.localPosition.z);
        progressFactor = speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(-0.5f, model.localPosition.y, model.localPosition.z);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.25f);
    }

    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(0.5f, model.localPosition.y, model.localPosition.z);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.25f);
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + 180f;
        model.localPosition = new Vector3(0f, model.localPosition.y, model.localPosition.z);
        transform.localPosition = positionFrom;
        progressFactor = 2f * speed;
    }
}