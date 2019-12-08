using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OCROnboarding : MonoBehaviour {

    // public Button backButton;
    // public Button continueButton;
    public Text currentRestaurant;

    // Start is called before the first frame update
    // void Start () {
    //     backButton.onClick.AddListener (handleClickBackButton);
    //     continueButton.onClick.AddListener (handleClickContinueButton);

    // }

    void Start () {
        //Start the coroutine we define below named ExampleCoroutine.
        currentRestaurant.text = RestaurantSelectionScreen.currentLocation;
        StartCoroutine (WaitBeforeNextScene ());
    }

    IEnumerator WaitBeforeNextScene () {
        //Print the time of when the function is first called.
        Debug.Log ("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds (3);

        //After we have waited 5 seconds print the time again.
        Debug.Log ("Finished Coroutine at timestamp : " + Time.time);
        SceneManager.LoadScene ("OCRMain");

    }

    // void handleClickBackButton () {
    //     SceneManager.LoadScene ("Startscreen");
    // }

    // void handleClickContinueButton () {
    //     SceneManager.LoadScene ("OCRMain");
    // }

    // Update is called once per frame
    void Update () {

    }
}