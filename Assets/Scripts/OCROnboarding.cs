using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OCROnboarding : MonoBehaviour {

    public Text currentRestaurant;
    public Slider slider;

    private float maxTime = 3f;
    private float activeTime = 0f;

    void Start () {
        // Check if there is a restaurant name aka location is enabled.
        if (RestaurantSelectionScreen.currentLocation != null) {
            currentRestaurant.text = RestaurantSelectionScreen.currentLocation;
        }
    }

    public void Update () {
        // progress bar, wait for 3 seconds before going to the next scene.
        if (activeTime < maxTime) {
            activeTime += Time.deltaTime;
            var percent = activeTime / maxTime;
            //Debug.Log (percent);
            slider.value = percent;
        } else {
            slider.value = 1f;
            SceneManager.LoadScene ("OCRMain");

        }

    }

}