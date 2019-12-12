using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARDishSelectionMenu : MonoBehaviour {
    // Start is called before the first frame update
    //public GameObject panel;

    [SerializeField]
    Transform menuPanel;

    [SerializeField]
    GameObject buttonPrefab;

    public Button restartScanButton;

    public static KeyValuePair<string, string> selectedDish;

    void Start () {

        restartScanButton.onClick.AddListener (handleClickRestart);

        foreach (KeyValuePair<string, string> keyValue in OCRMain.matchedDishesWithPath) {
            Debug.Log (keyValue.Key);
            GameObject button = (GameObject) Instantiate (buttonPrefab);
            Debug.Log (button);
            button.GetComponentInChildren<Text> ().text = keyValue.Key;

            button.transform.SetParent (menuPanel.transform, false);

            button.GetComponent<Button> ().onClick.AddListener (() => handleClickButton (keyValue));
        }

    }

    void handleClickRestart () {
        SceneManager.LoadScene ("OCRMain");
    }

    void handleClickButton (KeyValuePair<string, string> dish) {
        selectedDish = dish;
        Debug.Log (selectedDish);
        SceneManager.LoadScene ("Main");
    }

}