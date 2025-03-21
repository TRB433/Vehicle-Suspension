using UnityEngine;

public class WheelType : MonoBehaviour
{

    //Used to identify which wheel this is so it can be positioned correctly
    public enum WheelPosition{LeftFront, RightFront, LeftRear, RightRear}
    public WheelPosition wheelType;

    //Wheel offsets from the center of the vehicle body
    [HideInInspector]
    public Vector2 LeftFrontOffset = new Vector2(-0.75f, 0.4f);
    [HideInInspector]
    public Vector2 RightFrontOffset = new Vector2(0.75f, 0.4f);
    [HideInInspector]
    public Vector2 LeftRearOffset = new Vector2(-0.75f, -0.4f);
    [HideInInspector]
    public Vector2 RightRearOffset = new Vector2(0.75f, -0.4f);

    private LineRenderer suspensionLine;

    public GameObject suspensionPoint;

    void Start(){
        suspensionLine = GetComponent<LineRenderer>();
    }

    //Draws a line from the wheel to the body of the car to visualise the spring    
    void Update(){
        suspensionLine.SetPosition(0, transform.position);
        suspensionLine.SetPosition(1, suspensionPoint.transform.position);
    }
    
}
