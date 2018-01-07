using UnityEngine;

public class SoundController : MonoBehaviour {
    public AudioSource audioSource;

    public void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void playSound() {
        if (audioSource != null) {
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }
}
