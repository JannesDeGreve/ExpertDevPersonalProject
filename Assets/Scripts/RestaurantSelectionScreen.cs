using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleZip;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestaurantSelectionScreen : MonoBehaviour
{

    [System.Serializable]
    public class LatLng
    {
        public float latitude;
        public float longitude;
    }

    public static string currentLocation;

    // API links, taken from UrlData.cs
    private string newUrl = UrlData.allRestaurantsUrl;
    private string genericDishesUrl = UrlData.genericDishesUrl;
    Dictionary<string, string> headers;

    public static Dictionary<string, string> allDishesFromCurrentRestaurant;
    public static Dictionary<string, string> genericDishes;

    public LatLng locationInfo;

    // Class structure for the JSON
    // Handy tool to create c# classes from JSON: http://json2csharp.com/#

    [System.Serializable]
    public class Latitude
    {
        public double doubleValue;
    }

    [System.Serializable]
    public class RestaurantName
    {
        public string stringValue;
    }

    [System.Serializable]
    public class Path
    {
        public string stringValue;
    }

    [System.Serializable]
    public class Dish
    {
        public string stringValue;
    }

    [System.Serializable]
    public class Fields2
    {
        public Path path;
        public Dish dish;
    }

    [System.Serializable]
    public class MapValue
    {
        public Fields2 fields;
    }

    [System.Serializable]
    public class Value
    {
        public MapValue mapValue;
    }

    [System.Serializable]
    public class ArrayValue
    {
        public List<Value> values;
    }

    [System.Serializable]
    public class Dishes
    {
        public ArrayValue arrayValue;
    }

    [System.Serializable]
    public class Longitude
    {
        public double doubleValue;
    }

    [System.Serializable]
    public class Fields
    {
        public Latitude latitude;
        public RestaurantName restaurantName;
        public Dishes dishes;
        public Longitude longitude;
    }

    [System.Serializable]
    public class Document
    {
        public string name;
        public Fields fields;
        public DateTime createTime;
        public DateTime updateTime;
    }

    [System.Serializable]
    public class RootObject
    {
        public List<Document> documents;
    }

    // declaring objects
    public RootObject responses;
    public Document genericResponses;

    public Text description;
    public GameObject loadingCircle;
    public GameObject disabled;
    public Button restartButton;
    public Button continueButton;

    void Start()
    {

        // Set the headers
        headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=UTF-8");

        // Message when the location is disabled, make it invisible.
        disabled.SetActive(false);

        // add listeners for the buttons		
        restartButton.onClick.AddListener(handleRestartButton);
        continueButton.onClick.AddListener(handleContinueButton);

        // Start locationhandler and fetchrestaurantdata
        StartCoroutine("FetchGenericData");
        StartCoroutine("LocationHandler");
    }

    void handleRestartButton()
    {
        StartCoroutine("LocationHandler");
    }

    void handleContinueButton()
    {
        // Start the next scene without a location
        Debug.Log("continue");
        SceneManager.LoadScene("OCROnboarding");

    }

    private IEnumerator FetchRestaurantData()
    {

        // Function to fetch json data from firestore database

        using (UnityWebRequest webRequest = UnityWebRequest.Get(newUrl))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = newUrl.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                // Throw an error if there is an error
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                // Deserialize the JSON data
                string jsonString = webRequest.downloadHandler.text;
                responses = JsonUtility.FromJson<RootObject>(jsonString);

                CheckLocation();
            }
        }

    }

    private IEnumerator FetchGenericData()
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(genericDishesUrl))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = newUrl.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                // Throw an error if there is an error
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                //genericDishes = new List<Dictionary<string, string>> ();
                genericDishes = new Dictionary<string, string>();

                // Deserialize the JSON data
                string jsonString = webRequest.downloadHandler.text;
                genericResponses = JsonUtility.FromJson<Document>(jsonString);

                foreach (var genericDishAndPath in genericResponses.fields.dishes.arrayValue.values)
                {
                    Debug.Log(genericDishAndPath.mapValue.fields.dish.stringValue);
                    Debug.Log(genericDishAndPath.mapValue.fields.path.stringValue);

                    genericDishes.Add(genericDishAndPath.mapValue.fields.dish.stringValue, genericDishAndPath.mapValue.fields.path.stringValue);
                }

                Debug.Log(genericDishes);
            }
        }

    }

    // Start is called before the first frame update
    private IEnumerator LocationHandler()
    {
        Debug.Log("enabled:" + Input.location.isEnabledByUser);

        // Set the UI elements
        description.text = "We bekijken waar je bent aan de hand van jouw locatie.";
        loadingCircle.SetActive(true);
        disabled.SetActive(false);

        locationInfo = new LatLng();

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("not enabled");
            description.text = "Het ziet ernaar uit dat jouw locatie niet is ingeschakeld.";
            loadingCircle.SetActive(false);
            disabled.SetActive(true);
            yield break;

        }
        Debug.Log("Enabled check");

        // Start service before querying location, location precision 0.5f in meters
        Input.location.Start(0.5f, 0.5f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            locationInfo.latitude = Input.location.lastData.latitude;
            locationInfo.longitude = Input.location.lastData.longitude;
            StartCoroutine("FetchRestaurantData");
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public void CheckLocation()
    {

        allDishesFromCurrentRestaurant = new Dictionary<string, string>();

        Debug.Log("latitude : " + locationInfo.latitude);
        Debug.Log("longitude : " + locationInfo.longitude);

        // Check if there is a location and a response from the webrequest
        if (responses != null && locationInfo.latitude != 0 && locationInfo.longitude != 0)
        {
            // loop through all the restaurants from the webrequest
            foreach (var restaurant in responses.documents)
            {

                double restaurantLatitude = restaurant.fields.latitude.doubleValue;
                double restaurantLongitude = restaurant.fields.longitude.doubleValue;

                Debug.Log(restaurantLatitude);
                Debug.Log(restaurantLongitude);

                // Check if the current location matches one of the restaurants
                if (locationInfo.latitude <= restaurantLatitude + 0.01 && locationInfo.latitude >= restaurantLatitude - 0.01 && locationInfo.longitude <= restaurantLongitude + 0.01 && locationInfo.longitude >= restaurantLongitude - 0.01)
                {
                    Debug.Log("juiste locatie");
                    currentLocation = restaurant.fields.restaurantName.stringValue;

                    // add the dishes from the matched restaurant to the list with all the dishes
                    foreach (var dishAndPath in restaurant.fields.dishes.arrayValue.values)
                    {
                        Debug.Log(dishAndPath.mapValue.fields.dish.stringValue);
                        Debug.Log(dishAndPath.mapValue.fields.path.stringValue);

                        allDishesFromCurrentRestaurant.Add(dishAndPath.mapValue.fields.dish.stringValue, dishAndPath.mapValue.fields.path.stringValue);
                    }

                }
                else
                {
                    Debug.Log("niet juiste locatie");
                }

            }

            // start the next scene
            Debug.Log(currentLocation);
            SceneManager.LoadScene("OCROnboarding");
            return;

        }
        else
        {
            return;
        }

    }
}