using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayer : MonoBehaviour
{
    public AudioClip placementSound;
    public AudioSource audioSource;

    public static AudioPlayer Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

    }

    public void PlayPlacementSound()
    {
        if(placementSound != null)
        {
            audioSource.PlayOneShot(placementSound);
        }
    }
}
