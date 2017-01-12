using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectSound : MonoBehaviour {

    AudioSource awakeSFX;
    //Dictionary<AudioSource, AudioClip> soundmap;
    List<AudioClip> explosion_SFX;
    List<AudioClip> projectile_SFX;
    List<AudioClip> laser_SFX;
    List<AudioClip> missile_SFX;
    List<AudioSource> explosion_sounds;
    List<AudioSource> projectile_sounds;
    List<AudioSource> laser_sounds;
    List<AudioSource> missile_sounds;

    void Awake () {

        explosion_SFX = new List<AudioClip>();
        projectile_SFX = new List<AudioClip>();
        laser_SFX = new List<AudioClip>();
        missile_SFX = new List<AudioClip>();
        explosion_sounds = new List<AudioSource>();
        projectile_sounds = new List<AudioSource>();
        laser_sounds = new List<AudioSource>();
        missile_sounds = new List<AudioSource>();

        foreach (AudioSource s in transform.GetChild(0).GetComponents<AudioSource>())
        {
            explosion_sounds.Add(s);
            explosion_SFX.Add(s.clip);
        }
        
        foreach (AudioSource s in transform.GetChild(1).GetComponents<AudioSource>())
        {
            projectile_sounds.Add(s);
            projectile_SFX.Add(s.clip);
        }

        foreach (AudioSource s in transform.GetChild(2).GetComponents<AudioSource>())
        {
            laser_sounds.Add(s);
            laser_SFX.Add(s.clip);
        }

        foreach (AudioSource s in transform.GetChild(3).GetComponents<AudioSource>())
        {
            missile_sounds.Add(s);
            missile_SFX.Add(s.clip);
        }
    }
	

	public void FindSound (string type, int ID) {

        if (type == null)
            return;

        if (type.Equals("Explosion"))
        {
            explosion_sounds[ID].PlayOneShot(explosion_SFX[ID]);
        }
        else if (type.Equals("Projectile"))
        {
            projectile_sounds[ID].PlayOneShot(projectile_SFX[ID]);
        }
        else if (type.Equals("Laser"))
        {
            laser_sounds[ID].PlayOneShot(laser_SFX[ID]);
        }
        else if (type.Equals("Missile"))
        {
            missile_sounds[ID].PlayOneShot(missile_SFX[ID]);
        }
    }
}
