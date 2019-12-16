/* 

Simple spinner animation, adopted from this post:
https://www.salusgames.com/2017/01/08/circle-loading-animation-in-unity3d/

*/

using UnityEngine;

public class Progress : MonoBehaviour
{
    private RectTransform rectComponent;
    private float rotateSpeed = -250f;

    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}