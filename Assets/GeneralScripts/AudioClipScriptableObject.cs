using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClip", menuName = "Audio/AudioClip")]
public class AudioClipScriptableObject : ScriptableObject
{
    public AudioClip[] clips;
    public float MaxPitch;
    public float MinPitch;
    public bool randomPitch;
}
