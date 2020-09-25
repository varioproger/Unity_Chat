using UnityEngine;
using TCP;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Player player;
    public float sensitivity = 100f;
    public float clampAngle = 85f;

    private void Start()
    {
        TCPClient.m_Player.PlayerInitRotation(transform.localEulerAngles.x, player.transform.eulerAngles.y);
        StartCoroutine("Mouse_Movement");
    }

    private void Update()
    {
       
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);

    }
    IEnumerator Mouse_Movement()
    {
        while(true)
        {
            TCPClient.m_Player.PlayerRotationCheck(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
            yield return new WaitForSeconds(0.1f);
        }

    }
}
