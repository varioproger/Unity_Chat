using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_Player : MonoBehaviour
{
    public int Serial_N;
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Play_Anim()
    {
       
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        animator.SetFloat("Velocity_X", horz * 10f);
        animator.SetFloat("Velocity_Z", vert * 10f);
    }
}
