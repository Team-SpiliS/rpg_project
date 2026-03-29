using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _mainMenuButton;

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

    public void Show() => _pausePanel.SetActive(true);
    public void Hide() => _pausePanel.SetActive(false);
}