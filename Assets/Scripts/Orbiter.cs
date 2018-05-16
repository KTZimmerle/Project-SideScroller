using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour {

    float startX;
    float startY;
    float radius;
    float theta;
    public string oppoOrbName;
    GameObject playerShip;
    GameObject oppositeOrb;
    PlayerController pc;
    Quaternion originalRot;
    Vector3 startPosition;

    void Awake()
    {
        radius = 2.0f;
        theta = 0.0f;
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        //playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
    }

    void OnEnable()
    {
        playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        //transform.SetParent(playerShip.transform);
        pc = playerShip.GetComponent<PlayerController>();
        originalRot = transform.rotation;
        startPosition = new Vector3(startX, startY, 0.0f);
        if (oppositeOrb != null && oppositeOrb.activeSelf)
        {
            transform.position = playerShip.transform.position + (startPosition).normalized
                                 * radius;
            theta = (oppositeOrb.GetComponent<Orbiter>().GetTheta() + (360.0f * Time.deltaTime)) % 360.0f;
            transform.RotateAround(playerShip.transform.position, Vector3.forward, theta);
            transform.rotation = originalRot;
        }
        else
            transform.position = playerShip.transform.position + (startPosition).normalized
                                      * radius;

    }

    private void FixedUpdate()
    {

        Vector3 DirResultant = Vector3.zero;

        //Check for Vertical Movement & for Horizontal Movement

        DirResultant.y = Input.GetAxis("PlayerShipV") * Time.deltaTime;
        DirResultant.x = Input.GetAxis("PlayerShipH") * Time.deltaTime;

        if (transform.position.x + DirResultant.x < pc.GetGameBOunds().xMin ||
           transform.position.x + DirResultant.x > pc.GetGameBOunds().xMax)
        {
            DirResultant.x = 0;
        }

        if (transform.position.y + DirResultant.y < pc.GetGameBOunds().yMin ||
           transform.position.y + DirResultant.y > pc.GetGameBOunds().yMax)
        {
            DirResultant.y = 0;
        }

        //Combine both horizontal and vertical to create movement!
        GetComponent<Rigidbody>().velocity = pc.GetSpeed() * DirResultant;
    }

    // Update is called once per frame
    void LateUpdate () {
        if (playerShip != null)
        {
            //transform.position - playerShip.transform.position
            transform.position = playerShip.transform.position + (startPosition).normalized
                                 * radius;
            theta = (theta + (360.0f * Time.deltaTime)) % 360.0f;
            transform.RotateAround(playerShip.transform.position, Vector3.forward, theta);

            transform.rotation = originalRot;
        }
    }

    public void SetStartPosition(float x = 0.0f, float y = 1.0f)
    {
        startX = x;
        startY = y;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }

    public float GetTheta()
    {
        return theta;
    }

    public void SetOppositeOrb(GameObject orb)
    {
        oppositeOrb = orb;
    }

    public Vector3 GetOppositeOrbPos()
    {
        return new Vector3(oppositeOrb.GetComponent<Orbiter>().startX, oppositeOrb.GetComponent<Orbiter>().startY, 0.0f);
    }

    /*public void SetStartPosition()
    {
        //it will do nothing to itself
        if (this.gameObject == gameObject)
            return;

        Vector3 newStartPos = (playerShip.transform.position + transform.position).normalized;
        startX = newStartPos.x;
        startY = newStartPos.y;
    }*/
    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
