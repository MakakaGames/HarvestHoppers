using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Pot : MonoBehaviour
{
    [SerializeField] private Transform rootTargetTransform;

    public Transform RootTargetTransform => rootTargetTransform;
    public AudioSource BulkSound;

    private void Start()
    {
        Root.OnRootAddedToPot += DestroyRootOnAdd;
    }

    private void DestroyRootOnAdd(Root root)
    {
        DOTween.Kill(root);
        Destroy(root.gameObject);
        BulkSound.Play();
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
        Root.OnRootAddedToPot -= DestroyRootOnAdd;
    }
}

