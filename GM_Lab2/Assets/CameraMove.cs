using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{

    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {

        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.transform.Rotate(0, horizontal, 0);

        transform.LookAt(target.transform);

    }
}