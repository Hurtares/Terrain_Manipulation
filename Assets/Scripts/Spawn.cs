using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

    public Object SpawnObject;
    private bool isDown=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 spawnPoint = new Vector3();
        Quaternion spawnDirection = new Quaternion();
        spawnPoint = transform.position;
        if(Input.GetKeyDown(KeyCode.Space)) {
            isDown = true;
            Instantiate(SpawnObject , spawnPoint,spawnDirection);
        }
	}
}
