using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }

    }

    [Header("Score and Level")]
    [SerializeField] private GameObject gameUICanvas;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private Image[] playerBallsLeft;
    [SerializeField] private Image[] aiBallsLeft;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private TextMeshProUGUI currentGold;

    [Header("Transition")]
    [SerializeField] private GameObject transitionScreenCanvas;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    [SerializeField] private RawImage coinImage;
    [SerializeField] private Texture coinPile;
    [SerializeField] private Texture coinAlone;
    [SerializeField] private TextMeshProUGUI playerScoreTransition;
    [SerializeField] private TextMeshProUGUI aiScoreTransition;

    [Header("Turns")]
    [SerializeField] private GameObject turnCanvas;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private float timeOnScreen;

    public void InitializeInGameUI()
    {
        UpdateScore(0, 0);
        gameUICanvas.SetActive(true);
        UpdateBallsLeft(3, 3);
    }

    public void HideGameUI()
    {
        gameUICanvas.SetActive(false);
    }

    public void UpdateScore(float pScore, float aScore)
    {
        playerScoreText.text = pScore.ToString();
        aiScoreText.text = aScore.ToString();
    }

    public void InitializeMainMenu(int _currentGold)
    {
        currentGold.text = _currentGold.ToString();
    }

    public void UpdateBallsLeft(int pbLeft, int abLeft)
    {
        for (int i = 0; i < playerBallsLeft.Length; i++)
        {
            playerBallsLeft[i].enabled = i < pbLeft;
            aiBallsLeft[i].enabled = i < abLeft;
        }
    }

    public IEnumerator TurnAnnounce(bool isPlayerTurn)
    {
        
        turnText.text = isPlayerTurn ? "your turn!" : "enemy's turn!";
        turnCanvas.SetActive(true);

        yield return new WaitForSeconds(timeOnScreen);

        turnCanvas.SetActive(false);
        TurnManager.Instance.BeginTurn();
    }

    public void SetMainMenuVisibility(bool state)
    {
        mainMenuCanvas.SetActive(state);
    }

    public void SetTransitionUI(bool victory, int points)
    {
        if (victory)
        {
            victoryText.text = "you won!";
            coinText.text = "+" + points*2;
            coinImage.texture = coinPile;
        }
        else
        {
           victoryText.text = "you lost...";
           coinText.text = "+" + points;
            coinImage.texture = coinAlone;
        }

        playerScoreTransition.text = playerScoreText.text;
        aiScoreTransition.text = aiScoreText.text;

        totalCoinText.text = GameManager.Instance.currentGold.ToString();

        transitionScreenCanvas.SetActive(true);
    }

    public void HideTransitionUI()
    {
        transitionScreenCanvas.SetActive(false);
    }
}
