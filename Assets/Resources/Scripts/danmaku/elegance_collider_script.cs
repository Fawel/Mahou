using UnityEngine;
using System.Collections;

public class elegance_collider_script : MonoBehaviour {

    PlayerMovement pl;
	// Use this for initialization
	void Start () {
        pl = this.gameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Danmaku"))
        {
            print("ELegance");
           pl.elegance_score++;
        }
    }
}
