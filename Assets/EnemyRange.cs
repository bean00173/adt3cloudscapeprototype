using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : EnemyBehaviour
{
    public GameObject arrowPrefab;

    public void DoRangeAtk() // BOW
    {
        if (atkReady)
        {
            atkReady = false;

            Vector3 aimTarget = goal.position;
            Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
            Quaternion aimDir = Quaternion.LookRotation(target);
            target.Normalize();
            Vector3 velocity = target * 60f;
            GameObject projectile = Instantiate(arrowPrefab, projectileSpawn.position, aimDir);
            projectile.GetComponent<ProjectileData>().ProjectileDamage(this.enemy.damage, velocity);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(projectile, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
            StartCoroutine(AtkCD());
        }
    }
}
