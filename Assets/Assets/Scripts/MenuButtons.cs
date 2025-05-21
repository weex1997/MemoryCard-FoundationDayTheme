using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    [SerializeField] InputField _playerUsernameInput;

    public void LoadScene(string sceneToload)
    {
        SceneManager.LoadScene(sceneToload);
    }

    public void Restartlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadUrl()
    {
        Application.OpenURL("https://linktr.ee/hamamzahran");
    }

    public void NameSubmit(GameObject namePanel)
    {
        if (_playerUsernameInput.text != "")
            PlayerPrefs.SetString("PlayerName", _playerUsernameInput.text);
        else
            PlayerPrefs.SetString("PlayerName", "unknown");

        Debug.Log("Welcome: " + PlayerPrefs.GetString("PlayerName"));

        LeaderboardManager.Instance.SetPlayerName(PlayerPrefs.GetString("PlayerName"));
        namePanel.SetActive(false);
    }
    public void closeLoading(Canvas Loading)
    {
        Loading.sortingOrder = -2;
    }


}
