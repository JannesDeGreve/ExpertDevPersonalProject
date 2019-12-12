using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OCRMain : MonoBehaviour {

    // CAMERA
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    //OCR
    private string url = "https://vision.googleapis.com/v1/images:annotate?key=";
    public string apiKey = "";
    private FeatureType featureType = FeatureType.TEXT_DETECTION;
    public int maxResults = 10;

    Texture2D texture2D;
    Dictionary<string, string> headers;
    // List<string> dishes;
    // public static List<string> matchedDishes;
    public static Dictionary<string, string> matchedDishesWithPath;
    public List<string> remainingDishesLeftToMatchWithGeneric;

    public Button myButton;

    [System.Serializable]
    public class AnnotateImageRequests {
        public List<AnnotateImageRequest> requests;
    }

    [System.Serializable]
    public class AnnotateImageRequest {
        public Image image;
        public List<Feature> features;
    }

    [System.Serializable]
    public class Image {
        public string content;
    }

    [System.Serializable]
    public class Feature {
        public string type;
        public int maxResults;
    }

    [System.Serializable]
    public class ImageContext {
        public List<string> languageHints;
    }

    [System.Serializable]
    public class AnnotateImageResponses {
        public List<AnnotateImageResponse> responses;
    }

    [System.Serializable]
    public class AnnotateImageResponse {
        public List<EntityAnnotation> textAnnotations;
    }

    [System.Serializable]
    public class EntityAnnotation {
        public string description;
    }

    public enum FeatureType {
        TEXT_DETECTION
    }

    // Start is called before the first frame update
    void Start () {

        headers = new Dictionary<string, string> ();
        headers.Add ("Content-Type", "application/json; charset=UTF-8");

        StartCoroutine ("FetchRestaurantData");

        if (apiKey == null || apiKey == "")
            Debug.LogError ("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");

        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0) {
            Debug.Log ("no camera detected");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++) {
            if (!devices[i].isFrontFacing) {
                backCam = new WebCamTexture (devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null) {
            Debug.Log ("Unable to find camera");
            return;
        }

        backCam.Play ();
        background.texture = backCam;

        camAvailable = true;

        Button btn = myButton.GetComponent<Button> ();
        btn.onClick.AddListener (handleClickButton);

    }

    void handleClickButton () {
        Debug.Log ("Button click");
        StartCoroutine ("Capture");

    }

    private IEnumerator Capture () {
        //while (true) {
        if (this.apiKey == null)
            yield return null;

        //yield return new WaitForSeconds (captureIntervalSeconds);

        Color[] pixels = backCam.GetPixels ();
        if (pixels.Length == 0)
            yield return null;
        if (texture2D == null || backCam.width != texture2D.width || backCam.height != texture2D.height) {
            texture2D = new Texture2D (backCam.width, backCam.height, TextureFormat.RGBA32, false);
        }

        texture2D.SetPixels (pixels);
        // texture2D.Apply(false); // Not required. Because we do not need to be uploaded it to GPU
        byte[] jpg = texture2D.EncodeToJPG ();
        string base64 = System.Convert.ToBase64String (jpg);

        AnnotateImageRequests requests = new AnnotateImageRequests ();
        requests.requests = new List<AnnotateImageRequest> ();

        AnnotateImageRequest request = new AnnotateImageRequest ();
        request.image = new Image ();
        request.image.content = base64;
        request.features = new List<Feature> ();

        Feature feature = new Feature ();
        feature.type = this.featureType.ToString ();
        feature.maxResults = this.maxResults;

        request.features.Add (feature);

        requests.requests.Add (request);

        string jsonData = JsonUtility.ToJson (requests, false);
        if (jsonData != string.Empty) {
            string url = this.url + this.apiKey;
            byte[] postData = System.Text.Encoding.Default.GetBytes (jsonData);
            using (WWW www = new WWW (url, postData, headers)) {
                yield return www;
                if (string.IsNullOrEmpty (www.error)) {

                    Debug.Log ("www text" + www.text);

                    AnnotateImageResponses responses = JsonUtility.FromJson<AnnotateImageResponses> (www.text);
                    // SendMessage, BroadcastMessage or someting like that.
                    Debug.Log ("eerste log na annotate" + responses);
                    Sample_OnAnnotateImageResponses (responses);
                } else {
                    Debug.Log ("Error: " + www.error);
                }
            }
        }
        //}
    }

    void Sample_OnAnnotateImageResponses (AnnotateImageResponses responses) {
        Debug.Log ("eerste log IN annotate");
        if (responses.responses.Count > 0) {

            if (responses.responses[0].textAnnotations != null && responses.responses[0].textAnnotations.Count > 0) {
                //Debug.Log ("er zijn textannos" + responses.responses[0].textAnnotations[0].description);
                string detectedText = responses.responses[0].textAnnotations[0].description;
                //Debug.Log ("detected" + detectedText);

                string[] detectedTextArray = detectedText.Split (
                    new [] { "\r\n", "\r", "\n" },
                    System.StringSplitOptions.None
                );

                // CHECK THE MATCHES

                matchedDishesWithPath = new Dictionary<string, string> ();
                remainingDishesLeftToMatchWithGeneric = new List<string> ();

                //matchedDishes = new List<string> ();

                // loop door alle gedecteerde woorden
                foreach (string s in detectedTextArray) {
                    Debug.Log ("in detectedtextarray loop");

                    // rest de boolean
                    bool hasBeenAdded = false;

                    if (RestaurantSelectionScreen.allDishesFromCurrentRestaurant != null) {
                        Debug.Log ("In specifieke dishes loop");
                        //loop door alle specifieke gerechten
                        foreach (KeyValuePair<string, string> keyValue in RestaurantSelectionScreen.allDishesFromCurrentRestaurant) {
                            if (keyValue.Key.ToLower () == s.ToLower ()) {

                                // Er is een match tussen de gedetecteerde tekst en locatie gebaseerde menukaart
                                Debug.Log ("Er is een match: " + s);
                                matchedDishesWithPath.Add (keyValue.Key, keyValue.Value);
                                hasBeenAdded = true;
                            }
                        }

                        // Er was geen match tussen de gedetecteerde tekst
                        // Voeg toe aan de volgende List die met generieke gerechten checkt
                        if (hasBeenAdded == false) {
                            remainingDishesLeftToMatchWithGeneric.Add (s);

                        }
                    } else {
                        Debug.Log ("specifieke loop overgeslagen");

                        remainingDishesLeftToMatchWithGeneric.Add (s);
                    }

                }

                foreach (string stringsLeftToCheck in remainingDishesLeftToMatchWithGeneric) {
                    foreach (KeyValuePair<string, string> keyValue in RestaurantSelectionScreen.genericDishes) {
                        if (keyValue.Key.ToLower () == stringsLeftToCheck.ToLower ()) {
                            Debug.Log ("Er is een match: " + stringsLeftToCheck);
                            matchedDishesWithPath.Add (keyValue.Key, keyValue.Value);
                        }
                    }
                }

                // foreach (string detectedString in dishes) {

                //     if (detectedString.ToLower () == s.ToLower ()) {
                //         Debug.Log ("Er is een match: " + s);
                //         matchedDishes.Add (s);

                //     }
                //     // Debug.Log ("detected string" + detectedString.ToLower ());
                //     // Debug.Log ("dishes array" + s.ToLower ());
                // }

            }

            //Debug.Log (matchedDishes);

            // foreach (string matchedText in matchedDishes) {
            //     Debug.Log (matchedText);
            // }

            SceneManager.LoadScene ("ARDishSelectionMenu");

        }

    }

    // Update is called once per frame
    void Update () {

        if (!camAvailable) {
            return;
        }

        float ratio = (float) backCam.width / (float) backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3 (1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3 (0, 0, orient);
    }
}