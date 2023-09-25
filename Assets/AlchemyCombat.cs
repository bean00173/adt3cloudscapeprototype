using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyCombat : Combat
{
    public GameObject _potionBottle;
    public GameObject _forceField;
    public Transform handPos;

    public GameObject fieldHealthUi;


    Rigidbody bottleRb;

    public float throwStrength;

    LineRenderer lr;
    public int maxSteps;
    public float timeBetweenSteps;

    GameObject potion;

    // Start is called before the first frame update
    public override void Start()
    {
        lr = handPos.GetComponent<LineRenderer>();
        bottleRb = _potionBottle.GetComponent<Rigidbody>();

        if (GameManager.instance.currentScene.name == "TowerTest")
        {
            this.enabled = false;
        }

        base.Start();

        abilityMethod = AlchemyAbility;
    }

    // Update is called once per frame
    public override void Update()
    {
        CastArc();

        base.Update();
    }

    private void CastArc()
    {
        lr.enabled = true;
        lr.positionCount = Mathf.CeilToInt(maxSteps / timeBetweenSteps) + 1;

        Vector3 startPosition = handPos.position;
        Vector3 startVelocity = throwStrength * pm.orientation.forward/ bottleRb.mass;

        int i = 0;
        lr.SetPosition(i, startPosition);

        for (float time = 0; time < maxSteps; time += timeBetweenSteps)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (-Physics.gravity.y * .5f * Mathf.Pow(time, 2));

            lr.SetPosition(i, point);
        }
    }

    public void ThrowPotion()
    {
        potion.transform.parent = null;
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(potion, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
        potion.GetComponent<Rigidbody>().AddForce(pm.orientation.forward * throwStrength, ForceMode.Impulse);

        if (comboIndex == 2) // if combo index already maxed out, reset index
        {
            comboIndex = 0;
            Debug.Log("Combo Finished");
        }
        else if (!secondAtk || comboIndex == 1) // if combo index not 2 or only on first attack, increase index by one
        {
            comboIndex += 1;
        }

        pm.MoveInterrupt(true);
        readyToAtk = true;
    }

    public void CreatePotion()
    {
        
        potion = Instantiate(_potionBottle, handPos.position, Quaternion.identity, handPos.transform);
        potion.transform.localPosition = Vector3.zero;
    }

    private void AlchemyAbility()
    {
        fieldHealthUi.SetActive(true);
        GameObject field = Instantiate(_forceField, this.transform.position, Quaternion.identity);
        field.GetComponent<ForceField>().healthImg = this.fieldHealthUi;
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(field, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));

        pm.MoveInterrupt(true);
        readyToAtk = true;
    }


}
