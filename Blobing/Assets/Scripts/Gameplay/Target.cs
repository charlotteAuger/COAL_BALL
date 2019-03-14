using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public static Target Instance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }

    }

    private List<PointGiver> pointGivers = new List<PointGiver>();
    public List<PointGiver> playerPoints = new List<PointGiver>();
    public List<PointGiver> aiPoints = new List<PointGiver>();
    private int playerScore = 0;
    private int aiScore = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            PointGiver pG = other.gameObject.GetComponent<PointGiver>();

            if (pG.isOwnedByPlayer && !playerPoints.Contains(pG))
            {
                playerPoints.Add(pG);
            }
            else if (!pG.isOwnedByPlayer && !aiPoints.Contains(pG))
            {
                aiPoints.Add(pG);

            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            PointGiver pG = other.gameObject.GetComponent<PointGiver>();

            if (pG.isOwnedByPlayer)
            {
                playerPoints.Remove(pG);
            }
            else
            {
                aiPoints.Remove(pG);
            }
        }
    }

    public void Remove(PointGiver pG)
    {
        if (pG.isOwnedByPlayer && playerPoints.Contains(pG))
        {
            playerPoints.Remove(pG);
        }
        else if (!pG.isOwnedByPlayer && aiPoints.Contains(pG))
        {
            aiPoints.Remove(pG);
        }
    }

    public void UpdateScore()
    {
        int pScore = 0;
        int aScore = 0;

        foreach (PointGiver p in playerPoints)
        {
            pScore += p.GetPointValue();
        }

        foreach (PointGiver p in aiPoints)
        {
            aScore += p.GetPointValue();
        }

        playerScore = pScore;
        aiScore = aScore;

        UIManager.Instance.UpdateScore(playerScore, aiScore);

    }

    public bool PlayerHasMoreBalls()
    {
        return playerPoints.Count > aiPoints.Count;
    }

    public int DifferenceBetweenBiggestBalls()
    {
        int diff = 0;

        int biggestPlayerBall = 1;
        foreach (PointGiver pG in playerPoints)
        {
            int a = pG.stats.growthID;
            if (pG.stats.growthID > biggestPlayerBall)
            {
                biggestPlayerBall = a;
            }
        }

        int biggestAIBall = 1;
        foreach (PointGiver pG in aiPoints)
        {
            int a = pG.stats.growthID;
            if (pG.stats.growthID > biggestAIBall)
            {
                biggestAIBall = a;
            }
        }

        diff = biggestAIBall - biggestPlayerBall;

        return diff;
    }

    public bool CheckIfPlayerWins()
    { 
        return playerScore > aiScore;
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

}
