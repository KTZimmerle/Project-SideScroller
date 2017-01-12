using UnityEngine;
using System.Collections;

public class PlaySFX : MonoBehaviour {

    public string type;
    public int id;
    SelectSound ss;
    
	void Awake () {
        ss = GameObject.FindGameObjectWithTag("SoundSystem").GetComponent<SelectSound>();
	}
	

	void OnEnable () {

        //awakeSFX.PlayOneShot(sfx);
        ss.FindSound(type, id);
	}
}
