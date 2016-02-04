using UnityEngine;
using System.Collections;

public class BossHP : MonoBehaviour {
	float currentHP = 1000f;
	float maxHP = 1000f;
	float hpBarLenght = 0f;
	// Use this for initialization
	void Start () {
		hpBarLenght = Screen.width / 2f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.Box (new Rect (10, 10, hpBarLenght, 20), currentHP + "/" + maxHP);
	}

	void AdjustHP()
	{
		//currentHP += adj;
		currentHP = Mathf.Clamp (currentHP, 0, maxHP);
		if (maxHP<1) {
			maxHP = 1;
		}
		hpBarLenght = (Screen.width / 2) * (currentHP / maxHP);
	}
}
