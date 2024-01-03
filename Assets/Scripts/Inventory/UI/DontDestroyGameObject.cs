using UnityEngine;

public class DontDestroyGameObject : MonoBehaviour
{
    void Start()
    {
        // Check if there's already an instance of this script in the scene
        // DontDestroyGameObject[] instances = FindObjectsOfType<DontDestroyGameObject>();

        // // If there's more than one instance, destroy this instance to keep only one
        // if (instances.Length > 1)
        // {
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     // Mark the GameObject to persist across scene changes
        //     DontDestroyOnLoad(gameObject);
        // }

        DontDestroyOnLoad(gameObject);
    }
}