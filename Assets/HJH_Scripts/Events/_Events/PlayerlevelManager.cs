using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerlevelManager : MonoBehaviour
{
    [SerializeField]
    private STD PlayerSTD;

   /* [Header("Configuration")]
    [SerializeField] private int currntLevel;  
    [SerializeField] private int currntExp;
    [SerializeField] private int nextExp;
*/
   

    public GameObject levelupeffect;
    public TMP_Text LevelTe;
    Animator ani;
   /* public int ExpP;
   public int currentLevel; 
    public int currentExperience; */

    // GlobalConstants globalConstants;

    private void Awake()
    {

        // ExpP = PlayerSTD.NextExp;

        /*  currentLevel = currntLevel;

          currentExperience = currntExp; */
         ani = levelupeffect.GetComponent<Animator>();
    }

    private void Start()
    {

        //  PlayerSTD.level = currntLevel;
        //  PlayerSTD.exp = currntExp;
        //  PlayerSTD.nextExp = 100;
      /*   currntLevel = PlayerSTD.level;
         currntExp = PlayerSTD.exp;
         nextExp = PlayerSTD.nextExp;*/

        GameEventManager.instance.playerEvents.onExperienceGained += ExperienceGained;
        GameEventManager.instance.playerEvents.PlayerLevelChange(PlayerSTD.level);
        GameEventManager.instance.playerEvents.PlayerExperienceChange(PlayerSTD.exp);


        


        levelupeffect.SetActive(false);

        // currentLevel = currntLevel;
        //  currentExperience = currntExp;
    }
    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        GameEventManager.instance.playerEvents.onExperienceGained -= ExperienceGained;
        
       
    }

 
    IEnumerator LevelupEffect()
    {     
        levelupeffect.SetActive(true);
        ani.SetTrigger("UP");
        LevelTe.text = string.Format("         {0}", PlayerSTD.level);
        SoundManager.instance.Play(UISOUND.LevelUp);
        yield return new WaitForSeconds(3.5f);
        levelupeffect.SetActive(false);
    }
    private void ExperienceGained(int experience)
    {
        PlayerSTD.exp += experience;  //현재 경험치에 획득 경험치를 더해줌

        while (PlayerSTD.exp >= PlayerSTD.nextExp) // 다음 레벨을 향한 경험치를 체웠다면
        {
            PlayerSTD.exp -= PlayerSTD.nextExp;
            PlayerSTD.level++;
            PlayerSTD.abilityPoint += 5;
            GameEventManager.instance.playerEvents.PlayerLevelChange(PlayerSTD.level); 
            StartCoroutine(LevelupEffect());

            SetNextExp(); // 다음 레벨을 향한 경험치 수치 업데이트
        }
       GameEventManager.instance.playerEvents.PlayerExperienceChange(PlayerSTD.exp);
    }

    private void SetNextExp()
    {
        switch (PlayerSTD.level) //레벨 별 경험치
        {
            case 1:
                PlayerSTD.nextExp = 100;

                break;
            case 2:
                PlayerSTD.nextExp = 125;

                break;
            case 3:
                PlayerSTD.nextExp = 150;

                break;
            case 4:
                PlayerSTD.nextExp = 200;
                break;

            default:
                PlayerSTD.nextExp = 500;
                break;
        }
    }
   
    private void Update()
    {
     
       /* switch (PlayerSTD.level) //레벨 별 경험치
        {
            case 1:
                PlayerSTD.nextExp = 100;

                break;
            case 2:
                PlayerSTD.nextExp = 125;

                break;
            case 3:
                PlayerSTD.nextExp = 150;

                break;
            case 4:
                PlayerSTD.nextExp = 200;
                break;

            default:
                PlayerSTD.nextExp = 500;
                break;
        }*/
    }
}

