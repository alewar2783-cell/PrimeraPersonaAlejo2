using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiveDamage : MonoBehaviour
{
    public enum EnemyType
    {
        Common,
        Heavy,
        Ranged
    }

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int health = 100;
    private int maxHealth;
    private int damage;
    private float speed;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    [SerializeField] private GameObject sangrePrefab;
    [SerializeField] private Transform puntoImpacto;

    [Header("Loot System")]
    [SerializeField] private GameObject normalCoinPrefab;
    [SerializeField] private GameObject specialCoinPrefab;

    void Awake()
    {
        InitializeStats();
    }

    void Start()
    {
        UpdateUI();
    }

    private void InitializeStats()
    {
        switch (enemyType)
        {
            case EnemyType.Common:
                maxHealth = 100;
                damage = 15;
                speed = 20f;
                break;
            case EnemyType.Heavy:
                maxHealth = 150;
                damage = 25;
                speed = 15f;
                break;
            case EnemyType.Ranged:
                maxHealth = 50;
                damage = 15;
                speed = 10f;
                break;
        }

        health = maxHealth;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            ApplyDamage(collision);
        }
    }

    private void ApplyDamage(Collision collision)
    {
        Debug.Log("He recibido un disparo!");
        int damageReceived = collision.gameObject.GetComponent<Damage>().damageAmount;
        health -= damageReceived;

        SpawnSangre(collision);

        UpdateUI();

        if (health <= 0)
        {
            Debug.Log("He muerto!");

            
            Mision.instancia?.EnemigoEliminado();

            DropLoot();

            Destroy(gameObject);
        }

        else
        {
            Debug.Log("Salud restante: " + health);
        }
    }

    private void DropLoot()
    {
        float dropChance = Random.Range(0f, 100f);

        if (dropChance < 50f) 
        {
            if (normalCoinPrefab != null) Instantiate(normalCoinPrefab, transform.position, Quaternion.identity);
        }
        else if (dropChance < 75f) 
        {
            if (specialCoinPrefab != null) Instantiate(specialCoinPrefab, transform.position, Quaternion.identity);
        }
        // Remaining 25% is no drop
    }

    private void SpawnSangre(Collision collision)
    {
        if (sangrePrefab != null)
        {

            Vector3 posicion = puntoImpacto != null
                ? puntoImpacto.position
                : collision.contacts[0].point;

            Quaternion rotacion = Quaternion.LookRotation(collision.contacts[0].normal);

            GameObject sangre = Instantiate(sangrePrefab, posicion, rotacion);


            Destroy(sangre, 2f);
        }
        else
        {
            Debug.LogWarning("No se asignó un prefab de sangre en el inspector.");
        }
    }

    private void UpdateUI()
    {
        healthText.text = "Health: " + health;
        healthBar.fillAmount = (float)health / maxHealth;
    }
}
