using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTransparencyManager : MonoBehaviour
{
    public Transform playerTransform;
    public float maxTransparency = 0.5f;
    public string tagToAffect = "Tree";

    void Start()
    {
        if (playerTransform == null)
        {
            // If the playerTransform is not assigned, assume the main camera's parent
            playerTransform = Camera.main.transform.parent;
        }
    }

    void Update()
    {
        HandleObjectTransparency();
    }

    void HandleObjectTransparency()
    {
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (Renderer objectRenderer in renderers)
        {
            // Check if the object has the specified tag
            if (!objectRenderer.gameObject.CompareTag(tagToAffect))
            {
                continue; // Skip objects without the specified tag
            }
            if (IsBetweenCameraAndPlayer(objectRenderer.transform))
            {
                SetObjectTransparency(objectRenderer, maxTransparency);
            }
            else
            {
                SetObjectTransparency(objectRenderer, 1f);
            }
        }
    }

    bool IsBetweenCameraAndPlayer(Transform objectTransform)
    {
        Vector3 cameraToPlayer = playerTransform.position - Camera.main.transform.position;
        Vector3 cameraToObject = objectTransform.position - Camera.main.transform.position;

        // Ignore the vertical component by setting the y component to 0
        cameraToPlayer.z = 0f;
        cameraToObject.z = 0f;

        // Check if the object is between the camera and the player using dot products
        float dotProduct = Vector3.Dot(cameraToPlayer.normalized, cameraToObject.normalized);

        // Use the cross product to determine if the object is on the left or right side
        Vector3 crossProduct = Vector3.Cross(cameraToPlayer.normalized, cameraToObject.normalized);

        // Determine the orientation based on the sign of the cross product
        float orientation = Mathf.Sign(crossProduct.y);

        // Debug logs for inspection
        Debug.Log("Dot Product: " + dotProduct);
        Debug.Log("Orientation: " + orientation);
        Debug.Log("Is Between Camera And Player: " + (dotProduct > 0f && orientation > 0f));

        // Return true only if the dot product is positive and the orientation is positive
        return dotProduct > 0f && orientation > 0f;
    }

    void SetObjectTransparency(Renderer objectRenderer, float alpha)
    {
        Color currentColor = objectRenderer.material.color;
        currentColor.a = alpha;
        objectRenderer.material.color = currentColor;
    }
}