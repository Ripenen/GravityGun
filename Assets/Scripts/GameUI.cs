using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _tapText;

    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private ProgressSlider _progressSlider;

    private void Awake()
    {
        _restartButton.onClick.AddListener(RestartLevel);
        _nextLevelButton.onClick.AddListener(NextLevel);
    }

    public IEnumerator TapWait()
    {
        _tapText.SetActive(true);
        while (!Input.GetMouseButton(0))
        {
            yield return new WaitForEndOfFrame();
        }
        _tapText.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("GeneratedLevel");
    }

    public void AddProgress(float value) => _progressSlider.AddValue(value);
    public void UpLevelBaseNumber() => _progressSlider.UpLevelBaseNumber();
    public void SetLevelText(string text) => _progressSlider.SetLevelText(text);
    public void SetMaxProgressValue(float value) => _progressSlider.SetMaxValue(value);
    public void ShowLose() => _loseScreen.SetActive(true);

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    public void ShowWin() => _winScreen.SetActive(true);
}