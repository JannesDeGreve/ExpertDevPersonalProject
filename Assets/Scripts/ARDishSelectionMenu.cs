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

    //public static string selectedDish;
    public static KeyValuePair<string, string> selectedDish;

    //public List<string> matchedDishes;
    public Vector3 positionVector = new Vector3 (0, 0, 0);

    void Start () {
        // matchedDishes = new List<string> ();

        // matchedDishes.Add ("Spaghetti Bolognese");
        // matchedDishes.Add ("Biefstuk met frieten");
        // matchedDishes.Add ("Lasagne");
        // matchedDishes.Add ("Een gerecht met een lange naam");

        foreach (KeyValuePair<string, string> keyValue in OCRMain.matchedDishesWithPath) {
            Debug.Log (keyValue.Key);
            GameObject button = (GameObject) Instantiate (buttonPrefab);
            Debug.Log (button);
            button.GetComponentInChildren<Text> ().text = keyValue.Key;

            button.transform.SetParent (menuPanel.transform, false);
            button.transform.position += positionVector;

            positionVector[1] -= 250;

            button.GetComponent<Button> ().onClick.AddListener (() => handleClickButton (keyValue));

            //button.GetComponent<Text> ().text = dish;

            // button.GetComponent<Text> ().text = dish;
            // button.transform.parent = menuPanel;
        }

    }

    void handleClickButton (KeyValuePair<string, string> dish) {
        selectedDish = dish;
        Debug.Log (selectedDish);
        SceneManager.LoadScene ("Main");

    }

    // Update is called once per frame
    void Update () {

    }
}