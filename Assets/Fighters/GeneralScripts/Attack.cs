using System;
using UnityEngine;

public class FighterAttack : MonoBehaviour
{
    
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public Transform weaponHitBox; // Posici�n donde se verificar� el impacto de las armas
    public float attackRange; // Rango en el que se pueden detectar jugadores enemigos

    // Valores de da�o para diferentes ataques
    public float hitDamage = 10f;
    public float kickDamage;
    public float specialPowerDamage;
    
    public float hitDamageToShield; 
    public float kickDamageToShield;
    
    public float attackRate = 1f; // Tasa de ataque: n�mero de ataques por segundo permitidos
    public float waitingTimeHit; // Tiempo de espera entre golpes
    public float waitingTimeKick; // Tiempo de espera entre patadas
    private float nexAttackTime = 0f; // Acumulador del tiempo de espera para el pr�ximo ataque
    
    public KeyCode hitKey;
    public KeyCode kickKey;
    public KeyCode specialPowerKey;

    private SpecialAttack specialAttack;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundAttack1;

    void Start()
    {
        specialAttack = GetComponent<SpecialAttack>();
        animator = GetComponent<Animator>();
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update se llama una vez por cuadro
    void Update()
    {
        // Solo permite ataques si ha pasado suficiente tiempo desde el �ltimo ataque
        if (Time.time >= nexAttackTime)
        {
            // Si se presiona la tecla correspondiente, realiza un golpe
            if (Input.GetKeyDown(hitKey))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Si se presiona la tecla correspondiente, realiza una patada
            else if (Input.GetKeyDown(kickKey))
            {
                kick();
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Si se presiona la tecla correspondiente, activa el poder especial
            else if (Input.GetKeyDown(specialPowerKey))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // M�todo para realizar el golpe
    void hit()
    {
        animator.SetTrigger("Attack"); // Activa la animaci�n de ataque
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Aplica da�o a los enemigos detectados
    }

    // M�todo para realizar la patada
    private void kick()
    {
        // Activa la animaci�n de ataque
        animator.SetTrigger("Attack"); // DEBER�A SER DIFRENTE PARA LA ANIMACI�N DE KICK
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }

    // M�todo que aplica da�o a los enemigos detectados
    private void applyDamageToEnemies(float damage, float damageToShield)
    {
        // Detecta jugadores enemigos dentro del �rea del "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);

        // Aplica da�o a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            var health = playerEnemy.GetComponent<FighterHealth>();
            var shield = playerEnemy.GetComponent<FighterShield>();
            

            if (health != null)
            {
                if (shield == null || !shield.IsShieldActive())
                {
                    health.decreaselife(damage);
                    Debug.Log("We hit " + playerEnemy.name);
                    // Cargar barra de ataque especial con cada golpe acertado
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    shield.DecreaseShieldCapacity(damageToShield);
                }
            }
            
        }
    }

   
    // M�todo necesario para usar hijos del GameObject en el editor
    private void OnValidate()
    {
        if (weaponHitBox == null)
        {
            weaponHitBox = transform.Find("WeaponHitBox");
            if (weaponHitBox == null)
            {
                Debug.LogWarning("WeaponHitBox not found. Ensure there is a child GameObject named 'WeaponHitBox'.");
            }
        }
    }

    // Dibuja un Gizmo para visualizar el �rea de ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Color del Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // �rea circular del rango de ataque
    }

    public void setHitKey(KeyCode hitKey)
    {
        this.hitKey = hitKey;
    }

    public void setKickKey(KeyCode kickKey)
    {
        this.kickKey = kickKey;
    }

    public void setSpecialPowerKey(KeyCode specialPowerKey)
    {
        this.specialPowerKey = specialPowerKey;
    }

    public void setHitDamage(float hitDamageFromPersonaje)
    {
        hitDamage = hitDamageFromPersonaje;
    }

    public void setKickDamage(float kickDamageFromPersonaje)
    {
        kickDamage = kickDamageFromPersonaje;
    }

    public void setSpecialPowerDamage(float specialPowerDamageFromPersonaje)
    {
        specialPowerDamage = specialPowerDamageFromPersonaje;
    }

    public void setHitDamageToShield(float hitDamageToShieldFromPersonaje)
    {
        hitDamageToShield = hitDamageToShieldFromPersonaje;
    }

    public void setKickDamageToShield(float kickDamageToShieldFromPersonaje)
    {
        kickDamageToShield = kickDamageToShieldFromPersonaje;
    }

    public void setWaitingTimeHit(float waitingTimeHitFromPersonaje)
    {
        waitingTimeHit = waitingTimeHitFromPersonaje;
    }

    public void setWaitingTimeKick(float waitingTimeKickFromPersonaje)
    {
        waitingTimeKick = waitingTimeKickFromPersonaje;
    }

    public void setAttackRange(float attackRangeFromPersonaje)
    {
        attackRange = attackRangeFromPersonaje;
    }

    public void setAttackRate(float attackRateFromPersonaje)
    {
        attackRate = attackRateFromPersonaje;
    }
}
