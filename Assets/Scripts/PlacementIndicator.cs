using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour {
    private ARRaycastManager rayManager;
    private GameObject visual;
    public Text instructionText;
    bool isCreated = false;
    bool selectionIsMade = false;

    private GameObject foodPrefab;
    private GameObject cubePrefab;
    public List<GameObject> objectArray;

    public Button firstButton, secondButton;
    int objectToRender;

    void Start () {

        foodPrefab = (GameObject) Resources.Load ("prefabs/NoodleMeshCleaned", typeof (GameObject));
        cubePrefab = (GameObject) Resources.Load ("prefabs/Cube", typeof (GameObject));

        objectArray.Add (foodPrefab);
        objectArray.Add (cubePrefab);

        //foodPrefab = Instantiate(Resources.Load("prefabs/cube", typeof(GameObject))) as GameObject;

        // get the components
        rayManager = FindObjectOfType<ARRaycastManager> ();
        visual = transform.GetChild (0).gameObject;

        // hide the placement indicator visual
        visual.SetActive (false);
        firstButton.onClick.AddListener (handleClickFirstButton);
        secondButton.onClick.AddListener (handleClickSecondButton);

    }

    void handleClickFirstButton () {
        objectToRender = 0;
        Destroy (firstButton.gameObject);
        Destroy (secondButton.gameObject);
        selectionIsMade = true;
    }

    void handleClickSecondButton () {
        objectToRender = 1;
        Destroy (firstButton.gameObject);
        Destroy (secondButton.gameObject);
        selectionIsMade = true;

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

        if (selectionIsMade == true) {

            // enable the visual if it's disabled
            if (isCreated == false) {
                visual.SetActive (true);
                //instructionText.text = "Druk op het scherm om jouw gerecht te zien";

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
                        //Instantiate (cubePrefab, hitPose.position, hitPose.rotation);
                        Instantiate (objectArray[objectToRender], hitPose.position, hitPose.rotation);
                        isCreated = true;
                    }
                }
            }
        }
    }
}