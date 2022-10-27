using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text speed;
    public Text mass;
    public Text velocity;
    public Text damage;
    public Slider slider;
    public float maxHealth = 100;
    public float currentHealth = 0;

    Rigidbody rd;

    
    PrometeoCarController controller;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rd = GetComponent<Rigidbody>();
        controller =GetComponent<PrometeoCarController>();
        InvokeRepeating("CarSpeedUI", 0f, 0.1f);
    }
    public void DamageHealth(float damage, HealthBar hb)
    {
        // Adjust health
        hb.currentHealth -= damage;

        if (hb.currentHealth < 0) hb.currentHealth = 0;
        // Adjust healthbar.
        SetHealth(hb.currentHealth);
  
        // If health is 0 or less, run Die animation.
        if (hb.currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle Orc is dead.
    /// </summary>
    private void Die()
    {
        // Die action here.
     
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void CarSpeedUI()
    {

            try
            {
                float absoluteCarSpeed = Mathf.Abs(controller.carSpeed);
            float absoluteCartorque = Mathf.Abs(controller.rearLeftCollider.motorTorque);

            speed.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();

            mass.text = rd.mass.ToString();
            
            velocity.text = "( "+ Math.Round(rd.velocity.x, 2)  + "," + Math.Round(rd.velocity.y, 2) + "," + Math.Round(rd.velocity.z, 2) + " )";

        }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex);
            }


    }
    // Update is called once per frame
  
}
