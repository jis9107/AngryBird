using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Handler : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer lineRenderer_2;
    
    private GameObject birdInstance;

    [SerializeField] private float maxDistnace;
    private float shootPower;
    
    private Rigidbody2D birdRigidbody;
    
    private Vector2 startPosition;
    private Vector2 startMousePos;
    private Vector2 currentMousePos;

    private Camera mainCamera;

    [Header("Projectile trajectory")] 
    private Transform trajectory_parent;
    private int maxStep = 5;
    private float timeStep = 0.05f;
    [SerializeField] private GameObject projectilePrefab;
    private List<GameObject> trajectoryPoints;
    
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        trajectory_parent = transform.GetChild(0);
        
        
        trajectoryPoints = new List<GameObject>();
        InitionalizePool();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.canShot)
        {
            MouseClick();
        }

        if (Input.GetMouseButton(0) && GameManager.instance.canShot)
        {
            LookAtShootDirection();
            UpdateTrajectory();
        }

        if (Input.GetMouseButtonUp(0) && GameManager.instance.canShot)
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(Shoot());
            lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
            lineRenderer_2.SetPosition(1, lineRenderer_2.GetPosition(0));
            HideTrajectory();
            
            if(birdInstance != null)
                Deceleration();
        }
    }

    private void MouseClick()
    {
        startPosition= transform.position;
        Vector2 dir = mainCamera.ScreenToWorldPoint(Input.mousePosition) - birdInstance.transform.position;
        birdInstance.transform.right = dir.normalized;
        startMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnSpawnBird(int index)
    {
        birdInstance = Instantiate(GameManager.instance.birdPrefab[index], transform.position, Quaternion.identity);
        birdRigidbody = birdInstance.GetComponent<Rigidbody2D>();
    }

    void InitionalizePool()
    {
        for (int i = 0; i < maxStep; i++)
        {
            GameObject go = Instantiate(projectilePrefab);
            go.transform.SetParent(trajectory_parent);
            go.SetActive(false);
            trajectoryPoints.Add(go);
        }
    }

    void HideTrajectory()
    {
        for (int i = 0; i < trajectoryPoints.Count; i++)
        {
            trajectoryPoints[i].SetActive(false);
        }
    }

    void UpdateTrajectory()
    {
        List<Vector2> position = GetTrajectoryPoints(transform.right * shootPower, birdRigidbody.mass);

        for (int i = 0; i < position.Count; i++)
        {
            trajectoryPoints[i].transform.position = position[i];
            trajectoryPoints[i].SetActive(true);
        }
    }

    List<Vector2> GetTrajectoryPoints(Vector2 force, float mass)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 position = transform.position;
        Vector2 velocity = force / mass;
        
        // maxStep으로 고정된 횟수만큼 반복
        for (int i = 0; i < maxStep; i++)
        {
            float timeElapsed = timeStep * (i+1);
            Vector2 nextPos = position + 
                              velocity * timeElapsed + 
                              Physics2D.gravity * (0.5f * timeElapsed * timeElapsed);
            
            points.Add(nextPos);
        }
        
        return points;
    }
    
    IEnumerator Shoot()
    {
        GameManager.instance.canShot = false;
        birdInstance.transform.position = Vector3.Lerp(birdInstance.transform.position, startPosition, 0.5f);
        birdRigidbody.gravityScale = 1;
        birdRigidbody.AddForce(transform.right * shootPower, ForceMode2D.Impulse);
        
        yield return null;
    }

    void LookAtShootDirection()
    {
        currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = startMousePos - currentMousePos;
        transform.right = dir;
        
        float distance = dir.magnitude;
        
        Vector3 LinePosition1 = Vector3.zero;
        Vector3 LinePosition2 = Vector3.zero;

        if (distance > maxDistnace)
        {
            dir = dir.normalized * maxDistnace;
            LinePosition1 = startPosition - dir;
            LinePosition2 = startPosition - dir;
            birdInstance.transform.position = LinePosition1;
            shootPower = maxDistnace * 25f;
        }

        else
        {
            Vector2 curPos = startPosition - dir.normalized * distance;
            LinePosition1 = curPos;
            LinePosition2 = curPos;
            birdInstance.transform.position = curPos;
            shootPower = distance * 25f;
        }
        lineRenderer.SetPosition(1, LinePosition1);
        lineRenderer_2.SetPosition(1, LinePosition2);
    }
    
    public void Deceleration()
    {
        StartCoroutine(DecelerationCoroutine());
    }
    
    IEnumerator DecelerationCoroutine()
    {
        while (birdRigidbody.velocity.magnitude > 0.1f)
        {
            birdRigidbody.velocity *= 0.9f;
            
            yield return new WaitForSeconds(0.1f);
        }

        if (birdInstance != null)
        {
            birdRigidbody.velocity = Vector2.zero;
            
            yield return new WaitForSeconds(2f);
            
            Destroy(birdInstance);
        }
    }
    
}
