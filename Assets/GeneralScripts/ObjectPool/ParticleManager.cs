using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private int startingCount = 2;

    [SerializeField]
    private List<ParticlePoolable> particlePoolables;

    private readonly Dictionary<string, ObjectPool<ParticlePoolable>> particlePools = new();

    public static ParticleManager Instance { get; private set; }
    public static bool Exists { get { return Instance != null; } }

    public static WaitUntil WaitUntillExists = new(() => Exists);

    private IEnumerator Start()
    {
        foreach (ParticlePoolable particle in particlePoolables)
        {
            particlePools.Add(particle.Type, new ObjectPool<ParticlePoolable>(startingCount, particle));
        }
        yield return null;
        Instance = this;
    }

    public void PlayParticleAt(string particleType, Vector3 position)
    {
        ParticlePoolable particlePoolable = particlePools[particleType].RequestObject();
        particlePoolable.transform.position = position;
    }

    public void ReturnToPool(ParticlePoolable particlePoolable)
    {
        particlePools[particlePoolable.Type].ReturnObjectToPool(particlePoolable);
    }

    private void OnDestroy()
    {
        Instance = null; //Instance destroyed
    }
}
