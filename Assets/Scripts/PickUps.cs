using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour {

    public PowerUpSystem player;

    void Start()
    { 
        
    }

    //pick up handler
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerShip")
        {
            switch(GetComponent<Collider>().tag)
            {
                case "MissilePickUp":
                    player = other.GetComponent<PowerUpSystem>();
                    player.missilePowUp = true;
                    break;
            }
            Destroy(gameObject);
        }
    }
}
