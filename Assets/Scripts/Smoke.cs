using UnityEngine;

public class Smoke : WarEntity
{
    float age;
    float duration;

    void Awake()
    {
        age = 0f;
        duration = GetComponent<ParticleSystem>().main.duration;
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
    }

    public override bool GameUpdate()
    {
        age += Time.deltaTime;
        if (age >= duration)
        {
            OriginFactory.Reclaim(this);
            return false;
        }
        return true;
    }
}