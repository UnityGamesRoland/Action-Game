using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NavPoint
{
	public Vector3 point;
	[Range(0, 360)] public float angle;
	[Range(0, 10)] public float waitTime = 2f;
}

public class EnemyAI : MonoBehaviour
{
	public float enemyHealth = 100;
	public float walkSpeed = 3.5f;
	public float runSpeed = 5.65f;
	public int damage = 100;

	public float timeToSpotPlayer = 0.5f;
	[Range(0, 180)] public float lookAngle = 60f;

	public Transform backpackPrefab;
	public NavPoint[] waypoints;

	private float dissolveAmount = 0f;
	private float playerVisibleTimer;

	private bool isDead;
	private bool isWaiting;
	public bool isAggroed;

	private int waypointIndex;
	private NavMeshAgent agent;
	private Animation anim;
	private Renderer model;
	private Transform player;
	private LayerMask targetMask;

	private void Start()
	{
		player = FindObjectOfType<PlayerMotorBehavior>().transform;
		targetMask = player.gameObject.layer;

		model = transform.GetChild(2).GetComponent<Renderer>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animation>();
	}

	private void Update()
	{
		if(isDead)
		{
			dissolveAmount = Mathf.Lerp(dissolveAmount, 1, Time.deltaTime * 5);
			model.material.SetFloat("_DissolveAmount", dissolveAmount);
			return;
		}

		LookForPlayer();

		if(!isAggroed)
		{
			agent.speed = walkSpeed;

			if(isWaiting)
			{
				Quaternion targetRotation = Quaternion.Euler(0, waypoints[waypointIndex].angle, 0);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3);
			}

			if(!agent.hasPath && !isWaiting) StartCoroutine(PatrolToNextPoint());
		}

		else if(isAggroed)
		{
			agent.speed = runSpeed;
			agent.SetDestination(player.position);

			if(agent.remainingDistance <= 0.5f)
			{
				player.SendMessage("TakeDamage", damage);
				ApplyDamage(1000);
			}
		}

		HandleAnimation();
	}

	public void ApplyDamage(int amount)
	{
		if(isDead) return;

		enemyHealth -= amount;

		if(enemyHealth <= 0)
		{
			isDead = true;
			agent.speed = 0;
			anim.CrossFade("death");

			Destroy(gameObject, 1.2f);

			Vector3 randomFactor = Random.insideUnitSphere;
			Vector3 randomPos = new Vector3(transform.position.x + randomFactor.x, transform.position.y, transform.position.z + randomFactor.z);

			Instantiate(backpackPrefab, randomPos, Quaternion.identity);
		}
	}

	private void LookForPlayer()
	{
		if(CanSeePlayer()) playerVisibleTimer += Time.deltaTime;
		else playerVisibleTimer -= Time.deltaTime;

		playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);

		if(playerVisibleTimer >= timeToSpotPlayer)
		{
			isAggroed = true;
		}
	}

	private void HandleAnimation()
	{
		if(isAggroed)
		{
			anim.CrossFade("run");
			return;
		}

		if(agent.velocity.magnitude <= 0.2f) anim.CrossFade("idle");
		else anim.CrossFade("walk");
	}

	private IEnumerator PatrolToNextPoint()
	{
		isWaiting = true;
		agent.updateRotation = false;

		yield return new WaitForSeconds(waypoints[waypointIndex].waitTime);

		isWaiting = false;
		agent.updateRotation = true;

		waypointIndex++;
		if(waypointIndex == waypoints.Length) waypointIndex = 0;

		agent.SetDestination(waypoints[waypointIndex].point);
	}

	private bool CanSeePlayer()
	{
		if(Vector3.Distance(transform.position, player.position) < 10)
		{
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

			if(angleBetweenGuardAndPlayer < lookAngle / 2f)
			{
				if(!Physics.Linecast (transform.position, player.position, targetMask)) return true;
			}
		}

		return false;
	}

	private void OnDrawGizmos()
	{
		if(waypoints.Length < 2) return;

		Vector3 startPosition = waypoints[0].point;
		Vector3 previousPosition = startPosition;

		foreach (NavPoint waypoint in waypoints)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (waypoint.point, 0.25f);

			Gizmos.color = Color.blue;
			Gizmos.DrawRay(new Vector3(waypoint.point.x +0.02f, waypoint.point.y, waypoint.point.z), (Quaternion.AngleAxis(waypoint.angle, Vector3.up)) * Vector3.forward);
			Gizmos.DrawRay(waypoint.point, (Quaternion.AngleAxis(waypoint.angle, Vector3.up)) * Vector3.forward);
			Gizmos.DrawRay(new Vector3(waypoint.point.x -0.02f, waypoint.point.y, waypoint.point.z), (Quaternion.AngleAxis(waypoint.angle, Vector3.up)) * Vector3.forward);

			Gizmos.color = Color.white;
			Gizmos.DrawLine (previousPosition, waypoint.point);
			previousPosition = waypoint.point;
		}

		Gizmos.DrawLine (previousPosition, startPosition);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, (Quaternion.AngleAxis(-lookAngle /2, Vector3.up)) * transform.forward * 10);
		Gizmos.DrawRay(transform.position, (Quaternion.AngleAxis(lookAngle /2, Vector3.up)) * transform.forward * 10);
	}
}