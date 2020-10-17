using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unity_chan_controller : MonoBehaviour
{
    private Animator animator;
    public float speed;
    Vector3 Position;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Vector3 Position = transform.position;
        Quaternion Rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 new_Direction = CamForward * vert + Camera.main.transform.right * horz;


        animator.SetFloat("Velocity_X", new_Direction.x * 10f );
        animator.SetFloat("Velocity_Z", new_Direction.z * 10f);
   

        Position.x += horz * speed * Time.deltaTime;
        Position.z += vert * speed * Time.deltaTime;


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new_Direction), 4 * Time.deltaTime);
        
        transform.position = Position;
    }
}
