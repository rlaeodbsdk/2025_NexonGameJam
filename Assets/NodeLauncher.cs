using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class NodeLauncher : MonoBehaviour
{
    public Transform cannonHead; // ���� �Ӹ� (ȸ�� ����)
    public GameObject notePrefab;
    public Transform noteSpawnPoint;

    public float rotationAngle = 80f;
    public float rotationSpeed = 2f;
    public float launchForce = 800f;

    private bool isFiring = false;
    private float currentAngle;
    private bool rotatingRight = true;

    private GameObject currentNote;
    private Queue<NodeRecipe> noteQueue = new Queue<NodeRecipe>(); //  ��⿭
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

            if (Input.GetKeyDown(KeyCode.Space)) // �ӽ� �߻�Ű
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

    public void SpawnNote(NodeRecipe recipe) // ����!
    {
       
        if (currentNote == null)
        {
            //  ������ ��Ʈ�� ������ �ٷ� ����
            currentRecipe = recipe;
            currentNote = Instantiate(notePrefab, noteSpawnPoint.position, noteSpawnPoint.rotation);
            currentNote.transform.SetParent(noteSpawnPoint);
            currentNote.GetComponentInChildren<SpriteRenderer>().sprite = recipe.steps[recipe.currentstepIndex].sprite;
            Managers.Sound.Play("SFX/cannonLoad1");
            var bullet = currentNote.GetComponent<Bullet>();
            bullet.getNodeRecipe(recipe);
        }
        else
        {
            //  �̹� ������ ������ ��⿭�� ����
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

        //  �߻�
        currentNote.transform.SetParent(null);
        if (Time.timeScale == 0f)
        {
            
            Vector3 fireDir = cannonHead.up;
            float fireDist = 5f;     
            float fireTime = 0.5f;   

            yield return StartCoroutine(ManualFireAnimation(currentNote, fireDir, fireDist, fireTime));
        }
        else
        {
            Rigidbody2D rb = currentNote.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(cannonHead.up * launchForce);
        }
            
        Managers.Sound.Play("SFX/cannonFire1");

        Bullet bullet = currentNote.GetComponent<Bullet>();

        bullet.getNodeRecipe(currentRecipe);

        StartCoroutine(bullet.DestroyDelay());

        //  ���� ��Ʈ Ŭ����
        currentNote = null;

        //  ��� ��� (�߻� ��Ÿ�� ����)
        yield return new WaitForSeconds(0.2f);

        //  ��⿭�� ��Ʈ�� �ִٸ� �ڵ� ����
        if (noteQueue.Count > 0)
        {
            NodeRecipe next = noteQueue.Dequeue();
            SpawnNote(next);
            Managers.Sound.Play("SFX/cannonLoad1");
        }

        isFiring = false;
    }
    IEnumerator ManualFireAnimation(GameObject note, Vector3 direction, float distance, float duration)
    {
        Vector3 startPos = note.transform.position;
        Vector3 targetPos = startPos + direction.normalized * distance;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            note.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        note.transform.position = targetPos;
    }
}
