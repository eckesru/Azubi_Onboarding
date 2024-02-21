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
    [SerializeField] private bool isRoomTrigger = false;
    private bool locked = false;
    private bool roomEntered = false;

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

        if (!locked) {

            if(isRoomTrigger) {
                HandleTriggerJump(collider);
            }

            SwitchAudioSourceState();
        }

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

    private void HandleTriggerJump(Collider collider) {
            // Bewegt den Trigger beim Eintreten des Raums etwas zurueck, damit er nicht versehentlich doppelt ausgeloest werden kann
            // Beim erneuten ausloesen, wird der Trigger an die urspruengliche Position gesetzt
            transform.position += roomEntered ? (transform.forward.normalized / 2) : (-transform.forward / 2);
            roomEntered = !roomEntered;
    }
}
