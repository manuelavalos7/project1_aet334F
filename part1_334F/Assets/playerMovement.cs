using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float speed = 10f;
    private bool isGrounded = true;
    private int jumps = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Time.deltaTime * speed * Vector3.forward;
            GetComponent<Animator>().SetTrigger("walk");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Time.deltaTime * speed * Vector3.right;
            GetComponent<Animator>().SetTrigger("walk");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Time.deltaTime * speed * Vector3.left;
            GetComponent<Animator>().SetTrigger("walk");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Time.deltaTime * speed * Vector3.back;
            GetComponent<Animator>().SetTrigger("walk");
        }
        if (Input.GetKeyDown(KeyCode.Space) && (jumps >0)) { 
            GetComponent<Animator>().SetTrigger("jump");
            GetComponent<Rigidbody>().AddForce(Vector3.up * (150/3) * jumps);
            jumps--;
            isGrounded = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground") {
            isGrounded = true;
            jumps = 3;
        }
    }
}
