using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMove : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Transform target;
    private void OnEnable()
    {
        //위치 및 리지드바디 초기화
        transform.position = Vector2.zero;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        int x = Random.Range(0, 2) > 0 ? Random.Range(0, 80) : -Random.Range(0, 80);
        int y = Random.Range(0, 120);

        Vector2 dir = new Vector2(x, y);
        rigidbody.AddRelativeForce(dir);
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        yield return new WaitForSeconds(.5f);
        rigidbody.bodyType = RigidbodyType2D.Static;

        Vector3 dir = Vector3.zero;
        float distanceThisFrame = 0;

        while (dir.magnitude >= distanceThisFrame)
        {
            dir = target.position - transform.position;
            distanceThisFrame = 300f * Time.deltaTime;
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
