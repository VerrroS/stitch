using UnityEngine;

public class AlignToObject : MonoBehaviour
{
    public GameObject targetObject; // The object to align to
    public GameObject objectToAlign; // The object to be aligned
    public float offset = 0.01f; // Small offset to ensure the object lies on top

    void Start()
    {

        //AlignObject();
        objectToAlign.SetActive(false);
    }

    public void AlignObject(GameObject gameObject)
    {
        objectToAlign.SetActive(true);

        targetObject = gameObject;
        // Get the bounds of the target object
        Bounds targetBounds = targetObject.GetComponent<Renderer>().bounds;

        // Get the bounds of the object to align
        Bounds alignBounds = objectToAlign.GetComponent<Renderer>().bounds;

        // Calculate the position to place the object to align on top of the target object
        Vector3 alignedPosition = targetBounds.center;
        alignedPosition.y = targetBounds.max.y + (alignBounds.size.y / 2) + offset;

        // Set the position of the object to align
        objectToAlign.transform.position = alignedPosition;

        // Optionally, match the rotation of the target object
        objectToAlign.transform.rotation = targetObject.transform.rotation;

        Debug.Log("Object aligned on top of the target object with offset.");
    }
}
