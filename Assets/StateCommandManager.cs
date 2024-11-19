using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateCommandManager : MonoBehaviour
{
    public AudioClip odishaClip;
    public AudioClip tamilnaduClip;
    public AudioClip maharashtraClip;
    public AudioClip kashmirClip;
    public AudioClip winClip;
    public AudioClip wrongClip;

    public GameObject gameOverDisplay;
    public Material odishaBlinkMaterial; // Blinking material for Odisha
    public Material tamilnaduBlinkMaterial; // Blinking material for Tamil Nadu
    public Material maharashtraBlinkMaterial; // Blinking material for Maharashtra
    public Material kashmirBlinkMaterial; // Blinking material for Kashmir

    private AudioSource audioSource;
    private string currentCommand;
    private bool isPlayerInCorrectArea;
    private bool hasPlayerMadeChoice;
    private int score;
    private float gameStartTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        gameStartTime = Time.time;
        gameOverDisplay.SetActive(false); 
        StartCoroutine(PlayCommands());
    }

    IEnumerator PlayCommands()
    {
        yield return PlayCommand("odisha", odishaClip);
        yield return new WaitForSeconds(5); 
        yield return PlayCommand("tamilnadu", tamilnaduClip);
        yield return new WaitForSeconds(5);
        yield return PlayCommand("maharashtra", maharashtraClip);
        yield return new WaitForSeconds(5);
        yield return PlayCommand("kashmir", kashmirClip);
        yield return new WaitForSeconds(5);

        // Repeat commands if score is not yet 5
        while (score < 5)
        {
            yield return PlayCommand("odisha", odishaClip);
            yield return new WaitForSeconds(5);
            yield return PlayCommand("tamilnadu", tamilnaduClip);
            yield return new WaitForSeconds(5);
            yield return PlayCommand("maharashtra", maharashtraClip);
            yield return new WaitForSeconds(5);
            yield return PlayCommand("kashmir", kashmirClip);
            yield return new WaitForSeconds(5);
        }

        // If the score reaches 5, load the main menu
        SceneManager.LoadScene("mainmenu");
    }

    IEnumerator PlayCommand(string command, AudioClip clip)
    {
        currentCommand = command;
        hasPlayerMadeChoice = false; // Reset the choice flag
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);

        float commandStartTime = Time.time;

        while (Time.time < commandStartTime + 20)
        {
            if (hasPlayerMadeChoice)
            {
                break;
            }
            yield return null;
        }

        if (!hasPlayerMadeChoice)
        {
            audioSource.PlayOneShot(wrongClip);
            Debug.Log("No choice made. Wrong area. No score change.");
            yield return new WaitForSeconds(wrongClip.length);
        }

        currentCommand = null;

        // Check if 2.5 minutes have passed and score is less than 5
        if (Time.time - gameStartTime > 145 && score < 5)
        {
            audioSource.PlayOneShot(wrongClip);
            Debug.Log("Time over. Game over.");
            gameOverDisplay.SetActive(true);
            yield return new WaitForSeconds(wrongClip.length);
            SceneManager.LoadScene("mainmenu");
        }


        
    }

    void OnTriggerStay(Collider other)
    {
        if (currentCommand != null && !hasPlayerMadeChoice)
        {
            string areaTag = other.gameObject.tag;

            if (currentCommand !=null && other.CompareTag(currentCommand)) {
                isPlayerInCorrectArea = true;
                hasPlayerMadeChoice = true;
                audioSource.PlayOneShot(winClip);
                Debug.Log("Right area. Score increased.");
                score++;
                StartCoroutine(BlinkEffect(other.gameObject,currentCommand));
                StartCoroutine(WaitAfterWinAudio());

            }
            else if(currentCommand == "kashmir" && ((other.CompareTag("odisha")| (other.CompareTag("tamilnadu")| (other.CompareTag("maharashtra"))))))
            {
                isPlayerInCorrectArea = false;
                hasPlayerMadeChoice = true;
                audioSource.PlayOneShot(wrongClip);
                Debug.Log("Wrong area. No score change.");
            }
            else if (currentCommand == "odisha" && ((other.CompareTag("kashmir") | (other.CompareTag("tamilnadu") | (other.CompareTag("maharashtra"))))))
            {
                isPlayerInCorrectArea = false;
                hasPlayerMadeChoice = true;
                audioSource.PlayOneShot(wrongClip);
                Debug.Log("Wrong area. No score change.");
            }
            else if (currentCommand == "tamilnadu" && ((other.CompareTag("odisha") | (other.CompareTag("kashmir") | (other.CompareTag("maharashtra"))))))
            {
                isPlayerInCorrectArea = false;
                hasPlayerMadeChoice = true;
                audioSource.PlayOneShot(wrongClip);
                Debug.Log("Wrong area. No score change.");
            }
            else if (currentCommand == "maharashtra" && ((other.CompareTag("odisha") | (other.CompareTag("tamilnadu") | (other.CompareTag("kashmir"))))))
            {
                isPlayerInCorrectArea = false;
                hasPlayerMadeChoice = true;
                audioSource.PlayOneShot(wrongClip);
                Debug.Log("Wrong area. No score change.");
            }
        }
    }
    IEnumerator WaitAfterWinAudio()
    {
        yield return new WaitForSeconds(winClip.length + 2); // Wait for winClip length + 2 seconds
    }

    void OnTriggerExit(Collider other)
    {
        if (currentCommand != null && other.CompareTag(currentCommand))
        {
            isPlayerInCorrectArea = false;
        }
    }

    IEnumerator BlinkEffect(GameObject areaObject, string command)
    {
        Renderer areaRenderer = areaObject.GetComponent<Renderer>();
        if (areaRenderer != null)
        {
            Material originalMaterial = areaRenderer.material;
            Material blinkMaterial = null;

            switch (command)
            {
                case "odisha":
                    blinkMaterial = odishaBlinkMaterial;
                    break;
                case "tamilnadu":
                    blinkMaterial = tamilnaduBlinkMaterial;
                    break;
                case "maharashtra":
                    blinkMaterial = maharashtraBlinkMaterial;
                    break;
                case "kashmir":
                    blinkMaterial = kashmirBlinkMaterial;
                    break;
            }

            if (blinkMaterial != null)
            {
                float blinkDuration = 3f;
                float blinkInterval = 0.3f;
                float endTime = Time.time + blinkDuration;

                while (Time.time < endTime)
                {
                    areaRenderer.material = blinkMaterial;
                    yield return new WaitForSeconds(blinkInterval);
                    areaRenderer.material = originalMaterial;
                    yield return new WaitForSeconds(blinkInterval);
                }
            }
        }
    }
}
