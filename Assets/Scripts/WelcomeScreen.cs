using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour
{

    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(handleStartButton);
    }

    void handleStartButton()
    {
        Debug.Log("continue");
        SceneManager.LoadScene("Startscreen");

    }
}