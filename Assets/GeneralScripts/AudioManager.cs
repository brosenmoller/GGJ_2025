using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, AudioClip> AudioDictornary = new();
    private Dictionary<string, GameObject> SingleInstanceDictonary = new();
    public static AudioManager Instance { get; private set; }
    public static bool Exists { get { return Instance != null; } }

    public static WaitUntil waitUntil = new(() => Exists);

    private void Awake()
    {
        Instance = this;
    }

    public void PlayOneShotRandomPitch(AudioClip clip, Vector3 position)
    {
        PlayOneShot(clip, position);
    }

    public void PlayOneShotRandomPitchFromDictonary(string key, Vector3 position, bool single = false)
    {
        if (!AudioDictornary.ContainsKey(key))
        {
            Debug.LogError("Trying to start a clip that doesn't exist");
            return;
        }
        if (single == true && (SingleInstanceDictonary.ContainsKey(key) && SingleInstanceDictonary[key] != null))
            return;
        GameObject gameObject = PlayOneShot(AudioDictornary[key], position);
        if (single == true)
            SingleInstanceDictonary[key] = gameObject;
    }

    private GameObject PlayOneShot(AudioClip clip, Vector3 pos)
    {
        var tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        var source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
        Destroy(tempGO, clip.length);
        return tempGO;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
