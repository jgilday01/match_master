using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotFinger : MonoBehaviour
{
#if UNITY_ANDROID
    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began ) 
        {
            var hit : RaycastHit;
            var ray = GetComponent.<Camera>().ScreenPointToRay(Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y,0f));
            if (Physics.Raycast(ray, hit, Mathf.Infinity)) 
            {
                hit.collider.gameObject.GetComponent(Interaction).SelectedPiece();  //hit.rigidbody.gameObject.name;
            }
        }
    }
#endif
}
