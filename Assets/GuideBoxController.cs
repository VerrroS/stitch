using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideBoxController : MonoBehaviour
{
    // Reference to the GuideBox GameObject
    public GameObject guideBox;
    // Reference to the TextMeshProUGUI component inside the Textbox GameObject
    public TextMeshProUGUI textbox;
    public Transform guideboxTransform;



    // Start is called before the first frame update
    void Start()
    {
        // Ensure the GuideBox is hidden initially
        guideBox.SetActive(false);

        if (guideboxTransform != null)
        {
            // make it 1 meter above in the y axis
            guideBox.transform.position = guideboxTransform.position;
            guideBox.transform.position += new Vector3(0, 1, 0);
        }
        
    }

    // Function to show the GuideBox with a given message
    public void ShowMessage(string message)
    {
        textbox.text = message;
        guideBox.SetActive(true);
        // make it face the camera
        guideBox.transform.LookAt(Camera.main.transform);
        guideBox.transform.Rotate(180, 0, 0);
    }

    // Function to hide the GuideBox
    public void HideMessage()
    {
        guideBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // This can be used to add any real-time functionality if needed
    }
}
