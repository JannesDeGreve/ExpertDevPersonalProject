using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add the TextMesh Pro namespace to access the various functions.

public class RestaurantSelectionScreen : MonoBehaviour {

    // public Button button1;
    // public Button button2;
    // public Button button3;
    // public Button button4;
    // public Text textField;

    [System.Serializable]
    public class LatLng {
        public float latitude;
        public float longitude;
    }

    public static LatLng locationInfo;
    public static string currentLocation;

    // [System.Serializable]
    // public class Restaurants {
    //     public List<Restaurant> restaurants;
    // }

    // [System.Serializable]
    // public class Restaurant {
    //      float restaurantLat;
    //      float restaurantLong;
    //      string restaurantName;
    // }

    //public static RestaurantSelectionScreen Instance { get; set; }

    // Restaurants restaurants;
    // Restaurant restaurant1;
    // Restaurant restaurant2;
    // public Restaurant[] myArray;

    void Start () {

        //Instance = this;
        //DontDestroyOnLoad (gameObject);
        Debug.Log ("start log");

        //create an array of your new type
        // Restaurant[] myArray = new Restaurant[2];

        //Debug.Log (myArray[0].restaurantLat);

        //assign Value1 of myClassArray[someIndex] to someValue
        // myArray[0].restaurantLat = 50.8493271f;
        // myArray[0].restaurantLong = 3.27963328f;
        // myArray[0].restaurantName = "huis";

        // myArray[1].restaurantLat = 51.8493271f;
        // myArray[1].restaurantLong = 4.27963328f;
        // myArray[1].restaurantName = "resto 2";

        // restaurants = new Restaurants (); 
        // restaurants.restaurants = new List<Restaurant> ();

        // Restaurant restaurant1 = new Restaurant ();
        // restaurant1.coordinates.latitude = 50.8493271f;
        // restaurant1.coordinates.longitude = 3.27963328f;
        // restaurant1.restaurantName = "huis";

        // restaurants.restaurants.Add (restaurant1);

        // Restaurant restaurant2 = new Restaurant ();
        // restaurant2.coordinates.latitude = 51.8493271f;
        // restaurant2.coordinates.longitude = 4.27963328f;
        // restaurant2.restaurantName = "tweede resto";

        //restaurants.restaurants.Add (restaurant2);

        // button1.onClick.AddListener (handleClickButton);
        // button2.onClick.AddListener (handleClickButton);
        // button3.onClick.AddListener (handleClickButton);
        // button4.onClick.AddListener (handleClickButton);

        StartCoroutine ("LocationHandler");

    }
    // Start is called before the first frame update
    private IEnumerator LocationHandler () {
        Debug.Log ("script started");
        Debug.Log ("enabled:" + Input.location.isEnabledByUser);

        locationInfo = new LatLng ();

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;
        Debug.Log ("Enabled check");

        // Start service before querying location
        Input.location.Start (0.5f, 0.5f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds (1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
            Debug.Log ("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.Log ("Unable to determine device location");
            yield break;
        } else {
            // Access granted and location value could be retrieved
            locationInfo.latitude = Input.location.lastData.latitude;
            locationInfo.longitude = Input.location.lastData.longitude;
            // textField.text = "Latitude: " + locationInfo.latitude.ToString ("R") + " Longitude: " + locationInfo.longitude.ToString ("R");

            if (locationInfo.latitude <= 50.8493271f + 0.001 && locationInfo.latitude >= 50.8493271f - 0.001 && locationInfo.longitude <= 3.27963328f + 0.001 && locationInfo.longitude >= 3.27963328f - 0.001) {
                // textField.text = "op de juiste locatie";
                Debug.Log ("juiste locatie");
                currentLocation = "Thuis";

                SceneManager.LoadScene ("OCROnboarding");

            } else {
                Debug.Log ("niet juiste locatie");
                // textField.text = "niet op de juiste locatie";
            }

            // foreach (var restaurant in myArray) {
            //     Debug.Log ("test loop");

            // }

            // Debug.Log ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop ();

    }

    // void handleClickButton () {
    //     SceneManager.LoadScene ("OCROnboarding");
    // }

    // Update is called once per frame
    void Update () {

    }
}