using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    private GameObject playerobject;
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        playerobject = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - playerobject.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position=playerobject.transform.position + offset;
	}
}
