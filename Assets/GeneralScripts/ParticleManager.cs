using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, ParticleSystem> ParticleDictornary;
    public static ParticleManager Instance { get; private set; }
    public static bool Exists { get { return Instance != null; } }

    public static WaitUntil waitUntil = new WaitUntil(() => Exists);

    void Awake()
    {
        Instance = this;
    }

    public void PlayeParticleAt(string particleKey, Vector3 position)
    {
        //TODO: Replace with pool
        Instantiate(ParticleDictornary[particleKey], position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        Instance = null; //Instance destroyed
    }
}
