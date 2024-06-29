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
    private Renderer guideBoxRenderer;

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

        // Get the Renderer component for the GuideBox
        guideBoxRenderer = guideBox.GetComponent<Renderer>();

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
        StartCoroutine(GlideAndFadeIn(targetPosition));
    }

    // Function to hide the GuideBox
    public void HideMessage()
    {
        StartCoroutine(GlideBack(initialPosition));
    }

    // Coroutine to glide and fade in the GuideBox to a target position
    private IEnumerator GlideAndFadeIn(Vector3 targetPos)
    {
        float elapsedTime = 0;

        Vector3 startingPos = guideBox.transform.position;
        Color startColor = guideBoxRenderer.material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1);

        Color textStartColor = textbox.color;
        Color textTargetColor = new Color(textStartColor.r, textStartColor.g, textStartColor.b, 1);

        while (elapsedTime < glideDuration)
        {
            float t = elapsedTime / glideDuration;
            guideBox.transform.position = Vector3.Lerp(startingPos, targetPos, t);
            guideBoxRenderer.material.color = Color.Lerp(new Color(startColor.r, startColor.g, startColor.b, 0), targetColor, t);
            textbox.color = Color.Lerp(new Color(textStartColor.r, textStartColor.g, textStartColor.b, 0), textTargetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        guideBox.transform.position = targetPos;
        guideBoxRenderer.material.color = targetColor;
        textbox.color = textTargetColor;
    }

    // Coroutine to glide the GuideBox back to initial position
    private IEnumerator GlideBack(Vector3 targetPos)
    {
        float elapsedTime = 0;

        Vector3 startingPos = guideBox.transform.position;

        while (elapsedTime < glideDuration)
        {
            float t = elapsedTime / glideDuration;
            guideBox.transform.position = Vector3.Lerp(startingPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        guideBox.transform.position = targetPos;
        guideBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // This can be used to add any real-time functionality if needed
    }
}
