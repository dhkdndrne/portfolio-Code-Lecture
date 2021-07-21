using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float orthographSize;
    private float targetOrthographSize;

    private void Start()
    {
        //�ó׸ӽ� ������ orthograpghSize�� ����
        orthographSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographSize = orthographSize;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //���⺤�� ���ϱ�
        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthographSize += -Input.mouseScrollDelta.y * zoomAmount;  //targetOrthographSize�� ���콺 �� * �ܾ�

        float minOrthographicSize = 10; //�ּ� ũ��
        float maxOrthographicSize = 30; //�ִ� ũ��

        // targetOrthographSize�� ����� �ּҿ� �ִ� ���̰����� ����
        targetOrthographSize = Mathf.Clamp(targetOrthographSize, minOrthographicSize, maxOrthographicSize);


        //�� �Ǵ� �ӵ� �ε巴��
        float zoomSpeed = 5f;
        orthographSize = Mathf.Lerp(orthographSize, targetOrthographSize, zoomSpeed * Time.deltaTime);

        //�ó׸ӽ� ������ orthographSize �� ����
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographSize;
    }
}
