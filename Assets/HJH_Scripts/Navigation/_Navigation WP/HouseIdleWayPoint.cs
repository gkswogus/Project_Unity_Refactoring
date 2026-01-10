using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HouseIdleWayPoint : MonoBehaviour
{
    
    int nextIndex = 0;
    NavMeshAgent agent;

    float damping;
    readonly float patrolSpeed = 3.0f;
    Transform tr;

    bool isPatrol; //순찰유무
    public bool ISPATROL
    {
        get { return isPatrol; }
        set
        {
            isPatrol = value;
            if (isPatrol)
            {
                agent.speed = patrolSpeed; 
                              // 3.0f
                damping = 1.0f; // 회전 속도
                MoveWayPoint();
            }
        }
    }

    public List<Transform> wayPoints = new(); // WayPoint 담을 List
    void Start()
    {
        tr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; //목적지에 다다르면 속도를 줄이는 옵션 비활성화
        agent.updateRotation = false; //자동으로 회전하는 기능 비활성화
        var group = GameObject.Find("WayPoint"); // WayPoint들을 담고 있는 부모 오브젝트
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints); 
               // 자식오브젝트 포함 Transform 정보를 wayPoints List에 추가
            wayPoints.RemoveAt(0); // 부모 오브젝트 빼기
            nextIndex = Random.Range(0, wayPoints.Count); // 병사들이 이동할 다음 위치
        }
        this.ISPATROL = true; // 순찰 상태 시작(ISPATROL의 set 실행)
    }
    void Update()
    {
        Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        // 오브젝트가 이동 방향을 향하도록 자연스럽게 회전시키는 기능

        if (!isPatrol) return;
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
                                        // 이동 중이고 && 목적지 근처에 도달한 경우
        {
            nextIndex = Random.Range(0, wayPoints.Count); // 다음 이동할 위치 선택
            MoveWayPoint(); 
        }
    }
    public void MoveWayPoint() // 다음 순찰 지점으로 이동
    {
        agent.speed = patrolSpeed; // 이동 속도 3.0f 
        agent.stoppingDistance = 0; // 해당 지점 까지 정확히 도달
        agent.destination = wayPoints[nextIndex].position; // 다음 목적지 지정
        agent.isStopped = false; // 멈추지 말고 이동
    }
 
}
