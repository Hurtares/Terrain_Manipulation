using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

    public delegate void TestEvent();
    public delegate void CollisionEvent(Collider collision);

    public static event TestEvent eventoTeste;
    public static event CollisionEvent onColision;
    public static event CollisionEvent outColision;

	// Use this for initialization
	void Start () {
		
	}

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            if (eventoTeste != null)

                eventoTeste();
        }
    }
    // Update is called once per frame
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Edificio") {
            Debug.Log("a luciana Trigger out");
            if (outColision != null)
                outColision(other);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Edificio") {
            Debug.Log("a luciana Trigger");
            if (onColision != null)
                onColision(other);
        }
    }
    private void OnCollisionEnter(Collision collision) {
        /*Debug.Log("a luciana");
        if (collision.gameObject.tag == "Edifício") {
            if (onColision != null)
                onColision(collision);
        }*/
    }
}
