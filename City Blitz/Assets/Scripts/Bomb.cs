using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bomb : MonoBehaviour
{
    public Sprite[] bombImages;
    private SpriteRenderer sr;  // For splatter particle effect
    private Rigidbody2D rb;  // For splatter particle effect

    public ParticleSystem DestroyEffect;

    public static event Action<Bomb> OnBombDeath;
    public static event Action<int> OnBombTargetHit;


    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
        this.rb = GetComponentInChildren<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
    }

    public void SetBombImage(int imageNum)
    {
        if (imageNum <= bombImages.Length)
        {
            sr.sprite = bombImages[imageNum];
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Target":
                BuildingBlock target = coll.gameObject.GetComponent<BuildingBlock>();
                target.TargetHit();
                OnBombTargetHit?.Invoke(1);
                break;
            case "Ground":
                // Hit the ground and explode
                OnBombDeath?.Invoke(this);
                SpawnDestroyEffect();
                Destroy(this.gameObject, 0.1f);
                break;
            default:
                break;
        }

        // Enable partical effect to splatter

        // Destroy this pooh
    }

    private void SpawnDestroyEffect()
    {
        Vector3 bombPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(bombPos.x, bombPos.y, bombPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }
}
