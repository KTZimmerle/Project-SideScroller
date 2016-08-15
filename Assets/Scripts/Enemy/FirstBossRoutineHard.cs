using UnityEngine;
using System.Collections;

public class FirstBossRoutineHard : FirstBossRoutine
{

    protected override void Awake()
    {
        base.Awake();
        BossAttacks = HardBossRoutine();
    }

    IEnumerator HardBossRoutine()
    {
        yield return new WaitForSeconds(1.45f);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("Engine_FX"))
                ps.Stop();
        }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        while (notDead)
        {
            //phase 1
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < 75; i++)
            {
                BE.Shoot(projectile, gunPointOne.transform.position, projectile.transform.rotation, 1.0f);
                BE.Shoot(projectile, gunPointTwo.transform.position, projectile.transform.rotation, 1.0f);
                BE.Shoot(projectile, gunPointThree.transform.position, projectile.transform.rotation, 1.0f);
                BE.Shoot(projectile, gunPointFour.transform.position, projectile.transform.rotation, 1.0f);
                BE.Shoot(projectile, gunPointOne.transform.position, projectile.transform.rotation, 1.0f, 0.25f);
                BE.Shoot(projectile, gunPointTwo.transform.position, projectile.transform.rotation, 1.0f, 0.125f);
                BE.Shoot(projectile, gunPointThree.transform.position, projectile.transform.rotation, 1.0f, -0.125f);
                BE.Shoot(projectile, gunPointFour.transform.position, projectile.transform.rotation, 1.0f, -0.25f);
                foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                {
                    if (ps.CompareTag("Muzzle_FX"))
                        ps.Play();
                }
                yield return new WaitForSeconds(0.2f);
            }
            
            yield return new WaitForSeconds(0.5f);

            //phase 2
            GetComponent<Rigidbody>().velocity = transform.up * speed;
            StartCoroutine(RotateTask(20.0f));
            yield return new WaitForSeconds(0.5f);
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 20.0f);

            for (int i = 0; i < 20; i++)
            {
                BE.Shoot(projectileTwo, gunPointFive.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointSix.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointSeven.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointEight.transform.position, transform.rotation);
                StartCoroutine(RotateTask(0.0f));
                //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                yield return new WaitForSeconds(0.3f);
                BE.Shoot(projectileTwo, gunPointFive.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointSix.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointSeven.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointEight.transform.position, transform.rotation);
                StartCoroutine(RotateTask(-20.0f));
                //transform.rotation = Quaternion.Euler(0.0f, 0.0f, -20.0f);
                yield return new WaitForSeconds(0.3f);
                BE.Shoot(projectileTwo, gunPointFive.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointSix.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointSeven.transform.position, transform.rotation);
                BE.Shoot(projectileTwo, gunPointEight.transform.position, transform.rotation);
                StartCoroutine(RotateTask(20.0f));
                yield return new WaitForSeconds(0.4f);
                //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 20.0f);

                if (!(this.transform.position.y > -1.8f && this.transform.position.y < 1.8f))
                {
                    //GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().velocity *= -1;
                }
            }

            if (this.transform.position.y < 0.0f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().position = new Vector3(GetComponent<Rigidbody>().position.x, 0.0f, 0.0f);
            StartCoroutine(RotateTask(0.0f));
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            //phase 3
            DecidePattern();
            yield return new WaitForSeconds(1.0f);
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.CompareTag("Laser_FX"))
                    ps.Play();
            }
            BE.ShootLaser(laser, lasPointOne.transform.position, laser.transform.rotation);
            BE.ShootLaser(laser, lasPointTwo.transform.position, laser.transform.rotation);
            StartCoroutine(LaserPattern);
            yield return new WaitForSeconds(20.0f);

            //phase 4
            Vector3 oPos = transform.position;
            StartCoroutine(ChargePlayer(oPos));
            yield return new WaitForSeconds(5.5f);
            //repeat from the top
        }

    }

    void DecidePattern()
    {
        int[] angles = { 120, -120, 135, -135, 150, -150 };
        int choice = Random.Range(0, 4);
        if (choice == 1)
        {
            LaserPattern = LaserPatternC(angles);
        }
        else if (choice == 2)
        {
            LaserPattern = LaserPatternB(angles);
        }
        else
        {
            LaserPattern = LaserPatternA(angles);
        }
    }

    IEnumerator RotateTask(float angle)
    {
        Quaternion oRot = transform.rotation;
        Quaternion nRot = Quaternion.Euler(0.0f, 0.0f, angle);
        for (int i = 0; i < 10; i++)
        {
            float t = 0.1f + (0.1f * i);
            transform.rotation = Quaternion.Lerp(oRot, nRot, t);
            yield return new WaitForSeconds(0.015f);
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator LaserPatternA(int[] angles)
    {
        yield return new WaitForSeconds(3.0f);
        if (stopLaser)
        {
            StopLasFX();
            yield break;
        }

        for (int i = 0; i < 3; i++)
        {
            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[i * 2]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[i * 2 + 1]);
            yield return new WaitForSeconds(1.5f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }

            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[i * 2 + 1]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[i * 2]);
            yield return new WaitForSeconds(2.0f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }
        }
    }

    IEnumerator LaserPatternB(int[] angles)
    {
        yield return new WaitForSeconds(3.0f);
        if (stopLaser)
        {
            StopLasFX();
            yield break;
        }

        for (int i = 0; i < 3; i++)
        {
            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[i * 2]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[i * 2 + 1]);
            yield return new WaitForSeconds(1.5f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }

            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[((i * 2 + 1) + 2) % 6]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[((i * 2) + 2) % 6]);
            yield return new WaitForSeconds(2.0f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }
        }
    }

    IEnumerator LaserPatternC(int[] angles)
    {
        yield return new WaitForSeconds(3.0f);
        if (stopLaser)
        {
            StopLasFX();
            yield break;
        }

        for (int i = 0; i < 3; i++)
        {
            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[5 - (i * 2)]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[4 - (i * 2)]);
            yield return new WaitForSeconds(1.5f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }

            BE.ShootLaser(reflectLaser, gunPointSix.transform.position, laser.transform.rotation, angles[i * 2]);
            BE.ShootLaser(reflectLaser, gunPointSeven.transform.position, laser.transform.rotation, angles[i * 2 + 1]);
            yield return new WaitForSeconds(2.0f);
            if (stopLaser)
            {
                StopLasFX();
                yield break;
            }
        }
    }

    IEnumerator ChargePlayer(Vector3 oPos)
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("Engine_FX"))
                ps.Play();
        }
        GetComponent<AutoRotate>().enabled = true;
        GameObject player = GameObject.FindGameObjectWithTag("PlayerShip");
        Vector3 playerPos;
        if (player != null)
            playerPos = player.transform.position;
        else
            playerPos = new Vector3(-8.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 100; i++)
        {
            transform.position = Vector3.Lerp(oPos, playerPos, 0.01f * i);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(1.0f);

        GetComponent<AutoRotate>().enabled = false;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 100; i++)
        {
            transform.position = Vector3.Lerp(playerPos, oPos, 0.01f * i);
            yield return new WaitForSeconds(0.02f);
        }
        
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("Engine_FX"))
                ps.Stop();
        }
    }
}
