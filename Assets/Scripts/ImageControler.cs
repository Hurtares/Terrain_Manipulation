using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageControler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeBuilding(int building) {
        EditarTerreno.index = building;
    }

}
