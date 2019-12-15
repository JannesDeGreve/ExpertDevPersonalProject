using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour {

    public Button startButton;
    // Start is called before the first frame update
    void Start () {
        startButton.onClick.AddListener (handleStartButton);
    }

    void handleStartButton () {
        Debug.Log ("continue");
        SceneManager.LoadScene ("Startscreen");

    }
}