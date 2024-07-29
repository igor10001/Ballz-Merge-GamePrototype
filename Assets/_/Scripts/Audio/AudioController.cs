using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO audioClipRefs;
    [SerializeField]private AudioSource audioSource;

    private void Awake()
    {
        // audioSource = gameObject.GetComponent<AudioSource>();
        AudioEvents.OnPlayBallHitSound += HandlePlayBallHitSound;
        AudioEvents.OnPlayBlockDestructionSound += HandlePlayBlockDestructionSound;
    }

    private void OnDestroy()
    {
        AudioEvents.OnPlayBallHitSound -= HandlePlayBallHitSound;
        AudioEvents.OnPlayBlockDestructionSound -= HandlePlayBlockDestructionSound;
    }

    private void HandlePlayBallHitSound()
    {
        PlayRandomClip(audioClipRefs.ballHit);
    }

    private void HandlePlayBlockDestructionSound()
    {
        PlayRandomClip(audioClipRefs.blockDestruction);
    }

    private void PlayRandomClip(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0) return;

        int index = Random.Range(0, clips.Count);
        audioSource.PlayOneShot(clips[index]);
    }
}