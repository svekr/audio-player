using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AdjustVolume : MonoBehaviour {

    public AudioMixer mixer;
    public Slider slider;
    public GameObject iconMusicOn;
    public GameObject iconMusicOff;

    private float _previousVolume = 0f;

    public void Start() {
        float volume = -80f;
        if (slider != null && mixer != null) {
            mixer.GetFloat("MusicVolume", out volume);
            volume = DecibelToLinear(volume);
            slider.value = volume;
            UpdateIcons(Mathf.Approximately(volume, 0f));
            _previousVolume = volume;
        }
    }

    public void OnSliderValueChanged(float newValue) {
        float decibel = LinearToDecibel(newValue);
        bool isVolumeZero = true;
        if (mixer != null) {
            mixer.SetFloat("MusicVolume", decibel);
            isVolumeZero = decibel <= -80f;
        }
        UpdateIcons(isVolumeZero);
    }

    public void Mute() {
        if (slider != null) {
            _previousVolume = slider.value;
            slider.value = 0f;
            CanvasGroup canvasGroup = slider.GetComponent<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.alpha = 0.5f;
            }
        }
    }

    public void Unmute() {
        if (slider != null) {
            if (Mathf.Approximately(_previousVolume, 0f)) {
                _previousVolume = 1f;
            }
            slider.value = _previousVolume;
            CanvasGroup canvasGroup = slider.GetComponent<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.alpha = 1f;
            }
        }
    }

    private float LinearToDecibel(float linear) {
        float decibel = -80f;
        if (!Mathf.Approximately(linear, 0f)) {
            decibel = 20f * Mathf.Log10(linear);
        }
        return decibel;
    }

    private float DecibelToLinear(float decibel) {
        float linear = Mathf.Pow(10f, decibel / 20f);
        return linear;
    }

    private void UpdateIcons(bool isVolumeZero) {
        if (iconMusicOn != null) {
            iconMusicOn.SetActive(!isVolumeZero);
        }
        if (iconMusicOff != null) {
            iconMusicOff.SetActive(isVolumeZero);
        }
    }
}
