using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestaurantSelectionScreen : MonoBehaviour {

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    // Start is called before the first frame update
    void Start () {

        button1.onClick.AddListener (handleClickButton);
        button2.onClick.AddListener (handleClickButton);
        button3.onClick.AddListener (handleClickButton);
        button4.onClick.AddListener (handleClickButton);

    }

    void handleClickButton () {
        SceneManager.LoadScene (1);
    }

    // Update is called once per frame
    void Update () {

    }
}