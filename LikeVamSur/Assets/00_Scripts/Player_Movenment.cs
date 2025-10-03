using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))] 
public class Player_Movenment : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Vector3 moveDir;

    //public float detectionRadius = 10.0f;
    //public LayerMask monsterLayer;

    private CharacterController controller;
    private Animator animator;


    public Vector3 cameraDir = Vector3.zero;


    //private Transform target;

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
        //Transform nearest = GetNearestMonster();
        //Player.Instance.target = Player.Instance.GetNearestMonster();



        if (Player.instance.target != null)
        {
            //Vector3 dirToMonster = (Player.instance.target.position - transform.position);
            Vector3 dirToMonster = Player.instance.Direction();
            dirToMonster.y = 0; 

            RotateToQuaternion(dirToMonster);
        }

        else if (moveDir.sqrMagnitude > 0.01)
        {
            //중복되어 함수로 빼서 사용 RotateToQuaternion(Vector3 direction)
            //Quaternion targetRot = Quaternion.LookRotation(moveDir);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f*Time.deltaTime);
            RotateToQuaternion(moveDir);
        }
    }

    void RotateToQuaternion(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
    }
/*
    public Transform GetNearestMonster()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,detectionRadius, monsterLayer);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider col in hits)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                nearest = col.transform;
            }
        }

        return nearest;

    }
*/

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
