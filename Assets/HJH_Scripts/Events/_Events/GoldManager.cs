using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
   /* [Header("Configuration")]
    [SerializeField] private int startingGold = 50000;*/

    public int currentGold { get; private set; }

    STD PlayerSTD;

    private void Awake()
    {
        PlayerSTD = GameObject.FindWithTag("Player").GetComponent<STD>();
    }

    private void Start()
    {
        GameEventManager.instance.goldEvents.onGoldGained += GoldGained;
        GameEventManager.instance.goldEvents.GoldChange(PlayerSTD.money);
       // PlayerInfo1.money = startingGold;
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        GameEventManager.instance.goldEvents.onGoldGained -= GoldGained;
    }

   

    private void GoldGained(int gold)
    {
        PlayerSTD.money += gold;
        GameEventManager.instance.goldEvents.GoldChange(PlayerSTD.money);
    }
   
}
