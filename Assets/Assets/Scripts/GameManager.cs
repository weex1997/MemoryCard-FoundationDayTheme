using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    bool picked; // Set this true if we have 2 cards
    bool gameOver;
    List<MonoBehaviour> pickedCards = new List<MonoBehaviour>(); // Using MonoBehaviour to handle both Card and Card_2
    int pairs;
    int pairCounter;
    public bool hideMatches;
    public int scorePerMatch = 100;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject winEffect;

    public TMP_Text playerName;
    public TMP_Text playerFinalScore;
    public TMP_Text playerHighScore;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        winEffect.SetActive(false);
    }

    public void AddCardToPickedlist(MonoBehaviour card)
    {
        if (pickedCards.Contains(card))
        {
            return;
        }
        pickedCards.Add(card);
        if (pickedCards.Count == 2)
        {
            picked = true;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1.3f);

        int card1Id = 0;
        int card2Id = 0;

        if (gameObject.name == "GameManager_Phase1")
        {
            card1Id = ((Card)pickedCards[0]).GetCardId();
            card2Id = ((Card)pickedCards[1]).GetCardId();
        }
        else if (gameObject.name == "GameManager_Phase2")
        {
            card1Id = ((Card_2)pickedCards[0]).GetCardId();
            card2Id = ((Card_2)pickedCards[1]).GetCardId();
        }

        if (card1Id == card2Id)
        {
            if (hideMatches)
            {
                pickedCards[0].gameObject.SetActive(false);
                pickedCards[1].gameObject.SetActive(false);
            }
            else
            {
                pickedCards[0].GetComponent<BoxCollider>().enabled = false;
                pickedCards[1].GetComponent<BoxCollider>().enabled = false;
            }
            pairCounter++;

            // VFX activation
            ActivateConfetti(pickedCards[0]);
            ActivateConfetti(pickedCards[1]);

            ScoreManager.instance.AddScore(scorePerMatch);

            CheckForWin();
        }
        else
        {
            FlipOpenCard(pickedCards[0], false);
            FlipOpenCard(pickedCards[1], false);

            yield return new WaitForSeconds(0.2f);
        }

        picked = false;
        pickedCards.Clear();
        ScoreManager.instance.AddTurn();
    }

    void FlipOpenCard(MonoBehaviour card, bool open)
    {
        if (card is Card)
        {
            ((Card)card).FlipOpen(open);
        }
        else if (card is Card_2)
        {
            ((Card_2)card).FlipOpen(open);
        }
    }

    void ActivateConfetti(MonoBehaviour card)
    {
        if (card is Card)
        {
            ((Card)card).ActivateConfetti();
        }
        else if (card is Card_2)
        {
            ((Card_2)card).ActivateConfetti();
        }
    }

    void CheckForWin()
    {
        if (pairs == pairCounter)
        {
            winPanel.SetActive(true);
            winEffect.SetActive(true);

            ScoreManager.instance.StopTimer();
            int wholeScore = (ScoreManager.instance.score * ScoreManager.instance.tempTime) / ScoreManager.instance.turns;

            //playerScore and high score
            int prevusScore = 0;
            int highScore = 0;

            if (gameObject.name == "GameManager_Phase1")
            {
                if (PlayerPrefs.HasKey("PlayerScoreEasy"))
                {
                    prevusScore = PlayerPrefs.GetInt("PlayerScoreEasy");
                }
                else
                {
                    PlayerPrefs.SetInt("PlayerScoreEasy", 0);
                }
            }

            if (gameObject.name == "GameManager_Phase2")
            {
                if (PlayerPrefs.HasKey("PlayerScoreHard"))
                {
                    prevusScore = PlayerPrefs.GetInt("PlayerScoreHard");
                }
                else
                {
                    PlayerPrefs.SetInt("PlayerScoreHard", 0);
                }
            }

            Debug.Log("prevuss" + prevusScore);

            if (prevusScore < wholeScore)
            {
                highScore = wholeScore;
                Debug.Log("highScore: "+highScore);
            }
            else
            {
                highScore = prevusScore;
                Debug.Log("highScore: " + highScore);
            }

            if (gameObject.name == "GameManager_Phase1")
                PlayerPrefs.SetInt("PlayerScoreEasy", highScore);
            if (gameObject.name == "GameManager_Phase2")
                PlayerPrefs.SetInt("PlayerScoreHard", highScore);

            playerName.text = PlayerPrefs.GetString("PlayerName");
            playerFinalScore.text = wholeScore.ToString();
            playerHighScore.text = highScore.ToString();
            LeaderboardManager.Instance.SaveToLeaderboard(highScore);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        losePanel.SetActive(true);

        LeaderboardManager.Instance.showLeaderboard();
    }

    public bool HasPicked()
    {
        return picked;
    }

    public bool GameisOver()
    {
        return gameOver;
    }

    public void SetPairs(int pairAmount)
    {
        pairs = pairAmount;
    }
}
