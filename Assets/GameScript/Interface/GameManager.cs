using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //State of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    //Store the current game state
    public GameState currentState;
    //Store the previous game state
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;

    void Awake()
    {
        DisableScreens();    
    }
    private void Update()
    {
        
        switch (currentState)
        { 
            case GameState.Gameplay:
                CheckForPauseAndResume();
                break;    

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:

                break;

            default:
                Debug.LogWarning("State Does Not Exist");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused) 
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is Resumed");
        }
    }
    //Define method to check pause and resume input
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }   
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
    }
}
