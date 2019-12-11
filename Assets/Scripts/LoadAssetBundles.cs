using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleZip {

    public class LoadAssetBundles : MonoBehaviour {

        AssetBundle myLoadedAssetBundle;
        //public string path;
        public string assetName;
        private string url = "http://jannesdegreve.be/assetbundles/";
        private string urlPart = "osx/spaghettibolognese";

        private string fullpath = "casadimama/biefstukmetfrieten";
        private string dishNameToRender;

        void Start () {
            // LoadAssetBundle (path);
            // InstantiateObjectFromBundle (assetName);
            //instantiateObject ();
            StartCoroutine ("instantiateObject");
            // dishNameToRender = fullpath.Substring (fullpath.IndexOf ('/') + 1);
            // Debug.Log ("dish name to render: " + dishNameToRender);
        }

        // void LoadAssetBundle (string bundleUrl) {

        //     myLoadedAssetBundle = AssetBundle.LoadFromFile (bundleUrl);

        //     Debug.Log (myLoadedAssetBundle == null ? "Failed to load assetBundle" : "AssetBundle succesfully loaded");

        // }

        // void InstantiateObjectFromBundle (string objectName) {
        //     var prefab = myLoadedAssetBundle.LoadAsset (objectName);
        //     Instantiate (prefab);
        // }

        IEnumerator instantiateObject () {
            Debug.Log (url + urlPart);
            //string url = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle (url + urlPart, 0);
            yield return request.SendWebRequest ();

            if (request.isNetworkError) {
                Debug.Log ("Error: " + request.error);
            } else {
                Debug.Log (request.downloadHandler);
            }
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent (request);
            GameObject noodles = bundle.LoadAsset<GameObject> (assetName);

            Debug.Log (noodles == null ? "Failed to load assetBundle" : "AssetBundle succesfully loaded");

            //GameObject sprite = bundle.LoadAsset<GameObject> ("Sprite");
            Instantiate (noodles);
            Debug.Log (noodles);
            //Instantiate (sprite);

        }
    }
}