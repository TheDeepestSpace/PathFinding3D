using UnityEngine;
using System.Collections;

public class CameraSetup : MonoBehaviour {

	void Start () {
    gameObject.transform.position = new Vector3(30, 30, 30);
    gameObject.transform.Rotate(new Vector3(35, 225, 0));
	}
}
