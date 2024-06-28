using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuFSM : MonoBehaviour
{
    private const float translationTime = 0.3f;

    GameObject displayedMenu;
    public Vector3 initialPosition;

    private bool buttonPressed = false;
    private string buttonName;
    public void SetButton(string pressedButtonName)
    {
        // if to avoid multiple presses.
        if (buttonPressed == false)
        {
            buttonPressed = true;
            buttonName = pressedButtonName;
        }
    }

    private void Start()
    {
        this.transform.position = initialPosition;
        StartCoroutine(TranslateMenu("MainMenu"));
    }

    private IEnumerator TranslateMenu(string menuName)
    {
        float currentTime = 0f;

        if (displayedMenu != null)
        {
            SetMenuColliders(false);

            while (currentTime < translationTime)
            {
                float t = currentTime / translationTime;
                displayedMenu.transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(0, 0, 0), t);
                currentTime += Time.deltaTime;
                yield return null;
            }

            displayedMenu.SetActive(false);
        }
        for (int idx = 0; idx < transform.childCount; idx++)
        {
            if (transform.GetChild(idx).name == menuName)
                displayedMenu = transform.GetChild(idx).gameObject;
        }

        displayedMenu.transform.localScale = new Vector3(0, 0, 0);
        displayedMenu.SetActive(true);

        currentTime = 0f;
        while (currentTime < translationTime)
        {
            float t = currentTime / translationTime;
            displayedMenu.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), t);
            currentTime += Time.deltaTime;
            yield return null;
        }
        SetMenuColliders(true);
    }

    private void SetMenuColliders(bool colliderState)
    {
        foreach (var collider in displayedMenu.transform.GetComponentsInChildren<Collider>()) 
        {
            collider.enabled = colliderState;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            buttonPressed = false;
            UpdateState();
        }
    }

    private void UpdateState()
    {
        switch (buttonName)
        {
            case "Button_Quit":
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
            case "Button_Play":
                StartCoroutine(TranslateMenu("PlayMenu"));
                break;
            case "Button_Training":
                SceneManager.LoadScene("Training");
                break;
            case "Button_SelectBending":
                StartCoroutine(TranslateMenu("SelectBending"));
                break;
            case "Button_Back":
                StartCoroutine(TranslateMenu("MainMenu"));
                break;
            case "Button_Earth":
                Debug.Log("Earth bending selected");
                break;
            default: break;
        }
    }
}
