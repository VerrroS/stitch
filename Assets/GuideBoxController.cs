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

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float hightToFloatUp = 0.2f;

    // Duration for the glide effect
    private float glideDuration = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the GuideBox is hidden initially
        guideBox.SetActive(false);

        if (guideboxTransform != null)
        {
            // Set the initial and target positions
            initialPosition = guideboxTransform.position + new Vector3(0, 1, 0);
            targetPosition = initialPosition + new Vector3(0, hightToFloatUp, 0);
            guideBox.transform.position = initialPosition;
        }
    }

    // Function to show the GuideBox with a given message
    public void ShowMessage(string message)
    {
        textbox.text = message;
        guideBox.SetActive(true);
        guideBox.transform.LookAt(Camera.main.transform);
        guideBox.transform.Rotate(180, 0, 0);
        StartCoroutine(GlideToPosition(targetPosition));
    }

    // Function to hide the GuideBox
    public void HideMessage()
    {
        StartCoroutine(GlideToPosition(initialPosition, false));
    }

    // Coroutine to glide the GuideBox to a target position
    private IEnumerator GlideToPosition(Vector3 targetPos, bool showing = true)
    {
        float elapsedTime = 0;

        Vector3 startingPos = guideBox.transform.position;

        while (elapsedTime < glideDuration)
        {
            guideBox.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / glideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        guideBox.transform.position = targetPos;

        if (!showing)
        {
            guideBox.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // This can be used to add any real-time functionality if needed
    }
}
