
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance  = null;

    public SkillFrame frame;
    public Transform activeFrameContent;
    public Transform passiveFrameContent;
    List<GameObject> SkillFrameGorvage = new List<GameObject>();

    private void Awake()
    {
        if(instance == null) {instance = this;} 
    }

    private void Start()
    {
        EXPChange(0); //최초한번은 0으로 표시해 줘야지
        
        MANAGER.SESSION.onExpChanged += EXPChange;
        MANAGER.SESSION.onHpChanged += HPChanged;
        MANAGER.SESSION.onMonsterCountChanged += M_CountText;

        MANAGER.SESSION.onSelectedCard += SetSkillFrame;


        SelectCard(true);
    }

    private void OnDestroy()
    {
        MANAGER.SESSION.onExpChanged -= EXPChange;
        MANAGER.SESSION.onMonsterCountChanged -= M_CountText;
    }

    public Transform HOLDERLAYER; 

    public Image EXPFill;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI TimerText;

    Coroutine HP_Coroutine;

    public Image HpFill;
    public Image HPFillSeconds;
    public TextMeshProUGUI HPText;


    //public GameObject CardObject;
    public CardSelector CardObject;
    private void Update()
    {
        TimerText.text = Utils_UI.FormatTime(MANAGER.SESSION.GameTime);


    }

    public void SelectCard(bool AllActive = false)
    {
        Time.timeScale = 0;
        //CardObject.SetActive(true);
        CardObject.Initialize(AllActive);
    }

    private void M_CountText(int value) => monsterCountText.text = value.ToString();  
    public void EXPChange(float exp)
    {
        //float expPercentage = exp / 100.0f;
        float expPercentage = exp / MANAGER.SESSION.GetRequiredExp();   
        
        EXPFill.fillAmount = expPercentage;
        LevelText.text =
            string.Format(
            "Lv.{0} <color=#FFFF00>{1:0.0}%</color>",
            (MANAGER.SESSION.Level + 1),
            //exp
            expPercentage * 100.0f
            );

    }

    public void HPChanged(float hp)
    {
        float hpPercentage = hp / MANAGER.SESSION.MaxHP;
        HPText.text = string.Format("{0:0}/{1:0}", hp, MANAGER.SESSION.MaxHP);
        HpFill.fillAmount = hpPercentage;
        
        if(HP_Coroutine != null)
        {
            StopCoroutine(HP_Coroutine);
        }

        HP_Coroutine = StartCoroutine(ScondFillAmount(HpFill.fillAmount));
    }

    IEnumerator ScondFillAmount(float percentage)
    {
        float speed = 2f;
        float t = 0f;
        while(HPFillSeconds.fillAmount > percentage)
        {
            t += Time.deltaTime*speed;
            HPFillSeconds.fillAmount = Mathf.Lerp(
                                        HPFillSeconds.fillAmount,
                                        percentage, t);

            yield return null;
        }
        HPFillSeconds.fillAmount = percentage;
    }

    public void SetSkillFrame()
    {
        if(SkillFrameGorvage.Count > 0)
        {
            for(int i = 0;i<SkillFrameGorvage.Count; i++)
            {
                Destroy(SkillFrameGorvage[i]);
            }

            SkillFrameGorvage.Clear();
        }

        foreach(var data in MANAGER.SESSION.SelectedCards)
        {
            var go = Instantiate(frame,
                data.Value.db.state == CardState.Active ?
                activeFrameContent :
                passiveFrameContent);

            go.Initialize(data.Value);
            SkillFrameGorvage.Add(go.gameObject);
        }
    }


}
