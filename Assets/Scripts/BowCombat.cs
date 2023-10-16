using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCombat : Combat
{
    public Transform enemyContainer;
    public Transform spawnPoint;
    public GameObject arrow, abilityArrow;

    private bool special = false;

    private float leastDist;
    Transform closest;

    // Start is called before the first frame update
    public override void Start()
    {

        if(GameManager.instance.currentScene.name == "TowerTest")
        {
            this.enabled = false;
        }

        base.Start();

        abilityMethod = Dash;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void Shoot(GameObject arrowPrefab, float damageMult = 1)
    {
        Transform enemy = FindNearestEnemy();

        if(enemy != null)
        {
            Vector3 aimTarget = enemy.position;
            Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 1f, aimTarget.z - this.transform.position.z);
            Quaternion aimDir = Quaternion.LookRotation(target);
            target.Normalize();

            GameObject arw = Instantiate(arrowPrefab, spawnPoint.position, aimDir);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(arw, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
            arw.GetComponent<ProjectileData>().PlayerShot(pc.attackModifier * damageMult * (1 + (0.5f * comboIndex)), this, special);
        }
    }


    public Transform FindNearestEnemy()
    {
        foreach (Transform enemy in enemyContainer)
        {
            float dist = Vector3.Distance(this.transform.position, enemy.transform.position);
            if (leastDist == 0)
            {
                leastDist = dist;
                closest = enemy;
            }
            else if (dist < leastDist)
            {
                leastDist = dist;
                closest = enemy;
            }
        }

        leastDist = 0;

        return closest;
    }

    public void ShootNormal()
    {
        Shoot(arrow, 1);

        //bool incrementCombo = ac.GetCurrentAnimatorStateInfo(0).IsName("attack1") && comboIndex > 0 ? false : true;

        //if (incrementCombo)
        //{

        //}

        if (comboIndex == 2) // if combo index already maxed out, reset index
        {
            comboIndex = 0;
        }
        else // if combo index not 2 or only on first attack, increase index by one
        {
            comboIndex += 1;
        }

    }

    public void AttackReady()
    {
        pm.MoveInterrupt(true);
        readyToAtk = true;
    }

    public void ShootNormNoCombo()
    {
        Shoot(arrow, 1);

    }

    private void BowAbility()
    {
        special = true;

        Shoot(abilityArrow, 500);

        special = false;
    }

    private void Dash()
    {
        pc.GetComponent<DashingScript>().Dash();
    }

}
