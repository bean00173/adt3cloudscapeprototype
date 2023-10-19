using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI towersBeaten, enemiesDefeated, gruntsDefeated, brutesDefeated, rangersDefeated, specialsDefeated, totalDeaths, totalScore;
    public GameObject win, lose;
    // Start is called before the first frame update
    void Start()
    {
        win.SetActive(GameManager.instance.victory);
        lose.SetActive(GameManager.instance.defeat);

        towersBeaten.text = GameManager.towersBeaten.ToString();
        enemiesDefeated.text = GameManager.instance.totalDefeated.ToString();
        gruntsDefeated.text = GameManager.instance.gruntsDefeated.ToString();
        brutesDefeated.text = GameManager.instance.brutesDefeated.ToString();
        rangersDefeated.text = GameManager.instance.rangersDefeated.ToString();
        specialsDefeated.text = GameManager.instance.specialsDefeated.ToString();
        totalDeaths.text = GameManager.instance.totalDeaths.ToString();
        totalScore.text = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
