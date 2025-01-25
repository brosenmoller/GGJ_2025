using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public enum ParticleType
    {
        BubbleBurst = 0,
        Freeze = 1,
        FreezeEnd = 2,
        Death = 3,
    }

    [SerializeField]
    private int startingCount = 2;

    [SerializeField]
    private SerializableDictionary<ParticleType, ParticlePoolable> ParticleDictornary;

    private readonly Dictionary<ParticleType, ObjectPool<ParticlePoolable>> particlePools = new();

    public static ParticleManager Instance { get; private set; }
    public static bool Exists { get { return Instance != null; } }

    public static WaitUntil WaitUntillExists = new(() => Exists);

    private IEnumerator Start()
    {
        foreach (ParticleType type in ParticleDictornary.Keys)
        {
            particlePools.Add(type, new ObjectPool<ParticlePoolable>(startingCount, ParticleDictornary[type]));
        }
        yield return null;
        Instance = this;
    }

    public void PlayParticleAt(ParticleType particleType, Vector3 position)
    {
        ParticlePoolable particlePoolable = particlePools[particleType].RequestObject();
        particlePoolable.transform.position = position;
        particlePoolable.ParticleType = particleType;
    }

    public void ReturnToPool(ParticlePoolable particlePoolable)
    {
        particlePools[particlePoolable.ParticleType].ReturnObjectToPool(particlePoolable);
    }

    private void OnDestroy()
    {
        Instance = null; //Instance destroyed
    }
}
