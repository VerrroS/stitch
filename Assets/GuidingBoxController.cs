using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingBoxController : MonoBehaviour
{
    public GameObject guidingBox;
    private GameObject guidingBoxChild;
    private GameObject guidingBoxtexts;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Guiding Box Controller Started");

        // wait for 3 seconds and then show the guiding box
        StartCoroutine(ShowGuidingBox());
        Debug.Log("Guiding Box Controller finished");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showGuideBoxWithText(string guidingText = "This is a guiding box")
    {
        // get the child called TellJourStoryPanel
        guidingBoxChild = transform.Find("TellYourStoryPanel").gameObject;

        foreach (Transform child in transform)
        {
            if (child.name == "TellYourStory -TMPro")
            {
                child.GetComponent<TMPro.TextMeshPro>().text = guidingText;
            }
        }

        // set the guide box to active
        guidingBox.SetActive(true);
        // rotate the guide box to face the camera
        guidingBox.transform.LookAt(Camera.main.transform);

        // Start the movement coroutine
        StartCoroutine(MoveBox(Vector3.up * 0.2f, 1.0f));
    }

    IEnumerator ShowGuidingBox()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        // show the guiding box with text
        showGuideBoxWithText();
    }

    IEnumerator MoveBox(Vector3 direction, float duration)
    {
        Vector3 startPosition = guidingBox.transform.position;
        Vector3 endPosition = startPosition + direction;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            guidingBox.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        guidingBox.transform.position = endPosition;
    }

    public void hideGuideBox()
    {
        // Start the movement coroutine to move the box down
        StartCoroutine(MoveBox(Vector3.down * 0.2f, 1.0f));
        StartCoroutine(HideAfterMovement());
    }

    IEnumerator HideAfterMovement()
    {
        // wait for the movement to finish
        yield return new WaitForSeconds(1.0f);

        // set the guide box to inactive
        guidingBox.SetActive(false);
    }
}
