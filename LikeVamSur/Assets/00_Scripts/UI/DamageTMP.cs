using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTMP : MonoBehaviour
{
    GameObject Critical;
    private TextMeshProUGUI m_Text;
    private RectTransform rectTransform;

    private Vector2 velocity;       //초기 속도 (포물선 운동)
    private float gravity = -500f;  // 중력 효과(UI 이동이므로 값 조정 필요)
    private float lifetime = 1f;  // 지속 시간
    private Color textColor;


 
    private void Awake()
    {
        Critical = transform.Find("Critical").gameObject;
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }


    public void Initialize(Transform parent, Vector3 pos, string temp, bool critical = false)
    {
        Critical.SetActive(critical);
        transform.SetParent(parent);
        m_Text.text = temp; 

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        rectTransform.position = screenPosition;

        velocity = new Vector2(Random.Range(-50f, 50f), Random.Range(150f, 200f));
        textColor = m_Text.color;

        StartCoroutine(MoveAndFade());

    }

    IEnumerator MoveAndFade()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < lifetime)
        {
            velocity.y += gravity * Time.deltaTime;
            rectTransform.anchoredPosition += velocity * Time.deltaTime;
            textColor.a = Mathf.Lerp(1.0f, 0.0f, elapsedTime/lifetime);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        //Destroy(gameObject);

        MANAGER.POOL.m_pool_Dictionary["DamageFont"].Return(this.gameObject);
    }
   
}
