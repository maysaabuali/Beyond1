using UnityEngine;
using UnityEngine.UI; // Import UI namespace for button components

public class DestroyObjectOnClick : MonoBehaviour
{
    [SerializeField] private Button _destroyButton; // Reference to the button
    [SerializeField] private GameObject _objectToDestroy; // The object to destroy when the button is clicked

    private void Start()
    {
        // Ensure the button is assigned and add the listener for the click event
        if (_destroyButton != null)
        {
            _destroyButton.onClick.AddListener(DestroyObject);
        }
        else
        {
            Debug.LogWarning("Button is not assigned!");
        }
    }

    // Method to destroy the object
   public void DestroyObject()
    {
        if (_objectToDestroy != null)
        {
            Destroy(_objectToDestroy);
        }
        else
        {
            Debug.LogWarning("No object to destroy!");
        }
    }
}
