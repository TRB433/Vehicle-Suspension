using UnityEngine;

public class test : MonoBehaviour
{

    //Speed of the vehicle
    private float speed;

    [Header("Engine")]
    //Torque of the 'engine', increases rotational force applied to the body
    public float torque;

    [Header("Suspension")]
    //Resistance of the suspension
    public float damping;    
    public float stiffness;

    //The body of the vehicle's rigidbody
    private Rigidbody rb;
    private Rigidbody thisRb;

    [Header("Vehicle Parts")]
    public GameObject body;
    public GameObject[] wheels;

    private Vector2 lastForwardVelocity;
    private Vector2 forwardAcceleration;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody>();
        thisRb = GetComponent<Rigidbody>();

        forwardAcceleration = Vector2.zero;
        lastForwardVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        GetInput();
        rb.AddForce(transform.forward * speed);

        //Moving the wheels with the body
        //TODO: Fix wheel offsets
        foreach(GameObject wheel in wheels){
            switch(wheel.GetComponent<WheelType>().wheelType){
                case WheelType.WheelPosition.LeftFront:
                    wheel.transform.position = new Vector3(body.transform.position.x + wheel.GetComponent<WheelType>().LeftFrontOffset.x, wheel.transform.position.y, body.transform.position.z + wheel.GetComponent<WheelType>().LeftFrontOffset.y);
                    break;
                case WheelType.WheelPosition.RightFront:
                    wheel.transform.position = new Vector3(body.transform.position.x + wheel.GetComponent<WheelType>().RightFrontOffset.x, wheel.transform.position.y, body.transform.position.z + wheel.GetComponent<WheelType>().RightFrontOffset.y);
                    break;
                case WheelType.WheelPosition.LeftRear:
                    wheel.transform.position = new Vector3(body.transform.position.x + wheel.GetComponent<WheelType>().LeftRearOffset.x, wheel.transform.position.y, body.transform.position.z + wheel.GetComponent<WheelType>().LeftRearOffset.y);
                    break;
                case WheelType.WheelPosition.RightRear:
                    wheel.transform.position = new Vector3(body.transform.position.x + wheel.GetComponent<WheelType>().RightRearOffset.x, wheel.transform.position.y, body.transform.position.z + wheel.GetComponent<WheelType>().RightRearOffset.y);
                    break;
            }
        }
        
        ApplySuspensionForce();            
        
        ApplyDamping();      

        ApplyRotationalForce();
    }

    //Increases the speed of the vehicle, taking into account the torque
    void GetInput(){
        if(Input.GetKey(KeyCode.W)){
            speed += Time.deltaTime * torque;                
        }
        if(Input.GetKey(KeyCode.S)){
            speed -= Time.deltaTime * torque;
        }        
    }

    //Forward/Backward body rotation when accelerating/decelerating
    void ApplyRotationalForce(){       
        //Calculating the acceleration of the vehicle
        forwardAcceleration = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z) - lastForwardVelocity;
        lastForwardVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);        

        //Applying torque to the body when accelerating/decelerating
        //Only applies when the body is not already rotated too much
        if(body.transform.rotation.x > -0.05f && (forwardAcceleration.x > 0.0f || forwardAcceleration.y > 0.0f)){
            rb.AddTorque(-transform.right * forwardAcceleration.magnitude * 3.0f, ForceMode.Impulse);
        }
        else if(body.transform.rotation.x < 0.05f && (forwardAcceleration.x < 0.0f || forwardAcceleration.y < 0.0f)){
            rb.AddTorque(transform.right * forwardAcceleration.magnitude * 3.0f, ForceMode.Impulse);
        }

        //Constantly applying a torque in the opposite direction that it is currently rotating so it naturally corrects itself
        if(body.transform.rotation.x < 0.0f){
            rb.AddTorque(transform.right * 7.5f);
        }
        else{
            rb.AddTorque(-transform.right * 7.5f);
        }
    }

    void ApplySuspensionForce(){
        //Calculate the average distance from the wheels to the body
        float averageDistanceFromWheel = 0;

        foreach(GameObject wheel in wheels){
            averageDistanceFromWheel += Vector3.Distance(wheel.transform.position, body.transform.position);
        }

        averageDistanceFromWheel /= wheels.Length;

        //If the average distance from the wheels to the body is less than 3, apply an upwards force to the body
        //The force applied takes into account the distance from the wheels and the stiffness of the suspension
        if(averageDistanceFromWheel < 3f){
            rb.AddForce(transform.up * (stiffness - averageDistanceFromWheel));
        }
        else{
            //If the average distance from the wheels to the body is greater than 3, apply a downwards force to the body
            rb.AddForce(-rb.linearVelocity);
        }                                                                
    }

    //How much the suspension absorbs impact
    //A lower damping value absorbs less impact, this is emulated by applying a greater force in the opposite direction of the velocity
    void ApplyDamping(){
        rb.AddForce(-rb.linearVelocity / damping);
    }
}
