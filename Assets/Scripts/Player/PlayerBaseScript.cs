using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBaseScript : MonoBehaviour
{
    [SerializeField]
    private int health = 1000;
    [SerializeField]
    private int resources = 100;
    [SerializeField]
    private int currentTower = 0;

    private float resourseCoolDown = 10f;
    private float timerCoolDown;
    private int resourcesIncrease = 1;

    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerResourcesText;
    public GameObject gameOverField;
    public GameObject cursor;
    //public GameObject towerTemplate;
    public TowerListScript towerListManager;

    // Update is called once per frame
    void Update()
    {
        playerHealthText.text = "Health: " + health.ToString();
        playerResourcesText.text = "Money: " + resources.ToString();
        if (timerCoolDown <= 0f)
        {
            timerCoolDown = resourseCoolDown;
            resources += resourcesIncrease;
        }
        else
        {
            timerCoolDown -= Time.deltaTime;
        }
        if (health <= 0)
        {
            Time.timeScale = 0.0f;
            gameOverField.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && cursor.activeSelf)
        {
            BuyTower(cursor.transform.position);
            cursor.SetActive(false);
        }
        if (Input.GetMouseButtonDown(1) && cursor.activeSelf)
        {
            cursor.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Player 1 Base taking damage");

        //if (collision.gameObject.GetComponent<ProjectileManager>() != null)
        //{
        //    health -= collision.gameObject.GetComponent<ProjectileManager>().self.damage;
        //    Destroy(collision.gameObject);
        //}


        ProjectileManager pm = collision.gameObject.GetComponent<ProjectileManager>();

        if (pm != null && pm.CompareTag("p2"))
        {
            health -= pm.self.damage;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player 1 Base taking damage");

        if (collision.gameObject.GetComponent<ProjectileManager>() != null)
        {
            health -= collision.gameObject.GetComponent<ProjectileManager>().self.damage;
            Destroy(collision.gameObject);
        }
    }

    public void BuyTower(Vector3 location)
    {
        if (towerListManager.towerList[currentTower].cost <= resources)
        {
            resources -= towerListManager.towerList[currentTower].cost;
            // Spawn the units using selection, make sure to add cost
            //GameObject tower = Instantiate(towerTemplate, location, Quaternion.identity);
            GameObject tower = CreateTower(location, towerListManager.towerList[currentTower]);
            //tower.GetComponent<TowerManager>().self = towerListManager.towerList[currentTower];
        }
    }

    private GameObject CreateTower(Vector3 location, TowerType towerType)
    {
        GameObject tower = new GameObject();
        //tower.transform.parent = this.transform;
        tower.transform.position = location;

        tower.AddComponent<Rigidbody2D>();

        tower.AddComponent<PolygonCollider2D>();
        tower.GetComponent<PolygonCollider2D>().isTrigger = true;

        tower.AddComponent<SpriteRenderer>();
        tower.GetComponent<SpriteRenderer>().sortingOrder = 30;

        tower.AddComponent<TowerManager>();
        tower.GetComponent<TowerManager>().self = towerType;
        tower.GetComponent<TowerManager>().currentTarget = GameObject.FindGameObjectWithTag("Player2");
        tower.GetComponent<TowerManager>().baseTower = this;

        return tower;
    }

    public void SelectTowerToBuy(int currentSelection)
    {
        currentTower = currentSelection;
    }

    public void AddResourcesToPlayer(int amount)
    {
        resources += amount;
    }
}
