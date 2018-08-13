using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        if (GameObject.FindGameObjectsWithTag("BGM").Length > 1){
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
	}
	
}
