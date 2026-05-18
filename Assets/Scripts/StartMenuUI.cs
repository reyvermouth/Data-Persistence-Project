using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public InputField nameInputField;
    public Text bestScoreText;

    public void Start()
    {
        nameInputField.onValueChanged.AddListener(OnNameInputChanged);
    }

    private void OnNameInputChanged(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        if (SaveSystem.Instance != null)
        {
            int score = SaveSystem.Instance.GetHighScoreByName(text);

            if (score > 0)
            {
                bestScoreText.text = "Best score: " + nameInputField.text + ": " + score;
            }
        }
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(nameInputField.text)) return;

        SaveSystem.Instance.currentPlayerName = nameInputField.text;
        SaveSystem.Instance.currenScore = 0;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity Player
#endif
    }
}
