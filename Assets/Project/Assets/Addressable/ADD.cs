using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Oculus.Interaction; // For OVRGrabbable and GrabFreeTransformer
using Oculus.Interaction.HandGrab; // For HandGrabInteractable
using Oculus.Interaction.Grab; // For GrabInteractable

public class ADD : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject _environment; // Reference to the addressable object
    [SerializeField] Button _instantiateButton; // Reference to the UI button

    private GameObject _instantiatedAddressable; // To hold the reference to the instantiated Addressable

    private void Start()
    {
        // Ensure the button is assigned and add the listener
        if (_instantiateButton != null)
        {
            _instantiateButton.onClick.AddListener(OnButtonPress);
        }
    }

    // Method that gets called when the button is pressed
    public void OnButtonPress()
    {
        InstantiateAddressable();
    }

    // Instantiate the addressable at a specified position
    private void InstantiateAddressable()
    {
        // Randomly choose a Y value between 1.063676f and 1.6f
        float randomY = Random.Range(1.063676f, 1.6f);

        // Load the addressable object asynchronously at the fixed X and Z, and the random Y
        var handle = _environment.InstantiateAsync(new Vector3(-0.14126873016357423f, randomY, 0.8616613149642944f), Quaternion.identity);

        handle.Completed += (op) =>
        {
            // Once the object is instantiated, set its scale to the specified size
            op.Result.transform.localScale = new Vector3(2.8126230239868166f, 2.8126230239868166f, 2.8126230239868166f);
            _instantiatedAddressable = op.Result.gameObject; // Store the reference to the instantiated GameObject

            // --- Delete Button Logic ---
            Button deleteButton = _instantiatedAddressable.GetComponentInChildren<Button>();
            if (deleteButton != null && deleteButton.name == "DeleteButton") // Assuming the delete button is named "DeleteButton"
            {
                deleteButton.onClick.AddListener(() => DeleteInstantiatedAddressable(_instantiatedAddressable));
            }

            // --- Lock Button Logic ---
            // Find the LockButton. Assuming it's a child of the instantiated addressable.
            Button lockButton = _instantiatedAddressable.transform.Find("LockButton")?.GetComponent<Button>();
            if (lockButton != null)
            {
                lockButton.onClick.AddListener(() => ToggleObjectLock(_instantiatedAddressable));
            }
        };
    }

    // Method to delete the instantiated Addressable
    public void DeleteInstantiatedAddressable(GameObject instanceToRelease)
    {
        if (instanceToRelease != null)
        {
            Addressables.ReleaseInstance(instanceToRelease);
            _instantiatedAddressable = null; // Clear the reference after releasing
        }
    }

    // Method to toggle the lock state of the object
    public void ToggleObjectLock(GameObject targetObject)
    {
        if (targetObject == null) return;

        // Get the relevant components
        HandGrabInteractable handGrabInteractable = targetObject.GetComponent<HandGrabInteractable>();
        GrabInteractable grabInteractable = targetObject.GetComponent<GrabInteractable>();
        Rigidbody rb = targetObject.GetComponent<Rigidbody>();

        // Toggle the enabled state of interactable scripts
        if (handGrabInteractable != null)
        {
            handGrabInteractable.enabled = !handGrabInteractable.enabled;
            Debug.Log($"HandGrabInteractable enabled: {handGrabInteractable.enabled}");
        }
        else
        {
            Debug.LogWarning("HandGrabInteractable script not found on the instantiated object.");
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = !grabInteractable.enabled;
            Debug.Log($"GrabInteractable enabled: {grabInteractable.enabled}");
        }
        else
        {
            Debug.LogWarning("GrabInteractable script not found on the instantiated object.");
        }

        // Toggle Rigidbody isKinematic property for physical locking
        if (rb != null)
        {
            rb.isKinematic = !rb.isKinematic;
            Debug.Log($"Rigidbody isKinematic: {rb.isKinematic}");

            // Optionally, if you want to freeze position/rotation completely when locked
            if (rb.isKinematic) // If becoming kinematic (locked)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                rb.velocity = Vector3.zero;
#pragma warning restore CS0618 // Type or member is obsolete
                rb.angularVelocity = Vector3.zero;
                // You might also want to set constraints here if needed, e.g.,
                // rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            // else // If becoming non-kinematic (unlocked)
            // {
            //     rb.constraints = RigidbodyConstraints.None;
            // }
        }
        else
        {
            Debug.LogWarning("Rigidbody not found on the instantiated object.");
        }

        // Optionally, you might still want to toggle OVRGrabbable and GrabFreeTransformer
        // as they are part of the interaction system, even if not the primary controllers.
        OVRGrabbable ovrGrabbable = targetObject.GetComponent<OVRGrabbable>();
        GrabFreeTransformer grabFreeTransformer = targetObject.GetComponent<GrabFreeTransformer>();

        if (ovrGrabbable != null)
        {
            ovrGrabbable.enabled = !ovrGrabbable.enabled;
            Debug.Log($"OVRGrabbable enabled: {ovrGrabbable.enabled}");
        }

        if (grabFreeTransformer != null)
        {
            grabFreeTransformer.enabled = !grabFreeTransformer.enabled;
            Debug.Log($"GrabFreeTransformer enabled: {grabFreeTransformer.enabled}");
        }
    }
}
