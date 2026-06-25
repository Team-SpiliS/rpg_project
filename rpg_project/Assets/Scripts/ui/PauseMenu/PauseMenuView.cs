using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuView : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public event Action OnContinueClicked;
    public event Action OnSaveClicked;
    public event Action OnLoadClicked;
    public event Action OnMainMenuClicked;

    private void Awake()
    {
        _continueButton.onClick.AddListener(() => OnContinueClicked?.Invoke());
        _saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
        _loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        _mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());

        Hide(); 
    }

    public void SetScoreText(string text)
    {
        if (_scoreText != null) _scoreText.text = text;
    }

    public void Show() => _pausePanel.SetActive(true);
    public void Hide() => _pausePanel.SetActive(false);
}