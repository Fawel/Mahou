using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Patterns))]
public class test_Creature : MonoBehaviour {
    Patterns pattern;
	FireData data;
	// Use this for initialization
	void Start () {
        pattern = this.gameObject.GetComponent<Patterns>();
		data = this.gameObject.GetComponent<FireData>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.I))
        {
            //StartCoroutine(pattern.pattern_1());
			//StartCoroutine(pattern.pattern_2());
			StartCoroutine(pattern.Ethan_round_pattern());
        }
	}
}
