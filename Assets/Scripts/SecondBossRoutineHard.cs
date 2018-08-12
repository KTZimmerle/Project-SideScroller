using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossRoutineHard : SecondBossRoutine
{

    int patternID;
    protected override void Awake()
    {
        base.Awake();
        for (int drones = 0; drones < bosses.Count; drones++)
        {
            bosses[drones].GetComponent<LaserBeam>().ToggleLaser(false);
        }
        patternID = 0;
        //GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.yellow);
        /*transform.GetChild(0).GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.yellow);
        transform.GetChild(1).GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.yellow);
        transform.GetChild(2).GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.yellow);*/
    }

    protected void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < transform.childCount; i++)
        {
            bosses[i].GetComponent<AIBossDrone>().SetHardModeOn();
        }
        BossAttacks = HardBossRoutine();
    }

    protected void Start()
    {
        base.Start();
    }

    IEnumerator HardBossRoutine()
    {
        while (notDead)
        {
            yield return new WaitForSeconds(1.0f);
            //rotate pattern
            isMovingRotation = true;
            for (int seconds = 0; seconds < 15; seconds++)
            {
                bosses[firingOrder].GetComponent<AIBossDrone>().FireBullet();
                bosses[(firingOrder + 1) % 7].GetComponent<AIBossDrone>().FireBullet();
                bosses[(firingOrder + 2) % 7].GetComponent<AIBossDrone>().FireBullet();
                if (seconds > 5)
                {
                    bosses[(firingOrder + 3) % 7].GetComponent<AIBossDrone>().FireBullet();
                }
                if (seconds > 10)
                {
                    bosses[(firingOrder + 4) % 7].GetComponent<AIBossDrone>().FireBullet();
                }
                if (seconds == 12)
                {
                    for (int i = 0; i < bosses.Count; i++)
                        bosses[i].GetComponent<AIBossDrone>().PlayLasFX();
                }
                yield return new WaitForSeconds(0.75f);
                firingOrder = (firingOrder + 1) % 7;
            }
            yield return new WaitForSeconds(1.5f);
            for (int drones = 0; drones < bosses.Count; drones++)
            {
                bosses[drones].GetComponent<LaserBeam>().ToggleLaser(true);
            }
            yield return new WaitForSeconds(8.5f);
            for (int drones = 0; drones < bosses.Count; drones++)
            {
                bosses[drones].GetComponent<LaserBeam>().ToggleLaser(false);
            }
            yield return new WaitForSeconds(0.5f);
            isMovingRotation = false;
            yield return new WaitForSeconds(1.0f);

            //first movement pattern
            isMovingStraight = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<AIBossDrone>().startMovementPatternOne();
                yield return new WaitForSeconds(1.0f);
            }

            yield return new WaitForSeconds(4.5f);
            isMovingStraight = false;
            yield return new WaitForSeconds(0.5f);

            //rotate pattern
            isMovingRotation = true;
            for (int seconds = 0; seconds < 15; seconds++)
            {
                bosses[firingOrder].GetComponent<AIBossDrone>().FireBullet();
                bosses[(firingOrder + 2) % 7].GetComponent<AIBossDrone>().FireBullet();
                bosses[(firingOrder + 4) % 7].GetComponent<AIBossDrone>().FireBullet();
                if (seconds > 5)
                {
                    bosses[(firingOrder + 6) % 7].GetComponent<AIBossDrone>().FireBullet();
                }
                if (seconds > 10)
                {
                    bosses[(firingOrder + 1) % 7].GetComponent<AIBossDrone>().FireBullet();
                }
                if (seconds == 13)
                {
                    for (int i = 0; i < bosses.Count; i++)
                        bosses[i].GetComponent<AIBossDrone>().PlayLasFX();
                }
                yield return new WaitForSeconds(0.75f);
                firingOrder = (firingOrder + 1) % 7;
            }
            yield return new WaitForSeconds(0.5f);
            ChangeRotationDirection();
            yield return new WaitForSeconds(2.0f);

            for (int drones = 0; drones < bosses.Count; drones++)
            {
                bosses[drones].GetComponent<LaserBeam>().ToggleLaser(true);
            }
            yield return new WaitForSeconds(8.5f);
            for (int drones = 0; drones < bosses.Count; drones++)
            {
                bosses[drones].GetComponent<LaserBeam>().ToggleLaser(false);
            }

            yield return new WaitForSeconds(0.5f);
            ChangeRotationDirection();
            isMovingRotation = false;
            yield return new WaitForSeconds(1.0f);

            //select a random second movement pattern
            patternID = Random.Range(1, 4);
            if(patternID == 1)
                SecondMovePatternOne();
            else if (patternID == 2)
                SecondMovePatternTwo();
            else
                SecondMovePatternThree();
            Debug.Log(patternID);

            yield return new WaitUntil(() => transform.GetChild(0).GetComponent<AIBossDrone>().IsMovementTwoDone());
            yield return new WaitUntil(() => transform.GetChild(6).GetComponent<AIBossDrone>().IsMovementTwoDone());

            for (int i = 0; i < bosses.Count; i++)
            {
                bosses[i].GetComponent<AIBossDrone>().RedoEntrance();
                bosses[i].transform.RotateAround(Vector3.zero, Vector3.forward, 360 / 7 * i);
                bosses[i].transform.LookAt(new Vector3(bosses[i].transform.position.x, bosses[i].transform.position.y, 0.0f));
                bosses[i].transform.GetComponent<Rigidbody>().velocity = ((Vector3.zero - bosses[i].transform.position) / (Vector3.zero - bosses[i].transform.position).magnitude) * 5.0f;
                //bosses[i].GetComponent<AIBossDrone>().RedoEntrance();
            }

            yield return new WaitUntil(() => transform.GetChild(0).GetComponent<AIBossDrone>().isEntranceDone());
            yield return new WaitForSeconds(1.0f);

            /*for (int i = 0; i < bosses.Count; i++)
            {
                bosses[i].GetComponent<AIBossDrone>().ReturnToStart();
            }*/
            //repeat from the top
        }
    }

    void SecondMovePatternOne()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bosses[i].GetComponent<AIBossDrone>().SetDelay(i * 0.25f);
            bosses[i].GetComponent<AIBossDrone>().startMovementPatternTwo();
            bosses[i].GetComponent<AIBossDrone>().SetOutofMapPosition(OutofMapYCoordinates[i], flipX);
            flipX = !flipX;
        }
    }

    void SecondMovePatternTwo()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            bosses[i].GetComponent<AIBossDrone>().SetDelay(i * 0.25f);
            bosses[i].GetComponent<AIBossDrone>().startMovementPatternTwo();
            bosses[i].GetComponent<AIBossDrone>().SetOutofMapPosition(OutofMapYCoordinates[i], flipX);
            flipX = !flipX;
        }
    }
    void SecondMovePatternThree()
    {
        int i = 0;
        int middle = transform.childCount / 2;
        while (i <= middle)
        {
            if (i == 0)
            {
                bosses[middle].GetComponent<AIBossDrone>().SetDelay(i * 0.5f);
                bosses[middle].GetComponent<AIBossDrone>().startMovementPatternTwo();
                bosses[middle].GetComponent<AIBossDrone>().SetOutofMapPosition(OutofMapYCoordinates[middle], flipX);
                flipX = !flipX;
            }
            else
            {
                bosses[middle + i].GetComponent<AIBossDrone>().SetDelay(i * 0.5f);
                bosses[middle + i].GetComponent<AIBossDrone>().startMovementPatternTwo();
                bosses[middle + i].GetComponent<AIBossDrone>().SetOutofMapPosition(OutofMapYCoordinates[middle + i], flipX);
                flipX = !flipX;
                bosses[middle - i].GetComponent<AIBossDrone>().SetDelay(i * 0.5f);
                bosses[middle - i].GetComponent<AIBossDrone>().startMovementPatternTwo();
                bosses[middle - i].GetComponent<AIBossDrone>().SetOutofMapPosition(OutofMapYCoordinates[middle - i], flipX);
                flipX = !flipX;
            }
            i++;
        }
    }
}
