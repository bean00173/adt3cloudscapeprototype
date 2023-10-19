using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : EnemyBehaviour
{
    public GameObject arrowPrefab;
    public Transform projectileSpawn;

    public void DoRangeAtk() // BOW
    {
        //if (atkReady)
        //{
        //    Vector3 aimTarget = goal.position;

        //    float y = goal.transform.GetComponent<CapsuleCollider>() ? aimTarget.y + (goal.transform.GetComponent<CapsuleCollider>().height / 2) : 1f;
        //    Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
        //    Debug.DrawLine(this.transform.position, target, Color.green);
        //    Quaternion aimDir = Quaternion.LookRotation(target);
        //    target.Normalize();
        //    Vector3 velocity = target * 60f;
        //    GameObject projectile = Instantiate(arrowPrefab, projectileSpawn.position, aimDir);
        //    projectile.GetComponent<ProjectileData>().ProjectileDamage(this.enemy.damage, velocity);
        //    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(projectile, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
        //    StartCoroutine(AtkCD());
        //}

        Vector3 aimTarget = goal.position;

        float y = goal.transform.GetComponent<CapsuleCollider>() ? aimTarget.y + (goal.transform.GetComponent<CapsuleCollider>().height / 2) : 1f;
        Vector3 target = new Vector3(aimTarget.x - projectileSpawn.position.x, 0f, aimTarget.z - projectileSpawn.position.z);
        Quaternion aimDir = Quaternion.LookRotation(target);
        target.Normalize();
        Vector3 velocity = target * 60f;
        GameObject projectile = Instantiate(arrowPrefab, projectileSpawn.position, aimDir);
        projectile.GetComponent<ProjectileData>().ProjectileDamage(this.enemy.damage, velocity);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(projectile, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
        StartCoroutine(AtkCD());
    }
}
