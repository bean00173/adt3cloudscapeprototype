using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyCombat : Combat
{
    public GameObject _potionBottle;
    public GameObject _forceField;

    // Start is called before the first frame update
    public override void Start()
    {
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
        base.Update();

        CastArc();
    }

    private void CastArc()
    {

    }

    public void ThrowPotion()
    {

    }

    private void AlchemyAbility()
    {
        GameObject field = Instantiate(_forceField, this.transform.position, Quaternion.identity);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(field, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
    }


}
