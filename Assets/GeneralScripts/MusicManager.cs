using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public static bool Exists { get { return Instance != null; } }

    public static WaitUntil waitUntil = new(() => Exists);

    [SerializeField]
    private AudioClip slowMusic;
    [SerializeField]
    private AudioClip fastMusic;

    private AudioSource source;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        source = GetComponent<AudioSource>();
    }

    public void SwitchToFast()
    {
        source.clip = fastMusic;
        source.Play();
    }

    public void SwitchToSlow()
    {
        source.clip = slowMusic;
        source.Play();
    }
}
