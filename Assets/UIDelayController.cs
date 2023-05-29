using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDelayController : MonoBehaviour
{
    private ActionReader _actionReader;
    
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _sliderValueText;
    private float _delay => _actionReader.BotActionDelay;
    private bool _isPaused => _actionReader.Paused;
    
    
    private void Awake()
    {
        _actionReader = ActionReader.Instance;
    }
    
    
    private void Start()
    {
        _slider.value = _delay;
        UpdatePlayPauseButtons();
        _sliderValueText.text = _delay.ToString("F2");
        
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        _actionReader.Paused = false;
        UpdatePlayPauseButtons();
    }

    private void OnPauseButtonClicked()
    {
        Debug.Log("Pause");
        _actionReader.Paused = true;
        UpdatePlayPauseButtons();
    }

    private void UpdatePlayPauseButtons()
    {
        _pauseButton.gameObject.SetActive(!_isPaused);
        _playButton.gameObject.SetActive(_isPaused);
    }

    private void OnSliderValueChanged(float amount)
    {
        Debug.Log("Slider value changed to " + amount);
        _actionReader.BotActionDelay = amount;
        _sliderValueText.text = amount.ToString("F2");
    }
    
    
}
