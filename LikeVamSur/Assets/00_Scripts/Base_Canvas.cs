using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance  = null;

    private void Awake()
    {
        if(instance == null) {instance = this;} 
    }

    private void Start()
    {
        EXPChange(0); //최초한번은 0으로 표시해 줘야지
        MANAGER.SESSION.onExpChanged += EXPChange;
    }

    private void OnDestroy()
    {
        MANAGER.SESSION.onExpChanged -= EXPChange;
    }

    public Image EXPFill;
    public TextMeshProUGUI LevelText;

    public void EXPChange(float exp)
    {
        float expPercentage = exp / 100.0f;
        EXPFill.fillAmount = expPercentage;
        LevelText.text =
            string.Format(
            "Lv.{0} <color=#FFFF00>{1:0.0}%</color>",
            (MANAGER.SESSION.Level + 1),
            exp);

    }
}
