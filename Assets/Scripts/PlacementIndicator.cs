﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour {

    //ARPlaneManager manager = new ARPlaneManager ();

    private AssetBundle bundle;
    private string url = "http://jannesdegreve.be/assetbundles";
    private bool showPlanes;

    private ARRaycastManager rayManager;
    private GameObject visual;
    public Text instructionText;
    public Text loadingText;
    bool isCreated = false;

    private string path;
    private string dishName;
    private string dishNameToRender;
    public int indexToRenderVariable;

    bool DishIsLoading;

    private GameObject biefstuk;
    private GameObject lang;
    private GameObject lasagne;
    private GameObject spaghetti;
    private GameObject lemons;

    public List<GameObject> objectArray;
    public GameObject objectToRender;
    private GameObject objectInstance;

    private IEnumerator coroutine;

    private void Awake () {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    void Start () {

        if (bundle != null) {
            bundle.Unload (true);
        }

        showPlanes = true;
        indexToRenderVariable = ARDishSelectionMenu.selectedIndex;

        coroutine = loadSelectedObjectFromServer (indexToRenderVariable);
        StartCoroutine (coroutine);

        // get the components
        rayManager = FindObjectOfType<ARRaycastManager> ();
        visual = transform.GetChild (0).gameObject;

        // hide the placement indicator visual
        visual.SetActive (false);

    }

    IEnumerator loadSelectedObjectFromServer (int indexToRender) {
        DishIsLoading = true;
        loadingText.text = "aan het laden...";

        dishName = ARDishSelectionMenu.newDishObjectArray[indexToRender].testDishName;
        // dishText.text = dishName;

        path = ARDishSelectionMenu.newDishObjectArray[indexToRender].testDishPath;

        Debug.Log ("voor operatie " + path);

        dishNameToRender = path.Substring (path.IndexOf ('/') + 1);
        dishNameToRender = dishNameToRender.Substring (dishNameToRender.IndexOf ('/') + 1);

        Debug.Log ("dish name to render: " + dishNameToRender);

        string fullUrl = url + path;
        Debug.Log ("full url: " + fullUrl);
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle (fullUrl, 0);
        yield return request.SendWebRequest ();

        if (request.isNetworkError) {
            Debug.Log ("Error: " + request.error);
            loadingText.text = "er ging iets fout";
        } else {
            Debug.Log (request.downloadHandler);
        }

        bundle = DownloadHandlerAssetBundle.GetContent (request);
        objectToRender = bundle.LoadAsset<GameObject> (dishNameToRender);

        Debug.Log (objectToRender == null ? "Failed to load assetBundle" : "AssetBundle succesfully loaded");
        //objectToRender = loadedObject;
        Debug.Log (objectToRender.name);

        //objectToRender.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
        DishIsLoading = false;
        loadingText.text = "";

    }

    private void SwipeDetector_OnSwipe (SwipeData data) {
        Debug.Log ("Swipe in Direction: " + data.Direction);

        if (data.Direction.ToString () == "Left") {

            if (indexToRenderVariable < ARDishSelectionMenu.newDishObjectArray.Count - 1) {
                indexToRenderVariable++;
                Debug.Log ("Left swipe, doe ++");
            } else {
                indexToRenderVariable = 0;
                Debug.Log ("Left swipe, reset naar 0");
            }

            DishIsLoading = true;
            OnTogglePlanes (true);

            Destroy (objectInstance);
            bundle.Unload (true);

            isCreated = false;
            coroutine = loadSelectedObjectFromServer (indexToRenderVariable);
            StartCoroutine (coroutine);

        }

        if (data.Direction.ToString () == "Right") {
            if (indexToRenderVariable > 0) {
                indexToRenderVariable--;
                Debug.Log ("Right swipe, doe --");
            } else {
                indexToRenderVariable = ARDishSelectionMenu.newDishObjectArray.Count - 1;
                Debug.Log ("Right swipe, reset naar lengte -1");
            }

            DishIsLoading = true;
            OnTogglePlanes (true);

            Destroy (objectInstance);
            bundle.Unload (true);

            isCreated = false;
            coroutine = loadSelectedObjectFromServer (indexToRenderVariable);
            StartCoroutine (coroutine);

        }
    }

    void Update () {
        // shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit> ();
        rayManager.Raycast (new Vector2 (Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit an AR plane surface, update the position and rotation
        if (hits.Count > 0) {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            if (isCreated == false) {
                visual.SetActive (true);
                instructionText.text = "Druk op het scherm om jouw gerecht te zien";

            } else {
                visual.SetActive (false);
            }

            if (DishIsLoading == false) {
                if (Input.touchCount > 0) {
                    Touch touch = Input.GetTouch (0);
                    var hitPose = hits[0].pose;
                    instructionText.text = "Swipe om andere \ngerechten te zien.";

                    if (touch.phase == TouchPhase.Ended) {

                        if (isCreated == false) {
                            objectInstance = (GameObject) Instantiate (objectToRender, hitPose.position, hitPose.rotation);
                            OnTogglePlanes (false);
                            isCreated = true;
                        }
                    }
                }
            }
        }
    }

    public void OnTogglePlanes (bool flag) {
        showPlanes = flag;
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag ("plane")) {
            Renderer r = plane.GetComponent<Renderer> ();
            ARPlaneMeshVisualizer t = plane.GetComponent<ARPlaneMeshVisualizer> ();
            r.enabled = flag;
            t.enabled = flag;
        }
    }
}