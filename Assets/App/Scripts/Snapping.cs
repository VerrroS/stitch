using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour
{
    public float snapOffset = 0; // Adjustable offset to keep a small distance above the table

    public static event System.Action OnSnapped;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the tag "2DPattern"
        if (other.CompareTag("2DPattern"))
        {
            SnapToTable(other.transform);
        }
    }

    private void SnapToTable(Transform patternTransform)
    {
        // Get the closest point on the table's collider to the object's position
        Collider tableCollider = GetComponent<Collider>();
        Vector3 closestPoint = tableCollider.ClosestPoint(patternTransform.position);

        // Get the BoxCollider component
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // Calculate the y position of the top surface of the table's collider
        float tableTopY = boxCollider.bounds.max.y;

        // Optional: If you need to take into account the local scale and position, you can modify it accordingly
        tableTopY = boxCollider.center.y + (boxCollider.size.y / 2 * transform.localScale.y) + transform.position.y;


        // Set the object's position to keep x and z the same, but adjust the y position
        Vector3 newPosition = new Vector3(patternTransform.position.x, (tableTopY + snapOffset)/2 + 0.17f, patternTransform.position.z);

  
        // Call the Snap function of PatternPart to set the final position
        PatternPart patternPart = patternTransform.GetComponentInParent<PatternPart>();
        if (patternPart != null && !patternPart.isSnapped)
        {
            //patternPart.Snap(newPosition);
        }

        OnSnapped?.Invoke();
    }
}
