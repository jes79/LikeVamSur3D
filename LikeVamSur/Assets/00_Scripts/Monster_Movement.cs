using System.Collections;
using UnityEngine;

public class Monster_Movement : MonoBehaviour
{

    public Transform target;
    public float speed = 3.0f;

    private Rigidbody rb;
    private Animator animator;

    private bool isSpawned = false;


    /*
    public void SetTarget(Transform player)
    {
        target = player;
    }
    */

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void Initialize(Transform player)
    {
        target = player;

        Rotate(Direction(), false);


        StartCoroutine(SpawnStartCoroutine(transform.localScale));
    }

    IEnumerator SpawnStartCoroutine(Vector3 scaleEnd)
    {

        Vector3 ScaleStart = Vector3.zero;
        Vector3 ScaleEnd = scaleEnd;

        float duration = 0.5f;
        float timer = 0.0f;

        while (timer < duration)
        {
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(ScaleStart, ScaleEnd, t);
            timer += Time.deltaTime;
            yield return null;
        }


        isSpawned = true;
        animator.SetTrigger("Move");
    }

    private void FixedUpdate()
    {
        //if (target == null) return;
        if (!isSpawned) return;
         
        MoveAndRotate();

    }

    private void MoveAndRotate()
    {
        Rotate(Direction());
        

        rb.MovePosition(rb.position + Direction() * speed * Time.fixedDeltaTime);


    }


    Vector3 Direction()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;//수평으로만 회전하도록

        return direction;
    }

    void Rotate(Vector3 direction, bool Lerp = true)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (Lerp)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetRotation;
            }
            

        }

    }


}
