using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuController : MonoBehaviour
{
    public GameObject leftHand; // Assign the left hand GameObject in the inspector
    public GameObject quitButton;
    public GameObject importButton;
    public GameObject lockButton;
    public GameObject GameState;
    private bool isLockActionTriggered = false; 


    public void EnableMenu()
    {
        gameObject.SetActive(true);
    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);
    }   

    private void Start()
    {
        // Assign this HandMenuController to each ButtonCollisionHandler
        quitButton.GetComponent<ButtonCollisionHandler>().handMenuController = this;
        importButton.GetComponent<ButtonCollisionHandler>().handMenuController = this;
        lockButton.GetComponent<ButtonCollisionHandler>().handMenuController = this;
    }

    private void Update()
    {
        // Position the HandMenu at the left hand's position and rotation
        if (leftHand != null)
        {
            // make it be 10 cm above in the y axis
            transform.position = leftHand.transform.position + new Vector3(0, 0.1f, 0);
            transform.rotation = leftHand.transform.rotation;
            // make it look at the camera + 180 degrees
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }

    public void OnButtonCollision(GameObject collidedButton)
    {
        if (collidedButton == quitButton)
        {
            QuitGame();
        }
        else if (collidedButton == importButton)
        {
            ImportAction();
        }
        else if (collidedButton == lockButton)
        {
            isLockActionTriggered = true; // Set the flag to true to prevent multiple triggers
            LockAction();
        }
    }
    public void OnButtonExit(GameObject collidedButton)
    {
        if (collidedButton == lockButton)
        {
            isLockActionTriggered = false; // Set the flag to true to prevent multiple triggers
        }
    }



    private void QuitGame()
    {
        Debug.Log("Quit Game Selected");
        #if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
        #else
          Application.Quit();
        #endif
    }

    private void ImportAction()
    {
        Debug.Log("Import Action Selected");
    }

    private void LockAction()
    {
        Debug.Log("Lock Action Selected");

        GameState.GetComponent<GameState>().tableIsLocked = !GameState.GetComponent<GameState>().tableIsLocked;
        lockButton.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = GameState.GetComponent<GameState>().tableIsLocked ? "Unlock" : "Lock";

    }
}
