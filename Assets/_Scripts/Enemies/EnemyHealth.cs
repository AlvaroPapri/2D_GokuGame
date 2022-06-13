using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public TextMeshProUGUI textUI;
    public AnimationClip clipHit;
    public bool isDamaged;
    public bool isDeath;
    public float timeToShowText;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        textUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
            TakeDamage(collision.transform.parent.gameObject.GetComponent<PlayerMovement>().Damage());
    }

    private void TakeDamage(float dmgAmount)
    {
        if (isDeath)
            return;

        isDamaged = true;
        Invoke("DamagedToFalse", clipHit.length);

        currentHealth -= dmgAmount;
        
        StopCoroutine(TextDamageUI(dmgAmount));
        StartCoroutine(TextDamageUI(dmgAmount));

        if (currentHealth <= 0)
            Death();
        else 
            anim.SetTrigger("Hit");
    }

    private void Death()
    {
        isDeath = true;
        anim.SetTrigger("Death");
        Destroy(gameObject, 1);
    }

    IEnumerator TextDamageUI(float dmgReceived)
    {
        textUI.gameObject.SetActive(true);
        textUI.transform.localPosition = Vector3.zero;
        textUI.text = dmgReceived.ToString();
        yield return new WaitForSeconds(timeToShowText);
        textUI.gameObject.SetActive(false);
    }

    void DamagedToFalse() => isDamaged = false;
}
