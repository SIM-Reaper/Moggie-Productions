using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public bool isGameOver = false;
    
    public float startingTime;
    public float timeLeft = 60f;
    
    public UIController uiController;
    public GameObject winScreen;
    public GameObject loseScreen;
    
    public GameObject player;
    public Vector3 playerStartingPosition;

    private PickupItem[] _pickups;
    private TimeBoostItem[] _timeItems;

    private void Start()
    {
        isGameOver = false;
        startingTime = timeLeft;
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    void Update()
    {
        //update timer
        uiController.UpdateTimer(timeLeft);
        //Update score
        uiController.UpdateScore(score);

        if (score >=50)
        {
            WinGame();
        }
        
        if (!isGameOver)//
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }
    
    public void ResetProgress()
    {
        // Reset player's score to 0
        score = 0;
        uiController.UpdateScore(score);

        // Reset the timer to the starting time
        timeLeft = startingTime;
        uiController.UpdateTimer(timeLeft);

        // Reset player position to starting position.
        player.transform.position = playerStartingPosition;
        
        //find all pickup objects and adding to array
        _pickups = PickupItem.GetAllInactivePickupItems();
        _timeItems = TimeBoostItem.GetAllInactiveTimeItems();
        //iterate through all pickups and time boost items and set state to active
        foreach (PickupItem pickup in _pickups)
        {
            //Debug.Log((pickup.name));
            pickup.gameObject.SetActive(true);
        }

        foreach (TimeBoostItem timeItem in _timeItems)
        {
            timeItem.gameObject.SetActive(true);
        }

        if (!isGameOver)
        {
            startingTime = timeLeft;
            winScreen.SetActive(false);
            loseScreen.SetActive(false);
        }
    }

    void EndGame()
    {
        isGameOver = true;
        if (isGameOver)
        {
            loseScreen.SetActive(true);
            StartCoroutine(ResetGame());
        }
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(2f);
        isGameOver = false;
        ResetProgress();
    }

    void WinGame()
    {
        isGameOver = true;
        if (isGameOver)
        {
            winScreen.SetActive(true);
        }
    }
}