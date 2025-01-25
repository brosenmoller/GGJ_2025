using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, AudioClip> AudioDictornary;
    public static AudioManager Instance { get; private set; }
    public static bool Exists { get { return Instance != null; } }

    public static WaitUntil waitUntil = new WaitUntil(() => Exists);

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        Instance = this;
    }

    public void PlayOneShotRandomPitch(AudioClip clip, Vector3 position)
    {
        PlayOneShot(clip, position);
    }

    public void PlayOneShotRandomPitchFromDictonary(string key, Vector3 position)
    {
        PlayOneShot(AudioDictornary[key], position);
    }

    private void PlayOneShot(AudioClip clip,Vector3 pos)
    {
        var tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        var source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.pitch = Random.Range(0.5f, 1.5f);
        source.Play();
        Destroy(tempGO, clip.length);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
