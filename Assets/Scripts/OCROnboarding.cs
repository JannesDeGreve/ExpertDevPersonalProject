using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OCROnboarding : MonoBehaviour {

    public Button backButton;
    public Button continueButton;

    // Start is called before the first frame update
    void Start () {
        backButton.onClick.AddListener (handleClickBackButton);
        continueButton.onClick.AddListener (handleClickContinueButton);

    }

    void handleClickBackButton () {
        SceneManager.LoadScene ("Startscreen");
    }

    void handleClickContinueButton () {
        SceneManager.LoadScene ("OCRMain");
    }

    // Update is called once per frame
    void Update () {

    }
}