using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerStat playerStat;
    //State of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    //Store the current game state
    public GameState currentState;
    //Store the previous game state
    public GameState previousState;

    [Header("Extraction Settings")]
    public GameObject extractionExit; // Extraction exit object in the scene
    private bool extractionUnlocked = false;

    [Header("Objective Display")]
    public string enemyName;
    public TextMeshProUGUI objectiveTextBrief;
    public GameObject briefObjectiveText;
    public GameObject objectiveCompletionPanel; 
    private bool objectiveMessageShown = false;
    private int objectiveEnemiesKilled = 0;
    [SerializeField] private int objectiveKillTarget;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 34;
    public TMPro.TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Displays")] 
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentHealDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentFireRateDisplay;
    public TextMeshProUGUI currentStrengthDisplay;

    [Header("Result Screen Displays")]
    public TextMeshProUGUI ResultState;
    public Image chosenCharacterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI timeSurvivedDisplay;
    public TextMeshProUGUI levelReachedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stop Watch")]
    float stopwatchTime; //current time elapsed
    public TextMeshProUGUI stopwatchDisplay;
    //Flag to check if game is over 
    public bool isGameOver = false;
    //Flag to check if player is choosing upgrade
    public bool choosingUpgrade;
    //Reference to the player game object
    public GameObject playerObject;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DisableScreens();    
    }
    private void Start()
    {
        UpdateObjectiveText();
        briefObjectiveText.SetActive(true);
    }
    private void Update()
    {
        
        switch (currentState)
        {
            case GameState.Gameplay:
                UpdateStopWatch();
                CheckForPauseAndResume();
                break;    

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; //Stop the game
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if(!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; //pause the game for upgrading
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("State Does Not Exist");
                break;
        }
    }
    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        // Start generating the floating text.
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if (textFont) tmPro.font = textFont;
        rect.position = referenceCamera.WorldToScreenPoint(target.position);

        // Makes sure this is destroyed after the duration finishes.
        Destroy(textObj, duration);

        // Parent the generated text object to the canvas.
        textObj.transform.SetParent(instance.damageTextCanvas.transform);

        // Pan the text upwards and fade it away over time.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while (t < duration)
        {
            // Wait 1 frame and update time
            yield return w;
            t += Time.deltaTime;

            // Check if the text object still exists before updating its properties
            if (tmPro != null && rect != null)
            {
                // Fade the text to the right alpha value
                tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);

                // Pan the text upward
                yOffset += speed * Time.deltaTime;
                if (target != null)
                {
                    rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
                }
            }
        }
    }
    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        // If the canvas is not set, end the function so we don't
        // generate any floating text.
        if (!instance.damageTextCanvas) return;

        // Find a relevant camera that we can use to convert the world
        // position to a screen position.
        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
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
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        extractionExit.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        briefObjectiveText.SetActive(false);
        objectiveCompletionPanel.SetActive(false);
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
        objectiveMessageShown = false;
    }
    public void AssignChosenCharacterUI(PlayerScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon; 
        chosenCharacterName.text = chosenCharacterData.name;
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignChosenWeaponsAndPassiveItemUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            return;
        }
        // Assign chosen weapons data to chosenWeaponsUI
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            //Check that the sprite of the corresponding element in chosenWeaponData is not null
            if (chosenWeaponsData[i].sprite)
            {
                // Enable the corresponding element in chosenWeapon UI and set sprite correspond to chosen weapon data
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else {
                // if sprite is null, disable corresponding element in chosenweaponUI
                chosenWeaponsUI[i].enabled =false;
            }
        }

        // Assign chosen PassiveItems data to chosenPassiveItemsUI
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            //Check that the sprite of the corresponding element in PassiveItemsData is not null
            if (chosenPassiveItemsData[i].sprite)
            {
                // Enable the corresponding element in PassiveItems UI and set sprite correspond to chosen passive data
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                // if sprite is null, disable corresponding element in chosenPassiveItemsUI
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }
    void UpdateStopWatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopWatchDisplay();
    }

    void UpdateStopWatchDisplay()
    {
        // Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        //Update the stopwatch text to display the elapsed time 
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        if (playerObject != null)
        {
            playerObject.SendMessage("RemoveAndApplyUpgrade");
        }
    }
    public void StopLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; //continue da game
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }

    //Display enemy kill objective in pause menu
    public void OnObjectiveEnemyKilled()
    {
        objectiveEnemiesKilled++;

        if (objectiveEnemiesKilled >= objectiveKillTarget)
        {
            UnlockExtraction();
        }
    }
    private void UpdateObjectiveText()
    {
        if (!extractionUnlocked)
        {
           objectiveTextBrief.text = $"Defeat {enemyName}!";
        }
     }
    private void ShowCompletionMessage()
    {
        if (!objectiveMessageShown) // Check if the message was already shown
        {
            objectiveCompletionPanel.SetActive(true);
            objectiveMessageShown = true; // Mark as shown
            
        }
    }
    private void UnlockExtraction()
    {
        if (!extractionUnlocked) // Check if extraction isn't already unlocked
        {
            extractionUnlocked = true;
            extractionExit.SetActive(true);
            ShowCompletionMessage();
            briefObjectiveText.SetActive(false);
        }
    }
    public void CompleteExtraction()
    {

        ResultState.text = "Objective Completed!";
        ResultState.color = Color.green;
        briefObjectiveText.SetActive(false);
        objectiveCompletionPanel.SetActive(false);
        playerStat.kill();
    }
}
