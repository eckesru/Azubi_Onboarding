using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    [SerializeField] private bool active = false;
    [SerializeField] private bool destroyOnTrigger = false;
    [SerializeField] private bool muteMode = false;
    [SerializeField] private float destroyTime = 0f;
    private bool locked = false;

    private AudioSource[] audioSources; 

    void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    void Start() {
        if (active || muteMode) {
            SwitchAudioSourceState();
            SwitchAudioSourceState();
        }

    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {

        if (!locked) SwitchAudioSourceState();

        if(destroyOnTrigger) {
            locked = true;
            // Startet eine Coroutine, welche waehrend der Ausfuehrung ein oder mehrere return yields verarbeiten kann
            StartCoroutine(DestroyTrigger());
        }
    }

    private void SwitchAudioSourceState() {
        foreach (AudioSource audioSource in audioSources) {
            if(active) {
                if (muteMode){
                    audioSource.mute = true;
                } else {
                audioSource.Stop();
                }
            } else {
                if (muteMode){
                    audioSource.mute = false;
                } else {
                audioSource.Play();
            }
            }
        }
        active = !active;
    }

    private IEnumerator DestroyTrigger() {

        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
}
