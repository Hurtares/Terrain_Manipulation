using UnityEngine;
using System.Collections;

public class AICubo : MonoBehaviour {
    public float speed=10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 velocidade = new Vector3(speed,0,0);
        transform.Translate(velocidade * Time.deltaTime , Space.Self);
        transform.Rotate(0 , 4 , 0 , Space.Self);
	}
}
