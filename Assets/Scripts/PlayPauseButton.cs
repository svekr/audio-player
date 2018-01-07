using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayPauseButton : MonoBehaviour, IPointerClickHandler {
    public AudioSource musicSource;
    public GameObject iconPlay;
    public GameObject iconPause;

    private MusicController _musicController;

    public IEnumerator Start() {
        if (musicSource != null) {
            _musicController = musicSource.GetComponent<MusicController>();
        }
        yield return null;
        CheckAndSetActiveIcon();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (_musicController != null) {
            if (_musicController.isPlaying) {
                _musicController.Pause();
            } else {
                _musicController.Play();
            }
        }
        CheckAndSetActiveIcon();
    }

    private void CheckAndSetActiveIcon() {
        bool isPlaying = false;
        if (_musicController != null) {
            isPlaying = _musicController.isPlaying;
        }
        if (iconPlay != null) {
            iconPlay.SetActive(!isPlaying);
        }
        if (iconPause != null) {
            iconPause.SetActive(isPlaying);
        }
    }
}
