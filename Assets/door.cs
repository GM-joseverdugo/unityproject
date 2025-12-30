using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingEntry : MonoBehaviour
{
    [Header("Entry Settings")]
    [SerializeField] private string interiorSceneName = "BuildingInterior";
    [SerializeField] private bool useSceneTransition = true;

    [Header("Interior Reveal (if not using scenes)")]
    [SerializeField] private GameObject interiorRoom;
    [SerializeField] private GameObject exteriorObjects;
    [SerializeField] private Transform interiorSpawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip doorSound;

    private GameObject player;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (!useSceneTransition && interiorRoom != null)
            interiorRoom.SetActive(false);
    }

    void EnterBuilding()
    {
        // Play door sound
        if (doorSound != null && audioSource != null)
            audioSource.PlayOneShot(doorSound);

        if (useSceneTransition)
        {
            // Load interior scene
            SceneManager.LoadScene(interiorSceneName);
        }
        else
        {
            // Reveal interior in same scene
            RevealInterior();
        }
    }

    void RevealInterior()
    {
        if (interiorRoom != null)
            interiorRoom.SetActive(true);

        if (exteriorObjects != null)
            exteriorObjects.SetActive(false);

        // Move player to interior spawn point
        if (player != null && interiorSpawnPoint != null)
        {
            player.transform.position = interiorSpawnPoint.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            EnterBuilding();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
