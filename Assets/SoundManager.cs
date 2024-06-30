using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip snapSound;
    public AudioClip transformSound;
    public AudioClip chooseWorkplaceSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Listen to Snap event
        SnapIngEvent.OnSnap += PlaySnapSound;
        // Listen to Transform event
        PatternPart.OnTransformed += PlayTransformSound;
        // Listen to ChooseWorkplace event
        AnchorIT.OnChoosenWorkspace += PlayChooseWorkplaceSound;

    }

    public void PlaySnapSound()
    {
        audioSource.PlayOneShot(snapSound);
        Debug.Log("Snap sound played");
    }

    public void PlayTransformSound()
    {
        audioSource.PlayOneShot(transformSound);
    }

    public void PlayChooseWorkplaceSound()
    {
        audioSource.PlayOneShot(chooseWorkplaceSound);
        Debug.Log("Choose workplace sound played");
    }
}