using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCombat : Combat
{
    public Transform enemyContainer;
    public Transform spawnPoint;
    public GameObject arrow, abilityArrow;

    private float leastDist;
    Transform closest;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        abilityMethod = BowAbility;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void Shoot(GameObject arrowPrefab, float damageMult = 1)
    {
        Transform enemy = FindNearestEnemy();
        Vector3 aimTarget = enemy.position;
        Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
        Quaternion aimDir = Quaternion.LookRotation(target);
        target.Normalize();

        Transform arw = Instantiate(arrowPrefab, spawnPoint.position, aimDir, this.transform).transform;
        arw.GetComponent<ProjectileData>().PlayerShot(this.GetComponent<PlayableCharacter>().attackModifier * damageMult * (1 + (0.5f * comboIndex)), enemy);
    }

    public void ShootNormal()
    {
        Shoot(arrow, 1);
    }

    private void BowAbility()
    {
        Shoot(abilityArrow, 500);
    }

    private Transform FindNearestEnemy()
    {
        foreach (Transform enemy in enemyContainer)
        {
            float dist = Vector3.Distance(this.transform.position, enemy.transform.position);
            Debug.Log(enemy.name + " " + dist);
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

        return closest;
    }
}
