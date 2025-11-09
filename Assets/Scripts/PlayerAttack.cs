using UnityEngine;
using UnityEngine.InputSystem; // For the new input system.
using System.Collections;
using NUnit.Framework; // To use IEnumerator (for attack delay coroutine).

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;  //Time between two attacks(seconds).
    [SerializeField] private float attackRange = 2f;     //How far the attack reach.
    [SerializeField] private float attackDelay = 0.4f;   //Delay before hit detection(sync with animatoin).
    [SerializeField] private Transform cam;              //Reference to player's camera for raycast direction.

    private PlayerControls controls;        //Input actions instance.
    private bool isAttacking = false;       //True while attacking.
    private float lastAttackTime = 0f;      //Time of the last attack.

    private void Awake()
    {
        controls = new PlayerControls();    //Create Input Actions class instance.
    }

    private void OnEnable()
    {
        controls.Enable();      //Enable input actions.
        controls.Player.Attack.performed += ctx => TryAttack();   //Listen  for attack input.
    }

    private void OnDisable()
    {
        controls.Disable();    //Disable input actions when script is not actibe.
    }

    private void TryAttack()
    {
        //If already attacking or still in cooldown -> stop here.
        if (isAttacking || Time.time - lastAttackTime < attackCooldown)
            return;

        //Otherwise start attack coroutine
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;     // Lock player actions during attack.
        lastAttackTime = Time.time;  //Remember when we attacked

        Debug.Log("Attack stared!");  // Log for debugging.

        // Wait for delay (simulate swing animation).
        yield return new WaitForSeconds(attackDelay);

        // After delay, check if something is hit 
        DetectHit();

        isAttacking = false; // Unlock player actions again.
    }

    private void DetectHit()
        {
            // Create a ray from camera position, facing forward
            Ray ray = new Ray(cam.position, cam.forward);

            // Cast a ray forward and check if it hits something
            if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
            {
                Debug.Log("Hit object: " + hit.collider.name); // Show the hit object name in console
                // Later: Add enemy damage system here (e.g., hit.collider.GetComponent<Enemy>().TakeDamage();)
            }
        }    


}