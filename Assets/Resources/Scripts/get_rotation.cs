using UnityEngine;
using System.Collections;

public class get_rotation : MonoBehaviour {
    public float rotaion;
	// Use this for initialization
	void Start () {
        rotaion = this.transform.rotation.eulerAngles.y;
    }
	
	// Update is called once per frame
	void Update () {
        rotaion = this.transform.rotation.eulerAngles.y;
	}
}
