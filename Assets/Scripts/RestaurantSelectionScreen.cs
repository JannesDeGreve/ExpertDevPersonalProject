using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestaurantSelectionScreen : MonoBehaviour {

    [System.Serializable]
    public class LatLng {
        public float latitude;
        public float longitude;
    }

    public static string currentLocation;

    //private string url = "https://storage.googleapis.com/ocrfoodappbucket/restaurants/restaurants.json";
    private string newUrl = "https://firestore.googleapis.com/v1/projects/arfoodappocr/databases/(default)/documents/restaurants";
    Dictionary<string, string> headers;

    public LatLng locationInfo;

    [System.Serializable]
    public class Latitude {
        public double doubleValue;
    }

    [System.Serializable]
    public class RestaurantName {
        public string stringValue;
    }

    [System.Serializable]
    public class Path {
        public string stringValue;
    }

    [System.Serializable]
    public class Dish {
        public string stringValue;
    }

    [System.Serializable]
    public class Fields2 {
        public Path path;
        public Dish dish;
    }

    [System.Serializable]
    public class MapValue {
        public Fields2 fields;
    }

    [System.Serializable]
    public class Value {
        public MapValue mapValue;
    }

    [System.Serializable]
    public class ArrayValue {
        public List<Value> values;
    }

    [System.Serializable]
    public class Dishes {
        public ArrayValue arrayValue;
    }

    [System.Serializable]
    public class Longitude {
        public double doubleValue;
    }

    [System.Serializable]
    public class Fields {
        public Latitude latitude;
        public RestaurantName restaurantName;
        public Dishes dishes;
        public Longitude longitude;
    }

    [System.Serializable]
    public class Document {
        public string name;
        public Fields fields;
        public DateTime createTime;
        public DateTime updateTime;
    }

    [System.Serializable]
    public class RootObject {
        public List<Document> documents;
    }

    public RootObject responses;

    void Start () {

        // Set the headers
        headers = new Dictionary<string, string> ();
        headers.Add ("Content-Type", "application/json; charset=UTF-8");

        // Start locationhandler and fetchrestaurantdata
        StartCoroutine ("LocationHandler");
        StartCoroutine ("FetchRestaurantData");
    }

    private IEnumerator FetchRestaurantData () {

        // Function to fetch json data from firestore database

        using (UnityWebRequest webRequest = UnityWebRequest.Get (newUrl)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest ();

            string[] pages = newUrl.Split ('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError) {
                // Throw an error if there is an error
                Debug.Log (pages[page] + ": Error: " + webRequest.error);
            } else {

                Debug.Log (webRequest.downloadHandler.text);

                // Deserialize the JSON data
                string jsonString = webRequest.downloadHandler.text;
                responses = JsonUtility.FromJson<RootObject> (jsonString);

                CheckLocation ();
            }
        }

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
            CheckLocation ();
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop ();
    }

    public void CheckLocation () {

        //Debug.Log (responses.documents[0].name);

        if (responses != null && locationInfo.latitude != null && locationInfo.longitude != null) {

            foreach (var restaurant in responses.documents) {

                double restaurantLatitude = restaurant.fields.latitude.doubleValue;
                double restaurantLongitude = restaurant.fields.longitude.doubleValue;

                Debug.Log (restaurantLatitude);
                Debug.Log (restaurantLongitude);

                if (locationInfo.latitude <= restaurantLatitude + 0.0005 && locationInfo.latitude >= restaurantLatitude - 0.0005 && locationInfo.longitude <= restaurantLongitude + 0.0005 && locationInfo.longitude >= restaurantLongitude - 0.0005) {
                    Debug.Log ("juiste locatie");
                    currentLocation = restaurant.fields.restaurantName.stringValue;

                    // load the next scene
                    SceneManager.LoadScene ("OCROnboarding");
                    return;

                } else {
                    Debug.Log ("niet juiste locatie");
                }

            }

            Debug.Log (currentLocation);

        } else {
            return;
        }

    }

    // Update is called once per frame
    void Update () {

    }
}