using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaybackModeButton : MonoBehaviour, IPointerClickHandler {
    public AudioSource musicSource;
    public GameObject iconShuffle;
    public GameObject iconRepeat;

    private MusicController _musicController;

    void Start () {
        if (musicSource != null) {
            _musicController = musicSource.GetComponent<MusicController>();
        }
        CheckAndSetActiveIcon();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (_musicController != null) {
            _musicController.isShuffling = !_musicController.isShuffling;
        }
        CheckAndSetActiveIcon();
    }

    private void CheckAndSetActiveIcon() {
        bool isShuffling = false;
        if (_musicController != null) {
            isShuffling = _musicController.isShuffling;
        }
        if (iconShuffle != null) {
            iconShuffle.SetActive(isShuffling);
        }
        if (iconRepeat != null) {
            iconRepeat.SetActive(!isShuffling);
        }
    }
}
