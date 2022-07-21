using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Die die;

    bool? winCondition = null;

    [SerializeField]
    Player[] players;
    int currentPlayerIndex = 1;

    Dictionary<int, PositionModifier> snakesAndLadders;
    Player CurrentPlayer => players[currentPlayerIndex];

    [SerializeField]
    Text turnIndicator;

    [SerializeField]
    GameObject restartButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayGame());

        List<PositionModifier> modifiers = new List<PositionModifier>(FindObjectsOfType<PositionModifier>());

        snakesAndLadders = new Dictionary<int, PositionModifier>();
        for (int i = 0; i < modifiers.Count; i++)
        {
            PositionModifier mod = modifiers[i];

            snakesAndLadders.Add(mod.Start, mod);
        }
    }
    bool doubleTurn;

    IEnumerator PlayGame()
    {
        //For testing purposes
        //foreach(Player player in players)
        //{
        //    player.SetPosition(1);
        //}

        while (winCondition == null)
        {
            if (!doubleTurn)
                SwitchPlayer();

            doubleTurn = false;

            if(currentPlayerIndex == 0)
            {
                turnIndicator.text = "Player Turn";
                button.SetActive(true);
                yield return new WaitUntil(() => input);
                button.SetActive(false);
            }
            else
            {
                turnIndicator.text = "AI Turn";
            }

            die.gameObject.SetActive(true);

            die.PlayRollAnim();
            yield return new WaitForSeconds(1f);

            int result = die.Roll();

            if (result == 6 && CurrentPlayer.CurrentPostition != -1)
                doubleTurn = true;

            if (result == 1 && CurrentPlayer.CurrentPostition == -1)
                doubleTurn = true;

            yield return new WaitForSeconds(2f);

            die.gameObject.SetActive(false);

            CurrentPlayer.SetPosition(result);

            yield return new WaitUntil(() => !CurrentPlayer.Moving);

            if (snakesAndLadders.ContainsKey(CurrentPlayer.CurrentPostition))
            {
                PositionModifier mod = snakesAndLadders[CurrentPlayer.CurrentPostition];

                yield return new WaitForSeconds(0.5f);
                CurrentPlayer.ApplyModifier(mod);
                yield return new WaitUntil(() => !CurrentPlayer.Moving);

                if (mod.IsLadder)
                    doubleTurn = true;
            }

            if(CurrentPlayer.CurrentPostition == 99)
            {
                if (currentPlayerIndex == 0)
                {
                    winCondition = true;
                }
                else
                {
                    winCondition = false;
                }
            }
            input = false;
            yield return new WaitForSeconds(1f);
        }

        if(winCondition == true)
        {
            turnIndicator.text = "Player Wins! Restarting in 5 seconds...";
        }
        else
        {
            turnIndicator.text = "AI Wins! Restarting in 5 seconds...";
        }

        yield return new WaitForSeconds(5f);

        Replay();
    }
    [SerializeField]
    GameObject button;

    void Replay()
    {
        SceneManager.LoadScene(0);
    }

    bool input = false;

    public void InputDetected()
    {
        input = true;
    }
    void SwitchPlayer()
    {
        currentPlayerIndex++;

        if(currentPlayerIndex >= players.Length)
        {
            currentPlayerIndex = 0;
        }
    }
}
