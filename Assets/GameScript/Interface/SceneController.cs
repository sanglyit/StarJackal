using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ScenceChange(string name)
    {
        if (name == "ShipSelection")
        {
            ShipSelector.instance.DestroySingleTon();
        }
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the Unity Editor
        #else
        Application.Quit(); // Quits the game when built
        #endif
    }
}
