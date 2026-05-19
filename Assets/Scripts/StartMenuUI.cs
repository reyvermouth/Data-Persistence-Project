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
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(text)) return;

        GameData data = SaveSystem.LoadGame();

        PlayerRecord record = data.playerRecords.Find(p => p.playerName.Equals(playerName, System.StringComparison.OrdinalIgnoreCase));

        int savedScore = (record != null) ? record.highScore : 0;
        bestScoreText.text = "Best Score: " + nameInputField.text + ": " + savedScore;
    }

    public void StartGame()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Guest";
        }

        PlayerPrefs.SetString("CurrentPlayerName", playerName);
        PlayerPrefs.Save();

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
