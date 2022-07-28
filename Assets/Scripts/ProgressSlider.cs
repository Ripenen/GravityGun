using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelBase;
    [SerializeField] private Slider _progressSlider;
    
    private float _maxValue = 1;

    public void SetLevelText(string text) => _levelBase.text = text;
    public void SetMaxValue(float value) => _maxValue = value;
    public void AddValue(float value) => _progressSlider.value += value * (1 / _maxValue);
    public void UpLevelBaseNumber() => _levelBase.text = (int.Parse(_levelBase.text) + 1).ToString();
}
