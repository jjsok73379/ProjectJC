using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float speed; // �ؽ�Ʈ �̵��ӵ�
    public float alphaSpeed; // ���� ��ȯ�ӵ�
    public float destroyTime;
    TextMeshPro text;
    Color alpha;
    public int damage;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        alpha.a=Mathf.Lerp(alpha.a,0, Time.deltaTime*alphaSpeed);
        text.color = alpha;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
