using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defence Tower
/// </summary>
public class Tower : MonoBehaviour
{
    /// <summary>
    /// Tower data (scriptable object)
    /// </summary>
    public TowerData data;
    /// <summary>
    /// Tower collider that detects enemies
    /// </summary>
    public TowerCollider towerCollider;
    /// <summary>
    /// A weapon game object in a tower that tracks a target or fires a weapon
    /// </summary>
    public GameObject towerWeapon;
    /// <summary>
    /// Map of the game
    /// </summary>
    public GameMap gameMap;
    /// <summary>
    /// Whether the tower can be moved
    /// </summary>
    public bool isMovable = false;
    
    private int _tileLayerMask;
    private float _attackTimestamp = 0;

    private void Awake()
    {
        // Hide collider of the tower
        towerCollider.IsVisible = false;
        
        // Get layer mask for raycast
        _tileLayerMask = LayerMask.GetMask("Tile");
    }

    private void Update()
    {
        if (isMovable)
        {
            Movement();
        }
        
        if (!towerCollider.IsEmpty())
        {
            GameObject enemy = GetEnemy();
            WeaponRotate(enemy);

            if (_attackTimestamp >= data.attackSpeed)
            {
                AttackEnemy(enemy);
                _attackTimestamp = 0;
            }
        }
        else
        {
            WeaponReset();
        }

        _attackTimestamp += Time.deltaTime;
    }

    /// <summary>
    /// Get the closest GameObject among the enemies in the collider.
    /// </summary>
    /// <returns>Enemy object</returns>
    private GameObject GetEnemy()
    {
        List<GameObject> enemies = towerCollider.Enemies;
        GameObject enemy = enemies[0];
        float minDistance = float.MaxValue;

        for (int index = 0; index < enemies.Count; index ++)
        {
            float distance = Vector3.Distance(transform.position, enemies[index].transform.position);

            if (distance < minDistance)
            {
                enemy = enemies[index];
                minDistance = distance;
            }
        }

        return enemy;
    }

    /// <summary>
    /// Weapon rotation reset
    /// </summary>
    private void WeaponReset()
    {
        towerWeapon.transform.up = Vector3.back;
    }

    /// <summary>
    /// Make the weapon look at the enemy.
    /// </summary>
    /// <param name="enemy">Enemy GameObject</param>
    private void WeaponRotate(GameObject enemy)
    {
        towerWeapon.transform.LookAt(enemy.transform);
        towerWeapon.transform.Rotate(new Vector3(1, 0, 0), 90.0f);
        towerWeapon.transform.Rotate(new Vector3(0, 1, 0), 180.0f);
    }

    /// <summary>
    /// Creates a attack object towards the enemy.
    /// </summary>
    /// <param name="enemy">Enemy GameObject</param>
    private void AttackEnemy(GameObject enemy)
    {
        Vector3 position = towerWeapon.transform.position;
        
        GameObject bullet = Instantiate(data.attackObject, position, Quaternion.identity);
        bullet.transform.forward = position - enemy.transform.position;
        bullet.GetComponent<TowerAttackObject>().direction = Vector3.back;
    }

    /// <summary>
    /// Move the tower on the tower tile with the mouse or touch position
    /// (touch position has higher priority than mouse position)
    /// </summary>
    private void Movement()
    {
        Vector3 position = Input.mousePosition;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            position = touch.position;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(position);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _tileLayerMask);

        if (hit.transform != null)
        {
            GameObject hitObject = hit.transform.gameObject;
            
            if (GameMap.GetTileType(hitObject.tag) == GameMap.TileType.Tower)
            {
                transform.position = hitObject.transform.position + gameMap.TileObjectOffset;
            }
        }
    }
}
