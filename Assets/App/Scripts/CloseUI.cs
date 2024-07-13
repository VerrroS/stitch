using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClosed()
    {
        StartCoroutine(ScaleDownOverTime(0.6f));
    }
    public void OnOpened()
    {
        gameObject.SetActive(true);
        StartCoroutine(ScaleUpOverTime(0.6f));
    }


    IEnumerator ScaleDownOverTime(float duration)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        gameObject.SetActive(false);
    }



    IEnumerator ScaleUpOverTime(float duration)
    {
        Vector3 targetScale = new Vector3(1,1,1);
        transform.localScale = Vector3.zero; // Start from zero scale
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
