using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject player;
    private Vector3 cam_pos;
    // Start is called before the first frame update
    private Quaternion cam_rot;
    private void Start()
    {
        transform.Rotate(20.0f, 0.0f, 0.0f);
    }
    // Update is called once per frame
    void Update()
    {
        cam_pos = player.transform.position;
        cam_pos.y += 3.0f;
        cam_pos.z -= 5.0f;
        transform.position = cam_pos;
    }
}
