using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Button applyButton;
    public GameObject optionsPanel;
    private void Start()
    {
        // Initialize the volume
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        // Add listener for the Apply button
        applyButton.onClick.AddListener(ApplySettingsAndClose);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
    }
    public void ApplySettingsAndClose()
    {
        // Save volume setting
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);

        // Close the options panel
        optionsPanel.SetActive(false);
    }
}

