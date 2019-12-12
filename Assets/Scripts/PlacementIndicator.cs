using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour {

    AssetBundle myLoadedAssetBundle;
    private string url = "http://jannesdegreve.be/assetbundles";

    private ARRaycastManager rayManager;
    private GameObject visual;
    public Text instructionText;
    public Text dishText;
    bool isCreated = false;

    private string path;
    private string dishName;
    private string dishNameToRender;

    private GameObject biefstuk;
    private GameObject lang;
    private GameObject lasagne;
    private GameObject spaghetti;
    private GameObject lemons;

    public List<GameObject> objectArray;
    public GameObject objectToRender;

    void Start () {

        // static var
        //Debug.Log (ARDishSelectionMenu.selectedDish);
        Debug.Log ("Key: " + ARDishSelectionMenu.selectedDish.Key);

        dishName = ARDishSelectionMenu.selectedDish.Key;
        path = ARDishSelectionMenu.selectedDish.Value;
        dishText.text = dishName;

        Debug.Log ("voor operatie " + path);

        dishNameToRender = path.Substring (path.IndexOf ('/') + 1);
        dishNameToRender = dishNameToRender.Substring (dishNameToRender.IndexOf ('/') + 1);

        Debug.Log ("dish name to render: " + dishNameToRender);

        StartCoroutine ("loadSelectedObjectFromServer");

        // foodPrefab = (GameObject) Resources.Load ("prefabs/NoodleMeshCleaned", typeof (GameObject));
        // cubePrefab = (GameObject) Resources.Load ("prefabs/Cube", typeof (GameObject));
        // biefstuk = (GameObject) Resources.Load ("prefabs/Biefstuk", typeof (GameObject));
        // lang = (GameObject) Resources.Load ("prefabs/Lang", typeof (GameObject));
        // lasagne = (GameObject) Resources.Load ("prefabs/Lasagne", typeof (GameObject));
        // spaghetti = (GameObject) Resources.Load ("prefabs/Spaghetti", typeof (GameObject));
        // lemons = (GameObject) Resources.Load ("prefabs/Lemons", typeof (GameObject));

        // objectArray.Add (biefstuk);
        // objectArray.Add (lang);
        // objectArray.Add (lasagne);
        // objectArray.Add (spaghetti);
        // objectArray.Add (lemons);
        // objectArray.Add (loadedObj);

        // foreach (GameObject food in objectArray) {
        //     food.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
        // }

        //objectToRender = spaghetti;

        // matchedDishes.Add ("Spaghetti Bolognese");
        // matchedDishes.Add ("Biefstuk met frieten");
        // matchedDishes.Add ("Lasagne");
        // matchedDishes.Add ("Een gerecht met een lange naam");

        // switch (ARDishSelectionMenu.selectedDish) {
        //     case "Spaghetti Bolognese":
        //         Debug.Log ("Spaghetti");
        //         objectToRender = spaghetti;
        //         break;
        //     case "Biefstuk met frieten":
        //         Debug.Log ("Biefstuk");
        //         objectToRender = biefstuk;
        //         break;
        //     case "Lasagne":
        //         Debug.Log ("Lasagne");
        //         objectToRender = lasagne;
        //         break;
        //     case "Een gerecht met een lange naam":
        //         Debug.Log ("Lange naam");
        //         objectToRender = lang;
        //         break;
        //     default:
        //         Debug.Log ("Geen gerecht, code klopt niet foei");
        //         break;
        // }

        //foodPrefab = Instantiate(Resources.Load("prefabs/cube", typeof(GameObject))) as GameObject;

        // get the components
        rayManager = FindObjectOfType<ARRaycastManager> ();
        visual = transform.GetChild (0).gameObject;

        // hide the placement indicator visual
        visual.SetActive (false);

    }

    IEnumerator loadSelectedObjectFromServer () {

        string fullUrl = url + path;
        Debug.Log ("full url: " + fullUrl);
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle (fullUrl, 0);
        yield return request.SendWebRequest ();

        if (request.isNetworkError) {
            Debug.Log ("Error: " + request.error);
        } else {
            Debug.Log (request.downloadHandler);
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent (request);
        GameObject loadedObject = bundle.LoadAsset<GameObject> (dishNameToRender);

        Debug.Log (loadedObject == null ? "Failed to load assetBundle" : "AssetBundle succesfully loaded");
        objectToRender = loadedObject;
        objectToRender.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);

    }

    void Update () {
        // shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit> ();
        rayManager.Raycast (new Vector2 (Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit an AR plane surface, update the position and rotation
        if (hits.Count > 0) {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }

        // if (selectionIsMade == true) {

        // enable the visual if it's disabled
        if (isCreated == false) {
            visual.SetActive (true);
            instructionText.text = "Druk op het scherm om jouw gerecht te zien";

        } else {
            visual.SetActive (false);
        }
        // if(!visual.activeInHierarchy)
        //     visual.SetActive(true);

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch (0);
            var hitPose = hits[0].pose;
            instructionText.text = "";

            if (touch.phase == TouchPhase.Ended) {

                if (isCreated == false) {
                    Instantiate (objectToRender, hitPose.position, hitPose.rotation);
                    isCreated = true;
                }
            }
        }
        // }
    }
}