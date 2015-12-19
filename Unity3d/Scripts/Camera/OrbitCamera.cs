using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

    public Transform _target;

    public float _distance = 20.0f; 

    public float _zoomStep = 1.0f;

    public float _xSpeed = 1f;
    public float _ySpeed = 1f;

    private float _x = 0.0f;
    private float _y = 0.0f;

    private Vector3 _distanceVector;

    void Start() {
        _distanceVector = new Vector3(0.0f, 0.0f, -_distance);

        Vector2 angles = this.transform.localEulerAngles;
        _x = angles.x;
        _y = angles.y;

        this.Rotate(_x, _y);

    }

    void LateUpdate() {
        if (_target) {
            this.RotateControls();
            this.Zoom();
        }
    }

    void RotateControls() {
        if (Input.GetButton("Fire1")) {
            _x += Input.GetAxis("Mouse X") * _xSpeed;
            _y += -Input.GetAxis("Mouse Y") * _ySpeed;

            this.Rotate(_x, _y);
        }

    }

    void Rotate(float x, float y) {
        //Transform angle in degree in quaternion form used by Unity for rotation.
        Quaternion rotation = Quaternion.Euler(y, x, 0.0f);

        //The new position is the target position + the distance vector of the camera
        //rotated at the specified angle.
        Vector3 position = rotation * _distanceVector + _target.position;

        //Update the rotation and position of the camera.
        transform.rotation = rotation;
        transform.position = position;
    }

    void Zoom() {
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) {
            this.ZoomOut();
        } else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) {
            this.ZoomIn();
        }

    }

    void ZoomIn() {
        _distance -= _zoomStep;
        _distanceVector = new Vector3(0.0f, 0.0f, -_distance);
        this.Rotate(_x, _y);
    }

    void ZoomOut() {
        _distance += _zoomStep;
        _distanceVector = new Vector3(0.0f, 0.0f, -_distance);
        this.Rotate(_x, _y);
    }

}
