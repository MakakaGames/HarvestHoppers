using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Root : MonoBehaviour
{
    [HideInInspector] public int minSpawnChance;
    [HideInInspector] public int maxSpawnChance;
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float jumpDuration = 1;
    [SerializeField] private float rotDuration = 5;
    [SerializeField] private List<SpriteRenderer> renderers;

    public bool HasGrown => hasGrown;
    public static UnityAction<Root> OnRootAddedToPot;
    public static UnityAction<Root> OnRootRotten;

    public Vector3 spawnOffset;
    public Vector3 growScale;
    public float growDuration;
    public int chance;
    public RootType rootType;
    public float carryOffset = 0.5f;

    private int numOfJumps = 1;
    private bool hasGrown = false;
    private Coroutine rotCoroutine;
    private Vector3 pickedRootOffset;

    private void Start()
    {
        if (rootType != RootType.ObstacleRoot)
        {
            StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow()
    {
        transform.DOScale(growScale, growDuration)
            .OnComplete(() => transform.DOScale(growScale + new Vector3(0.075f, 0.075f, 0.075f), 0.175f).SetLoops(2, LoopType.Yoyo));

        yield return new WaitForSeconds(growDuration);

        hasGrown = true;
        rotCoroutine = StartCoroutine(Rot(rotDuration));
    }

    private IEnumerator Rot(float duration)
    {
        yield return new WaitForSeconds(duration);
        float alpha = 1;
        float startingRoat = duration;

        while (alpha > 0)
        {
            ChangeRendererAlpha(alpha);
            startingRoat -= Time.deltaTime;
            alpha = startingRoat / duration;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    public void JumpToPlayer(Player player)
    {
        transform.SetParent(player.transform, true);
        StopCoroutine(rotCoroutine);
        ChangeRendererAlpha(1);
        rotCoroutine = StartCoroutine(Rot(rotDuration / 3));
        var pickupRotation = player.transform.rotation == Quaternion.Euler(0, 0, 0) ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
        SetRenderersOrder(4);
        transform.DOJump(player.rootPickupAnchor.position, jumpPower, numOfJumps, jumpDuration)
            .Join(transform.DORotate(pickupRotation, jumpDuration))
            .OnComplete(() => player.SetState(Player.PlayerState.Carrying));
    }

    public IEnumerator JumpToPot(Vector3 targetPos, Character character)
    {
        StopCoroutine(rotCoroutine);
        ChangeRendererAlpha(0);
        transform.SetParent(character.transform, true);
        transform.position = character.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(1);
        ChangeRendererAlpha(1);
        transform.DOJump(targetPos, jumpPower, numOfJumps, jumpDuration).OnComplete(() => AddRootToPot());
    }

    private void AddRootToPot()
    {
        OnRootAddedToPot?.Invoke(this);
    }

    private void ChangeRendererAlpha(float value)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, value);
        }
    }

    private void SetRenderersOrder(int order)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sortingOrder = order;
        }
    }

    public static Root SpawnRootWithChance(List<Root> roots)
    {
        int chanceSum = 0;
        for (int i = 0; i < roots.Count; i++)
        {
            Root root = roots[i];
            chanceSum += root.chance;
            if (i == 0)
            {
                root.minSpawnChance = 0;
                root.maxSpawnChance = root.chance;
            }
            else
            {
                root.minSpawnChance = roots[i - 1].maxSpawnChance;
                root.maxSpawnChance = root.minSpawnChance + root.chance;
            }
        }

        int rand = Random.Range(0, chanceSum);

        for (int i = 0; i < roots.Count; i++)
        {
            Root root = roots[i];
            if (rand >= root.minSpawnChance && rand < root.maxSpawnChance)
            {
                return root;
            }
        }
        return null;
    }

    public enum RootType
    {
        Carrot,
        Potato,
        Buryak,
        ObstacleRoot
    }
}
