using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePoolable : MonoBehaviour, IPoolable
{
    [field: SerializeField] public string Type { get; private set; }

    public bool Active { get; set; }

    private ParticleSystem system;
    private float particleEndTime;

    public void OnEnableObject()
    {
        if (system == null) { system = GetComponent<ParticleSystem>(); }

        system.gameObject.SetActive(true);
        system.Play();
        particleEndTime = Time.time + system.main.duration;
    }

    public void OnDisableObject()
    {
        system.Stop();
        system.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Active && Time.time > particleEndTime)
        {
            ParticleManager.Instance.ReturnToPool(this);
        }
    }

}