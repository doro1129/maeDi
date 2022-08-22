using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles the knife's swing and animation of knife
/// </summary>
public class Knife_Swing : MonoBehaviour
{
    /// <summary>
    /// The damage that will be given to enemy who is touched
    /// </summary>
    public float damage = 10.0f;    // TODO: If enemy is touched by weapon, damage should be given as much as this value.

    /// <summary>
    /// Check if the knife is swung and touched
    /// </summary>
    public bool is_Swung = false;
    public bool is_Touched = false;

    /// <summary>
    /// Attack cooldown time
    /// </summary>
    public float Attack_time = 0.5f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Swing();
    }

    /// <summary>
    /// If mouse0 is clicked, swinging animation which is named "Sword_Swing" will work
    /// If the knife is being swung, "is_Swung" returns true
    /// </summary>
    private void Swing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            is_Swung = true;
            anim.Play("Knife_Swing");
            Invoke(nameof(Reset_Swing), Attack_time);
        }

    }

    /// <summary>
    /// "is_Swung" and "is_Touched" will become false after the value of time definded at "Attack_Time"
    /// </summary>
    private void Reset_Swing()
    {
        is_Swung = false;
        is_Touched = false;
    }

    /// <summary>
    /// Objects which have tags named "enemy" will be destroyed if enemy's collider and knife's collider are touched each other
    /// </summary>
    /// <param name="enemy">enemy's collider</param>
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.tag.Equals("Enemy") && is_Swung)
        {
            is_Touched = true;
            Destroy(enemy.gameObject);      //TODO: This must be changed after adding the enemies's HP function (or destroying function)
        }
    }
}