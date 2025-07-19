using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class NodeLauncher : MonoBehaviour
{
    public Transform cannonHead; // 대포 머리 (회전 기준)
    public GameObject notePrefab;
    public Transform noteSpawnPoint;

    public float rotationAngle = 80f;
    public float rotationSpeed = 2f;
    public float launchForce = 800f;

    private bool isFiring = false;
    private float currentAngle;
    private bool rotatingRight = true;

    private GameObject currentNote;
    private Queue<NodeRecipe> noteQueue = new Queue<NodeRecipe>(); //  대기열
    private NodeRecipe currentRecipe;
    private NodeManager nodeManager;

    private void Start()
    {
        nodeManager = FindFirstObjectByType<NodeManager>();
    }
    void Update()
    {
        if (!isFiring)
        {
            RotateCannon();

            if (Input.GetKeyDown(KeyCode.Return)) // 임시 발사키
            {
                TryFire();
            }
        }
    }

    void RotateCannon()
    {
        float angle = rotationSpeed * Time.deltaTime;
        currentAngle += rotatingRight ? angle : -angle;

        if (Mathf.Abs(currentAngle) > rotationAngle)
        {
            rotatingRight = !rotatingRight;
        }

        cannonHead.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public void SpawnNote(NodeRecipe recipe) // 장전!
    {
        if (currentNote == null)
        {
            //  장전된 노트가 없으면 바로 장전
            currentRecipe = recipe;
            currentNote = Instantiate(notePrefab, noteSpawnPoint.position, noteSpawnPoint.rotation);
            currentNote.transform.SetParent(noteSpawnPoint);
            currentNote.GetComponentInChildren<SpriteRenderer>().sprite = recipe.steps[recipe.currentstepIndex].sprite;
        }
        else
        {
            //  이미 장전돼 있으면 대기열에 넣음
            noteQueue.Enqueue(recipe);
        }
    }

    void TryFire()
    {
        if (currentNote == null || isFiring)
            return;

        StartCoroutine(FireCoroutine());
        nodeManager.nodeCount--;
    }

    IEnumerator FireCoroutine()
    {
        isFiring = true;

        //  발사
        currentNote.transform.SetParent(null);
        Rigidbody2D rb = currentNote.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(cannonHead.up * launchForce);

        Bullet bullet = currentNote.GetComponent<Bullet>();
        StartCoroutine(bullet.DestroyDelay());

        //  현재 노트 클리어
        currentNote = null;

        //  잠깐 대기 (발사 쿨타임 느낌)
        yield return new WaitForSeconds(0.2f);

        //  대기열에 노트가 있다면 자동 장전
        if (noteQueue.Count > 0)
        {
            NodeRecipe next = noteQueue.Dequeue();
            SpawnNote(next);
        }

        isFiring = false;
    }
}
