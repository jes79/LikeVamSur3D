using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))] 
public class Player_Movenment : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    private Vector3 moveDir;

    private CharacterController controller;
    private Animator animator;


    public Vector3 cameraDir = Vector3.zero;


    private void Start()
    {

        controller = GetComponent<CharacterController>();   
        animator = GetComponent<Animator>();
        

    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Animate();
        CameraMove();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(h,0, v).normalized;

        //Move(), SimpleMove()
        controller.SimpleMove(moveDir*moveSpeed);
    }

    void Rotate()
    {
        if (moveDir.sqrMagnitude > 0.01)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f*Time.deltaTime);
        }
    }

    void Animate()
    {
        animator.SetFloat("SPEED", moveDir.magnitude);
    }

    void CameraMove()
    {
        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position,
            transform.position + cameraDir, 
            2f*Time.deltaTime);
    }
}
