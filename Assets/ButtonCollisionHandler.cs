using UnityEngine;

public class ButtonCollisionHandler : MonoBehaviour
{
    public HandMenuController handMenuController; // Reference to the parent HandMenuController

    private void OnTriggerEnter(Collider other)
    {
        //print the layer name of what the name of the colided object is
        Debug.Log("COllided with: " + other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            if (handMenuController != null)
            {
                handMenuController.OnButtonCollision(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print the layer name of what the name of the colided object is
        Debug.Log("COllided with: " + other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            if (handMenuController != null)
            {
                handMenuController.OnButtonExit(this.gameObject);
            }
        }
    }
    }
}
