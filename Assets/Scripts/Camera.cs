using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
    }

    private void LateUpdate() //Bütün Updatelerden sonra çalýþan LateUpdate kullanma nedenim objeyi takip ederken atlama yapmamasýdýr.
    {
        if (GameManager.Instance._gameState == "Intro")
        {
            Offset = new Vector3(0f, 0.75f, -1.25f);
        }
        if (GameManager.Instance._gameState == "Game")
        {
            Offset = new Vector3(0f, 1.5f, -5f);
        }
        if (GameManager.Instance._gameState == "Win")
        {
            Offset = new Vector3(2.5f, 0, 3.25f);
        }
        Vector3 targetPosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime* Time.fixedDeltaTime);
        transform.LookAt(Target);
    }
}
