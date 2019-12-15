using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BundleBuilder : Editor {

    [MenuItem ("Assets/ Build AssetBundles")]
    static void BuildAllAssetBundles () {
        BuildPipeline.BuildAssetBundles ("/Users/jannesdegreve/Desktop/AssetBundles/sintenco", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }

}