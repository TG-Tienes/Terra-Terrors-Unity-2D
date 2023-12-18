using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform scrollArea;

    // Adjust this value to control the scroll speed
    public float scrollSpeed = 1.0f;

    void Update()
    {
        // Check if the mouse is over the scroll area
        if (IsMouseOverScrollArea())
        {
            // Get the scroll input from the mouse
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            // Update the scroll position of the ScrollRect
            UpdateScrollPosition(scrollInput);
        }
    }

    void UpdateScrollPosition(float scrollInput)
    {
        // Access the vertical scrollbar of the ScrollRect
        Scrollbar verticalScrollbar = scrollRect.verticalScrollbar;

        // Modify the scrollbar's value based on the scroll input
        verticalScrollbar.value += scrollInput * scrollSpeed;

        // Clamp the scrollbar's value to ensure it stays within valid range (0 to 1)
        verticalScrollbar.value = Mathf.Clamp(verticalScrollbar.value, 0.0f, 1.0f);
    }

    bool IsMouseOverScrollArea()
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to local coordinates of the scroll area
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollArea, mousePosition, null, out Vector2 localMousePosition);

        // Check if the local mouse position is within the scroll area
        return scrollArea.rect.Contains(localMousePosition);
    }
}