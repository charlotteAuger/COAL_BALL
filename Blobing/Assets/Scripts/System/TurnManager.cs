using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static TurnManager Instance;

    private bool isPlayerTurn;
    [SerializeField] private int nbrOfTurnPerPlayer;
    private int nbrOfTurnFinished;
    [SerializeField] private AIController ai;
    [SerializeField] private Transform anchorPoint;
    private int pbLeft;
    private int aiLeft;


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }

    }

    public void InitTurnSystem()
    {
        nbrOfTurnFinished = 0;
        pbLeft = aiLeft = nbrOfTurnPerPlayer;
        ChooseFirstPlayer();
        StartCoroutine(UIManager.Instance.TurnAnnounce(isPlayerTurn));
        

    }

    public void ChooseFirstPlayer()
    {
        isPlayerTurn = Random.Range(0, 3) < 2;
    }

    public IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(0.5f);

        Transform newBall = PoolManager.instance.CreateBall(anchorPoint.position, true, 0).transform;
        Dragable drag = newBall.GetComponent<Dragable>();
        drag.enabled = true;
        drag.InitDrag();
    }

    public IEnumerator AITurn()
    {
        yield return new WaitForSeconds(0.5f);
        ai.InitAITurn();
        yield return new WaitForSeconds(0.25f);
        ai.Play();
    }

    public void StartEndTurnCoroutine()
    {
        StartCoroutine(EndTurn());
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(1.5f);

        nbrOfTurnFinished++;

        if (isPlayerTurn)
        {
            pbLeft--;
        }
        else
        {
            aiLeft--;       
        }

        Target.Instance.UpdateScore();

        UIManager.Instance.UpdateBallsLeft(pbLeft, aiLeft);

        if (nbrOfTurnFinished >= nbrOfTurnPerPlayer * 2)
        {
            StartCoroutine(GameManager.Instance.EndGame(Target.Instance.CheckIfPlayerWins()));
        }
        else
        {
            isPlayerTurn = !isPlayerTurn;
            StartCoroutine(UIManager.Instance.TurnAnnounce(isPlayerTurn));
        }

        

    }

    public void BeginTurn()
    {
        if (isPlayerTurn)
        {
            StartCoroutine(PlayerTurn());
        }
        else
        {
            StartCoroutine(AITurn());
        }
    }

}
