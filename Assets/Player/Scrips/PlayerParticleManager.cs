using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem GROUNDHIT { get; private set; }
    [field: SerializeField] public ParticleSystem JUMP { get; private set; }
    [field: SerializeField] public ParticleSystem DIRECTIONCHANGELEFT { get; private set; }
    [field: SerializeField] public ParticleSystem DIRECTIONCHANGERIGHT { get; private set; }
}
