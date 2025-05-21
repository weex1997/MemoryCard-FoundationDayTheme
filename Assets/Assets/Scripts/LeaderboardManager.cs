using System.Collections;
using UnityEngine;
using TMPro;
using LootLocker.Requests;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] GameObject _leaderboardEntryPrefab;
    [SerializeField] GameObject[] _leaderboardEntryParent;
    [SerializeField] GameObject namePanel;
    [SerializeField] Canvas loadingPanel;
    [SerializeField] TMP_Text PlayerName;

    string LeaderboardID;
    string PlayerID;

    // Singleton instance.
    public static LeaderboardManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("PlayerName"))
            PlayerName.text = "åáÇ æÇááå " + PlayerPrefs.GetString("PlayerName");

    }

    private void Start()
    {
        loadingPanel.sortingOrder = 2;

        LootLockerSDKManager.StartGuestSession((response) =>
       {

           if (response.success)
           {
               print("response successful");
               PlayerID = response.player_identifier;
               GetPlayerName();
           }
           else
           {
               print(response.errorData);
           }
       });

    }

    private void GetPlayerName()
    {
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + response.name);
                if (response.name == "" || response.name == "unknown")
                {
                    namePanel.SetActive(true);
                    PlayerPrefs.SetString("PlayerName", response.name);
                }
                else
                {
                    namePanel.SetActive(false);
                    loadingPanel.sortingOrder = -2;
                    PlayerPrefs.SetString("PlayerName", response.name);
                    PlayerName.text = "åáÇ æÇááå " + response.name;
                }
            }
            else
            {
                Debug.Log("Error getting player name");
                //namePanel.SetActive(true);
            }
        });
    }

    public void SetPlayerName(string name)
    {

        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully set player name");
                PlayerName.text = "åáÇ æÇááå " + response.name;
            }
            else
            {
                Debug.Log("Could not set player name" + response.errorData);
            }
        });
    }

    public void SaveToLeaderboard(int score)
    {
        SceneLeaderbaoardID();

        LootLockerSDKManager.SubmitScore(PlayerID, score, LeaderboardID, (response) =>
        {

            if (response.success)
            {
                print("scores submitted");
                showLeaderboard();
            }

            else
            {
                print(response.errorData);
            }

        });
    }

    private void SceneLeaderbaoardID()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Gamifire Easy Phase1")
            LeaderboardID = "20437";
        if (scene.name == "Gamifire Easy Phase2")
            LeaderboardID = "20444";
    }

    public void showLeaderboard()
    {
        StartCoroutine(Wating());
    }

    public void Leaderboard()
    {
        SceneLeaderbaoardID();

        LootLockerSDKManager.GetScoreList(LeaderboardID, 100,0, (response) =>
        {

            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                _leaderboardEntryParent = GameObject.FindGameObjectsWithTag("Leaderboard");

                string PlayerNames = "Names\n";

                for (int i = 0; i < scores.Length; i++)
                {

                    if (scores[i].player.name != "")
                    {
                        PlayerNames += scores[i].player.name;
                    }
                    else
                    {
                        PlayerNames += scores[i].player.name;
                    }

                    foreach (GameObject parent in _leaderboardEntryParent)
                    {
                        GameObject print = Instantiate(_leaderboardEntryPrefab, parent.transform);
                        print.transform.GetChild(0).GetComponent<TMP_Text>().text =  scores[i].score.ToString();
                        print.transform.GetChild(1).GetComponent<TMP_Text>().text = scores[i].player.name.ToString();
                        print.transform.GetChild(2).GetComponent<TMP_Text>().text = scores[i].rank.ToString();
                    }

                }

            }
            else
            {
                print(response.errorData);
            }

        });
    }

    private IEnumerator Wating()
    {
        yield return new WaitForSeconds(1f);
        Leaderboard();

    }

}
