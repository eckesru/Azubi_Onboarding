using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    [SerializeField] private bool active = false;
    [SerializeField] private bool destroyOnTrigger = false;

    private AudioSource[] audioSources; 

    void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    void Start() {
        if (active) {
            SwitchAudioSourceState();
            SwitchAudioSourceState();
        }
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        SwitchAudioSourceState();

        if(destroyOnTrigger) {
            Destroy(gameObject);
        }
    }

    private void SwitchAudioSourceState() {
        foreach (AudioSource audioSource in audioSources) {
            if(active) {
                audioSource.Stop();
            } else {
                audioSource.Play();
            }
        }
        active = !active;
    }


}
