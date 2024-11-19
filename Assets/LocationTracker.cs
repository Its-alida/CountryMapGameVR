using UnityEngine;

public class LocationTracker : MonoBehaviour
{
    private string currentArea;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string areaTag = other.gameObject.tag;
            Debug.Log($"Player entered {areaTag}");
            currentArea = areaTag;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("kashmir"))
        {
            string areaTag = other.gameObject.tag;
            Debug.Log($"Player is in kashmir");
            currentArea = areaTag;
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string areaTag = other.gameObject.tag;
            Debug.Log($"Player exited {areaTag}");
            if (currentArea == areaTag)
            {
                currentArea = null;
            }
        }
    }*/

    public string GetCurrentArea()
    {
        return currentArea;
    }
}
