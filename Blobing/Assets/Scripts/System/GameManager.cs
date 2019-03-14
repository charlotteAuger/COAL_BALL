using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SaveScript saveScript;

    public LevelData currentLevel;
    [SerializeField] private LevelList levelList;
    public int currentGold;

    public delegate void PowerUpEvent(float amount);
    public PowerUpEvent TrailUp;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }

        saveScript = new SaveScript();
        int levelID = saveScript.GetSavedLevel();
        currentGold = saveScript.GetSavedGold();

        Screen.orientation = ScreenOrientation.Portrait;
    }


    public void StartGame()
    {

        UIManager.Instance.SetMainMenuVisibility(false);
        UIManager.Instance.InitializeInGameUI();
        UIManager.Instance.HideTransitionUI();

        TurnManager.Instance.InitTurnSystem();

        PoolManager.instance.Clear();

        LevelGenerator.instance.GenerateLevel();

    }

    public IEnumerator EndGame(bool playerWins)
    {

        yield return new WaitForSeconds(0.5f);

        int score = Target.Instance.GetPlayerScore();

        currentGold += playerWins ? score * 2 : score;

        UIManager.Instance.SetTransitionUI(playerWins, score);

        saveScript.Save(currentGold);
    }

}
