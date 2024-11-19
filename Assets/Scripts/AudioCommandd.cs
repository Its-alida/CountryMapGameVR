using System.Collections;
using UnityEngine;

public class AudioCommandd : MonoBehaviour
{
    public AudioClip kashmirClip;
    public AudioClip odishaClip;
    public AudioClip tamilnaduClip;
    public AudioClip maharashtraClip;
    public AudioClip winClip;
    public AudioClip wrongClip;

    private AudioSource audioSource;
    private string currentCommand;
    private bool correctStateReached;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayCommands());
    }

    IEnumerator PlayCommands()
    {
        yield return PlayCommand("Kashmir", kashmirClip);
        yield return PlayCommand("Odisha", odishaClip);
        yield return PlayCommand("Tamilnadu", tamilnaduClip);
        yield return PlayCommand("Maharashtra", maharashtraClip);
    }

    IEnumerator PlayCommand(string command, AudioClip clip)
    {
        currentCommand = command;
        correctStateReached = false;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);

        float startTime = Time.time;

        while (Time.time < startTime + 10)
        {
            if (correctStateReached)
            {
                audioSource.PlayOneShot(winClip);
                yield return new WaitForSeconds(winClip.length);
                break;
            }
            yield return null;
        }

        if (!correctStateReached)
        {
            audioSource.PlayOneShot(wrongClip);
            yield return new WaitForSeconds(wrongClip.length);
        }

        currentCommand = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string stateTag = gameObject.tag;

            if (currentCommand != null && stateTag.Equals(currentCommand, System.StringComparison.OrdinalIgnoreCase))
            {
                correctStateReached = true;
            }
            else if (currentCommand != null)
            {
                audioSource.PlayOneShot(wrongClip);
            }
        }
    }
}
