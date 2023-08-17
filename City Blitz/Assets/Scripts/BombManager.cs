using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{

    #region Singleton
    private static BombManager _instance;

    public static BombManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    [SerializeField]
    public Bomb[] bombPrefab;

    private float BOMB_DROP_SPEED = -50.0f;//-0.5f;

    private bool bombActive = false;

    // Start is called before the first frame update

    private void Start()
    {
    }

    public void SpawnBomb(Vector3 position, float x_speed)
    {
        int rand = UnityEngine.Random.Range(0, bombPrefab.Length);
        if (bombActive == false)
        {
            if (bombPrefab[rand] != null)
            {
                float rotation = UnityEngine.Random.Range(-0.06f, 0.06f);
                Bomb spawnedPooh = Instantiate(bombPrefab[rand], position, Quaternion.identity);
                Rigidbody2D spawnedPoohRb = spawnedPooh.GetComponent<Rigidbody2D>();
                spawnedPoohRb.isKinematic = false;
                spawnedPoohRb.angularVelocity = 1.0f;
                spawnedPoohRb.inertia = 0.0f;
                spawnedPoohRb.AddForce(new Vector2(x_speed, BOMB_DROP_SPEED));
                //spawnedPoohRb.AddTorque(rotation);
                bombActive = true;
                Bomb.OnBombDeath += OnBombDeath;

            }
            else
            {
                Debug.Log("SpawnPooh - poohPrefab is NULL!!");
            }
        }
    }
    private void OnBombDeath(Bomb obj)
    {
        bombActive = false;
    }

    private void OnDestroy()
    {
        Bomb.OnBombDeath -= OnBombDeath;

    }
}
