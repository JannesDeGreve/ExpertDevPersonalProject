using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class backToHome : MonoBehaviour {
    public Button homeButton;
    public Button scanButton;

    // Start is called before the first frame update
    void Start () {
        homeButton.onClick.AddListener (handleHomeButton);
        scanButton.onClick.AddListener (handleScanButton);

    }

    void handleHomeButton () {
        SceneManager.LoadScene ("WelcomeScreen");
    }

    void handleScanButton () {
        SceneManager.LoadScene ("OCRMain");
    }
}