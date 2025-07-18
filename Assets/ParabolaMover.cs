using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaMover : MonoBehaviour // 포물선 이동 할 수 있게 도와주는 코드
{
    public Transform target;       // 도착 지점
    public float height = 4f;      // 포물선 높이
    public float duration = 1f;    // 이동 시간

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

            // 포물선 보간
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            // 위로 솟는 y값 추가 (4 * height * t * (1 - t) 은 포물선 곡선 수식)
            currentPos.y += 4 * height * t * (1 - t);

            transform.position = currentPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 정확히 도착 지점 보정
        node.GetComponent<Node>().ChangeLine(); // 라인바꾸기
    }
}
