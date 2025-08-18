using UnityEngine;
using UnityEngine.UI;

public class ToggleList : MonoBehaviour
  // Make sure you import this to use UI elements


{
    public Button toggleButton;  // The button in your scene
    public GameObject targetObject;  // The game object you want to toggle

    private bool isVisible = false;

    void Start()
    {
        // Ensure the game object is initially hidden
        targetObject.SetActive(false);

        // Add listener to the button to call ToggleVisibility when clicked
        toggleButton.onClick.AddListener(ToggleVisibilityState);
    }

    // Toggle the visibility each time the button is pressed
    void ToggleVisibilityState()
    {
        isVisible = !isVisible;
        targetObject.SetActive(isVisible);
    }
}
