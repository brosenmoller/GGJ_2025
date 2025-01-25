using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    string[] lines;
    [SerializeField]
    string[] linesK;
    [SerializeField]
    float textSpeed;

    int index;

    [SerializeField]
    AudioClip clip;
    AudioSource source;

    [Range(-3, 3),SerializeField]
    float MinPitch;
    [Range(-3, 3), SerializeField]
    float MaxPitch;

    [SerializeField]
    GameObject ToEnable;
    [SerializeField]
    bool Unpause;



    

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        source = GetComponent<AudioSource>();
        lines = linesK;
        startDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void startDialogue()
    {
        StartCoroutine(WriteLine());
    }

    public void SetLines(string[] newLines)
    {
        lines = newLines;
        index = 0;
        text.text = "";
    }

    IEnumerator WriteLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            if(c != ' ')
            {
                playBeep();
            }
            yield return new WaitForSeconds(textSpeed);
        }
    }


    void Next()
    {
        if(index < lines.Length-1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            gameObject.SetActive(false);
            ToEnable.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(text.text == lines[index])
        {
            Next();
        }
        else
        {
            StopAllCoroutines();
            text.text = lines[index];
        }
    }


    void playBeep()
    {
        source.pitch = Random.Range(MinPitch, MaxPitch);
        source.PlayOneShot(clip);
    }
}
