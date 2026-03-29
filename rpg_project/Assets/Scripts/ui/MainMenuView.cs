using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [Header("Ёкраны")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Ёлементы Ќастроек")]
    [SerializeField] private Slider _volumeSlider;

    [Header(" нопки (ƒл€ прив€зки в »нспекторе)")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _closeSettingsButton;

    public event Action OnPlayClicked;
    public event Action<float> OnVolumeSliderChanged;

    private void Awake()
    {
        _playButton.onClick.AddListener(() => OnPlayClicked?.Invoke());

        _settingsButton.onClick.AddListener(ShowSettings);
        _closeSettingsButton.onClick.AddListener(ShowMainMenu);

        _volumeSlider.onValueChanged.AddListener(val => OnVolumeSliderChanged?.Invoke(val));

        ShowMainMenu(); 
    }

    public void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void UpdateVolumeSlider(float volume)
    {
        _volumeSlider.onValueChanged.RemoveAllListeners();
        _volumeSlider.value = volume;
        _volumeSlider.onValueChanged.AddListener(val => OnVolumeSliderChanged?.Invoke(val));
    }
}