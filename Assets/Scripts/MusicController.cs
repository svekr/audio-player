using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MusicController : MonoBehaviour {
    public AudioSource audioSource;
    public PlaylistData playlist;
    public Slider slider;
    public TextMeshProUGUI currentTrackName;
    public TextMeshProUGUI currentTrackDuration;
    public TextMeshProUGUI currentTrackProgress;

    private bool _isShuffling = false;
    private int _currentTrackIndex = -1;
    private int _previousTrackIndex = -1;
    private float _currentTrackDuration = 0;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;
    private AudioClip _currentTrack;

    public void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Update() {
        if (audioSource != null) {
            float currentTime = audioSource.time;
            float ratio = 0f;
            if (_currentTrackDuration > 0) {
                ratio = currentTime / _currentTrackDuration;
            }
            if (currentTrackProgress != null) {
                currentTrackProgress.text = SecondsToString(currentTime);
            }
            if (slider != null) {
                slider.value = ratio;
            }
        }

    }

    public bool isPlaying {
        get {
            if (audioSource != null) {
                return audioSource.isPlaying;
            }
            return false;
        }
    }

    public bool isShuffling {
        get {
            return _isShuffling;
        }
        set {
            _isShuffling = value;
        }
    }

    public void Play() {
        Debug.Log("Play current track");
        if (_currentTrackIndex < 0) {
            ChangeCurrentTrack(0);
        } else {
            StartPlayTrack();
        }
    }

    public void Pause() {
        Debug.Log("Pause current track");
        ResetCoroutine();
        if (audioSource != null && audioSource.isPlaying) {
            audioSource.Pause();
        }
    }

    public void Stop() {
        Debug.Log("Stop current track");
        ResetCoroutine();
        if (audioSource != null && audioSource.isPlaying) {
            audioSource.Stop();
        }
    }

    public void PlayNext() {
        Debug.Log("Play next track");
        int trackIndex = GetNextTrackIndex();
        ChangeCurrentTrack(trackIndex);
    }

    public void PlayPrevious() {
        Debug.Log("Play previous track");
        int trackIndex = GetPreviousTrackIndex();
        ChangeCurrentTrack(trackIndex);
    }

    private void ChangeCurrentTrack(int trackIndex) {
        if (trackIndex > -1 && trackIndex != _currentTrackIndex) {
            if (playlist != null && playlist.musicDataItems != null) {
                int tracksCount = playlist.musicDataItems.Length;
                if (trackIndex < tracksCount && tracksCount > 0) {
                    MusicItemData musicItem = playlist.musicDataItems[trackIndex];
                    if (musicItem != null && musicItem.track != null) {
                        if (audioSource != null) {
                            audioSource.Stop();
                            _previousTrackIndex = _currentTrackIndex;
                            _currentTrackIndex = trackIndex;
                            _currentTrackDuration = musicItem.track.length;
                            if (currentTrackDuration != null) {
                                currentTrackDuration.text = SecondsToString(_currentTrackDuration);
                            }
                            if (currentTrackName != null) {
                                if (musicItem.trackName != null) {
                                    currentTrackName.text = musicItem.trackName;
                                } else {
                                    currentTrackName.text = "";
                                }
                            }
                            audioSource.clip = musicItem.track;
                            StartPlayTrack();
                        }
                    }
                }
            }
        }
    }

    private void StartPlayTrack() {
        ResetCoroutine();
        if (audioSource != null && audioSource.clip != null) {
            audioSource.Play();
            _waitForSeconds = new WaitForSeconds(audioSource.clip.length - audioSource.time);
            _coroutine = StartCoroutine(WaitForTrackEnds());
        }
    }

    private IEnumerator WaitForTrackEnds() {
        yield return _waitForSeconds;
        PlayNext();
    }

    private void ResetCoroutine() {
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
            //StopCoroutine("WaitForTrackEnds");
            _coroutine = null;
        }
    }

    private int GetNextTrackIndex() {
        int resultIndex = -1;
        if (playlist != null && playlist.musicDataItems != null) {
            int tracksCount = playlist.musicDataItems.Length;
            if (tracksCount == 1) {
                resultIndex = 0;
            } else if (tracksCount > 0) {
                if (isShuffling) {
                    if (_currentTrackIndex > 0 && ((_currentTrackIndex >= (tracksCount - 1)) || (Random.value < 0.5f))) {
                        resultIndex = Random.Range(0, Mathf.Min(_currentTrackIndex, tracksCount - 1));
                    } else {
                        resultIndex = Random.Range(_currentTrackIndex + 1, tracksCount);
                    }
                } else {
                    if ((_currentTrackIndex < 0) || (tracksCount - _currentTrackIndex) < 2) {
                        resultIndex = 0;
                    } else {
                        resultIndex = _currentTrackIndex + 1;
                    }
                }
            }
        }
        return resultIndex;
    }

    private int GetPreviousTrackIndex() {
        int resultIndex = _previousTrackIndex;
        if (!isShuffling) {
            resultIndex = _currentTrackIndex - 1;
        }
        if (resultIndex < 0) {
            if (playlist != null && playlist.musicDataItems != null) {
                int tracksCount = playlist.musicDataItems.Length;
                if (tracksCount == 1) {
                    resultIndex = 0;
                } else if (tracksCount > 0) {
                    resultIndex = tracksCount - 1;
                }
            }
        }
        return resultIndex;
    }

    private string SecondsToString(float time) {
        string resultString;
        int seconds = (int)(time % 60);
        int minutes = (int)(time / 60);
        string sString = seconds.ToString();
        if (sString.Length < 2) {
            sString = "0" + sString;
        }
        string mString = minutes.ToString();
        if (mString.Length < 2) {
            mString = "0" + mString;
        }
        resultString = mString + ":" + sString;
        return resultString;
    }
}
