using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;


public class BuildingBlock : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public ParticleSystem DestroyEffect;


    //public static event Action<BuildingBlock> OnBrickDistruction;
    //public static event Action<BuildingBlock, int> OnBrickHit;

    public void TargetHit()
    {

    }

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCol = this.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        //BuildingManager.Instance.RemainingBlocks.Remove(this);
        //OnBrickDistruction?.Invoke(this);
        //OnBrickDestructionBuffs();
        SpawnDestroyEffect();
        Destroy(this.gameObject);
    }
    public void RemoveBlock()
    {
        Destroy(this.gameObject);

    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y - 1.0f, brickPos.z + 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    internal void Init(Transform transform, Sprite sprite, Color color, int hitPoints)
    {
        this.transform.SetParent(transform);
        this.sr.sprite = sprite;
        this.sr.color = color;
    }
}
