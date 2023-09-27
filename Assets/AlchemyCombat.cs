using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyCombat : Combat
{
    public GameObject _potionBottle;
    public GameObject _forceField;
    public Transform handPos;

    public GameObject fieldHealthUi;
    public LayerMask target;

    public GameObject pointer;

    Rigidbody bottleRb;

    private float throwStrength;
    private float upwardsForce;
    public float travelTime;

    LineRenderer lr;
    public int maxSteps;
    public float timeBetweenSteps;

    GameObject potion;

    RaycastHit hitInfo;

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
        if (Input.GetKey(KeyCode.Mouse1))
        {
            CastArc();
        }
        else
        {
            //lr.enabled = false;
            pointer.SetActive(false);
        }

        

        base.Update();
    }

    private void CastArc()
    {
        pointer.SetActive(true);

        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity, target);
        Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.magenta);

        pointer.transform.position = hitInfo.point;
        
    }

    private Vector3 CalculateVelocity(Vector3 target)
    {
        float distance = Vector3.Distance(new Vector3(this.transform.position.x, handPos.position.y, this.transform.position.z), target);
        float deltaVx = distance / travelTime;
        Vector3 velocityX = deltaVx * pm.orientation.forward;

        Vector3 velocityY = Mathf.Abs(Physics.gravity.y) * (travelTime / 2) * Vector3.up;

        return velocityX + velocityY;
    }

    public void ThrowPotion()
    {
        potion.GetComponent<Collider>().enabled = true;
        potion.transform.parent = null;
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(potion, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
        //potion.GetComponent<Rigidbody>().AddForce(CalculateVelocity(hitInfo.point), ForceMode.Impulse);
        potion.GetComponent<Rigidbody>().velocity = CalculateVelocity(hitInfo.point);

        if (comboIndex == 2) // if combo index already maxed out, reset index
        {
            potion.GetComponent<Potion>().StoreData(type.flame, pc.attackModifier, player, hitInfo.point, travelTime);
            comboIndex = 0;
            Debug.Log("Combo Finished");
        }
        else if (!secondAtk || comboIndex == 1) // if combo index not 2 or only on first attack, increase index by one
        {
            potion.GetComponent<Potion>().StoreData(type.explosive, pc.attackModifier, player, hitInfo.point, travelTime);
            comboIndex += 1;
        }

        pm.MoveInterrupt(true);
        readyToAtk = true;
    }

    public void CreatePotion()
    {
        
        potion = Instantiate(_potionBottle, handPos.position, Quaternion.identity, handPos.transform);
        potion.GetComponent<Collider>().enabled = false;
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
