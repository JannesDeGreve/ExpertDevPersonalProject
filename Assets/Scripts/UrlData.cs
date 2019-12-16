using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlData : MonoBehaviour
{
    // static variables to use across the whole project
    public static string cloudVisionApiUrl = "https://vision.googleapis.com/v1/images:annotate?key=";
    public static string assetBundleUrl = "http://jannesdegreve.be/assetbundles";
    public static string allRestaurantsUrl = "https://firestore.googleapis.com/v1/projects/arfoodappocr/databases/(default)/documents/restaurants";
    public static string genericDishesUrl = "https://firestore.googleapis.com/v1/projects/arfoodappocr/databases/(default)/documents/generic_dishes/generic_list";

}
