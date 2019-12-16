using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARDishSelectionMenu : MonoBehaviour
{

    [Tooltip("The panel used as a parent for the buttons")]
    [SerializeField]
    Transform menuPanel;

    [SerializeField]
    GameObject buttonPrefab;

    public Button restartScanButton;

    public static string selectedDish;
    public static string selectedPath;

    // class to save the dish data from dictionary to ordered list
    [System.Serializable]
    public class testArrayData
    {
        public string testDishName { get; set; }
        public string testDishPath { get; set; }
    }

    public static string[][] dishObjectsArray;

    public static List<testArrayData> newDishObjectArray;
    public static int selectedIndex;

    void Start()
    {

        Debug.Log(OCRMain.matchedDishesWithPath.Count);

        // instantiate a new array
        dishObjectsArray = new string[OCRMain.matchedDishesWithPath.Count][];
        newDishObjectArray = new List<testArrayData>();

        // add a handler for the restart button
        restartScanButton.onClick.AddListener(handleClickRestart);

        int index = 0;

        foreach (KeyValuePair<string, string> keyValue in OCRMain.matchedDishesWithPath)
        {

            newDishObjectArray.Add(new testArrayData()
            {
                testDishName = keyValue.Key,
                testDishPath = keyValue.Value
            });

            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.GetComponentInChildren<Text>().text = newDishObjectArray[index].testDishName;

            button.transform.SetParent(menuPanel.transform, false);

            button.GetComponent<Button>().onClick.AddListener(() => handleClickButton(keyValue));
            index += 1;
        }

        Debug.Log("dish object array count: " + dishObjectsArray.Length);

    }

    void handleClickRestart()
    {
        SceneManager.LoadScene("OCRMain");
    }

    void handleClickButton(KeyValuePair<string, string> keypair)
    {

        int testIndex = 0;
        Debug.Log("naam" + keypair.Key);

        foreach (var item in newDishObjectArray)
        {
            if (item.testDishName == keypair.Key)
            {
                selectedIndex = testIndex;
            }
            testIndex++;
        }

        Debug.Log("de key is: " + selectedIndex + newDishObjectArray[selectedIndex].testDishName);

        // Load the next scene, the selectedIndex is static and shows which dish is selected
        SceneManager.LoadScene("Main");
    }

}