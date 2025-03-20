using UnityEngine;

public class test : MonoBehaviour
{

    //Speed of the vehicle
    private float speed;

    //Torque of the 'engine', increases rotational force applied to the body
    public float torque;

    //Resistance of the suspension
    public float suspensionDamping;    

    //The body of the vehicle's rigidbody
    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        ApplyRotationalForce();

        rb.AddForce(transform.forward * speed);

        if(rb.linearVelocity.magnitude > 10){
            rb.AddForce(-rb.linearVelocity);
        }
    }

    void GetInput(){
        if(Input.GetKey(KeyCode.W)){
            speed += Time.deltaTime * torque;
        }
        if(Input.GetKey(KeyCode.S)){
            speed -= Time.deltaTime * torque;
        }        
    }

    void ApplyRotationalForce(){
        rb.AddTorque(-transform.right * speed);
    }
}
