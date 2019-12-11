using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleZip {

    public class LoadAssetBundles : MonoBehaviour {

        AssetBundle myLoadedAssetBundle;
        public string path;
        public string assetName;
        //private string uri = "https://storage.googleapis.com/storage/v1/b/ocrfoodappbucket/o/noodles%2Fnoodles.zip";
        //public string url = "https://storage.googleapis.com/ocrfoodappbucket/noodles/noodles";
        private string url = "http://jannesdegreve.be/assetbundles/noodles";
        // Start is called before the first frame update
        void Start () {
            // LoadAssetBundle (path);
            // InstantiateObjectFromBundle (assetName);
            //instantiateObject ();
            StartCoroutine ("instantiateObject");
        }

        void LoadAssetBundle (string bundleUrl) {

            myLoadedAssetBundle = AssetBundle.LoadFromFile (bundleUrl);

            Debug.Log (myLoadedAssetBundle == null ? "Failed to load assetBundle" : "AssetBundle succesfully loaded");

        }

        void InstantiateObjectFromBundle (string objectName) {
            var prefab = myLoadedAssetBundle.LoadAsset (objectName);
            Instantiate (prefab);
        }

        IEnumerator instantiateObject () {
            //string url = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle (url, 0);
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
            //Instantiate (sprite);

        }
    }
}