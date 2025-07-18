using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaMover : MonoBehaviour // ������ �̵� �� �� �ְ� �����ִ� �ڵ�
{
    public Transform target;       // ���� ����
    public float height = 4f;      // ������ ����
    public float duration = 1f;    // �̵� �ð�

    public void StartParabolaMove(Vector3 targetPosition,GameObject node)
    {
        StopAllCoroutines();
        StartCoroutine(MoveInParabola(targetPosition,node));
    }

    IEnumerator MoveInParabola(Vector3 targetPos,GameObject node)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // ������ ����
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            // ���� �ڴ� y�� �߰� (4 * height * t * (1 - t) �� ������ � ����)
            currentPos.y += 4 * height * t * (1 - t);

            transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // ��Ȯ�� ���� ���� ����
        node.GetComponent<Node>().ChangeLine(); // ���ιٲٱ�
    }
}
