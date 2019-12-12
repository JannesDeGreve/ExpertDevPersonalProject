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

    // public static KeyValuePair<string, string> selectedDish;
    public static string selectedDish;
    public static string selectedPath;

    // public class dishesList {
    //     string[] dishObject;
    // }

    // public class DishObject {
    //     string dishName;
    //     string dishPath;
    // }
    [System.Serializable]
    public class testArrayData {
        public string testDishName { get; set; }
        public string testDishPath { get; set; }
    }

    public static string[][] dishObjectsArray;

    public static List<testArrayData> newDishObjectArray;
    public static int selectedIndex;
    //public string[] dishObject;

    void Start () {

        Debug.Log (OCRMain.matchedDishesWithPath.Count);
        Debug.Log ("ik ben tot lijn 36 geraakt");

        dishObjectsArray = new string[OCRMain.matchedDishesWithPath.Count][];
        Debug.Log ("ik ben tot lijn 39 geraakt");

        newDishObjectArray = new List<testArrayData> ();

        restartScanButton.onClick.AddListener (handleClickRestart);
        Debug.Log ("ik ben tot lijn 42 geraakt");

        int index = 0;

        foreach (KeyValuePair<string, string> keyValue in OCRMain.matchedDishesWithPath) {
            Debug.Log ("ik ben tot in de foreach geraakt");

            newDishObjectArray.Add (new testArrayData () {
                testDishName = keyValue.Key,
                    testDishPath = keyValue.Value
            });

            GameObject button = (GameObject) Instantiate (buttonPrefab);
            Debug.Log (button);
            // button.GetComponentInChildren<Text> ().text = keyValue.Key;
            button.GetComponentInChildren<Text> ().text = newDishObjectArray[index].testDishName;

            button.transform.SetParent (menuPanel.transform, false);

            button.GetComponent<Button> ().onClick.AddListener (() => handleClickButton (keyValue));
            index += 1;
        }

        Debug.Log ("dish object array count: " + dishObjectsArray.Length);

    }

    void handleClickRestart () {
        SceneManager.LoadScene ("OCRMain");
    }

    void handleClickButton (KeyValuePair<string, string> keypair) {

        int testIndex = 0;

        Debug.Log ("naam" + keypair.Key);

        foreach (var item in newDishObjectArray) {
            if (item.testDishName == keypair.Key) {
                selectedIndex = testIndex;
            }
            testIndex++;
        }

        Debug.Log ("de key is: " + selectedIndex + newDishObjectArray[selectedIndex].testDishName);
        SceneManager.LoadScene ("Main");
    }

}