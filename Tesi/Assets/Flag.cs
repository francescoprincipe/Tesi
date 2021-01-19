using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class Flag : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreTextObject;

    [SerializeField]
    private GameObject opponentScoreTextObject;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI opponentScoreText;

    [SerializeField]
    private GameObject endPanel;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject opponent;

    private CatchTheFlagGuy playerComponent;
    private CatchTheFlagGuy opponentComponent;

    private int playerScore;
    private int opponentScore;

    bool endGame = false;

    private void Awake()
    {
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        opponentScoreText = opponentScoreTextObject.GetComponent<TextMeshProUGUI>();
        playerComponent = player.GetComponent<CatchTheFlagGuy>();
        opponentComponent = opponent.GetComponent<CatchTheFlagGuy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (endGame)
            return;

        if (other.gameObject == player)
        {
            endGame = true;
            endPanel.SetActive(true);
            playerScore += 1;
            playerComponent.EndGame();
            opponentComponent.EndGame();
            scoreText.text = "TU: " + playerScore.ToString();
        }

        if (other.gameObject == opponent)
        {
            endGame = true;
            endPanel.SetActive(true);
            opponentScore += 1;
            playerComponent.EndGame();
            opponentComponent.EndGame();
            opponentScoreText.text = "AVVERSARIO: " + opponentScore.ToString();
        }

    }

    public void ResetGame()
    {
        endGame = false;
    }
}
