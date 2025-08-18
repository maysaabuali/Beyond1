using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // The object to toggle
    [SerializeField] private GameObject targetObject2; // The object to toggle
    public void ToggleActive()
    {
        if (targetObject != null&& targetObject2 != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
            targetObject2.SetActive(!targetObject2.activeSelf);
        }
    }
}
