using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {
    private GameObject go;
    void Start()
    {
        go = this.gameObject;
    }
	// Update is called once per frame
	void Update () {
        if (go.tag == "pickup")
        {
            Random r;
            r = new Random();
            Vector3 rotate = new Vector3(Random.Range(10f, 50f), Random.Range(10f, 50f), Random.Range(10f, 50f));
            transform.Rotate(rotate * Time.deltaTime);
        }
        else {
            if(Input.GetKey(KeyCode.R))
            transform.Rotate(0, 1f, 0);
        }
	}
}
