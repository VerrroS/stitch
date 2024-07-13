using UnityEngine;

public class AlignToObject : MonoBehaviour
{
    public GameObject targetObject; // The object to align to
    public GameObject objectToAlign; // The object to be aligned
    public float offset = -0.06f; // Small offset to ensure the object lies on top
    public float desiredYRotationOffset = 90f; // Desired Y-axis rotation offset


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
        alignedPosition.y = targetBounds.max.y + (alignBounds.size.y / 2 ) + offset;
        Debug.Log("The SNAP parent-  now aligned position was set");

        // Set the position of the object to align
        objectToAlign.transform.position = alignedPosition;



        // rotate to object to align to the y axis of the target object

        //Quaternion targetRotation = targetObject.transform.rotation;
        //Vector3 targetEulerAngles = targetRotation.eulerAngles;
        //Quaternion desiredRotationQuaternion = Quaternion.Euler(0, desiredYRotationOffset, 0);
        //Debug.Log("The SNAP parent-  new roatation is " + desiredRotationQuaternion);
        //objectToAlign.transform.rotation = desiredRotationQuaternion;
        objectToAlign.transform.RotateAround(objectToAlign.transform.position, Vector3.up, 64);


        Debug.Log("Object aligned on top of the target object with offset.");
    }
}
