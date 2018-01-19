using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAI : MonoBehaviour
{
	public float enemyHealth = 100;
	public float runSpeed = 5.65f;
	public int damage = 100;
	public float attackSpeed = 1.1f;
	public bool countKill;
	public Transform upperBody;
	public Gradient hitFeedbackGradient;

	private float attackTimer = 0f;
	private float dissolveAmount = 0f;
	private float gradientBlendAmount = 0f;
	private bool isDead;

	private NavMeshAgent agent;
	private Animation anim;
	private Renderer model;
	private Transform player;
	private AudioSource source;

	private void Start()
	{
		player = FindObjectOfType<PlayerMotorBehavior>().transform;
		model = transform.GetChild(2).GetComponent<Renderer>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animation>();

		model.material.SetFloat("_DissolveAmount", 1);
		dissolveAmount = 1;

		anim["punch"].layer = 1;
		anim["punch"].AddMixingTransform(upperBody);
	}

	private void Update()
	{
		dissolveAmount = Mathf.Lerp(dissolveAmount, isDead ? 1 : 0, Time.deltaTime * (isDead ? 3.5f : 1f));
		model.material.SetFloat("_DissolveAmount", dissolveAmount);

		gradientBlendAmount = Mathf.Lerp(gradientBlendAmount, 0, Time.deltaTime * 4);
		model.material.SetColor("_Color", hitFeedbackGradient.Evaluate(gradientBlendAmount));

		agent.speed = isDead ? 0 : runSpeed;
		agent.SetDestination(player.position);

		HandleAnimations();

		if(agent.hasPath && agent.remainingDistance <= 1.55f && !isDead)
		{
			if(attackTimer < Time.time)
			{
				StartCoroutine(PunchTarget());
			}
		}
	}

	private void HandleAnimations()
	{
		if(isDead) anim.CrossFade("death");

		else if(!isDead)
		{
			if(agent.velocity.magnitude > 2.5f) anim.CrossFade("run");
			if(agent.velocity.magnitude > 0.05f && agent.velocity.magnitude <= 2.5f) anim.CrossFade("walk");
			if(agent.velocity.magnitude <= 0.05f) anim.CrossFade("idle");
		}
	}

	private IEnumerator PunchTarget()
	{
		attackTimer = Time.time + attackSpeed;
		anim.Blend("punch");
		yield return new WaitForSeconds(0.37f);

		if(!isDead) player.SendMessage("TakeDamage", damage);
	}

	public void ApplyDamage(int amount)
	{
		if(isDead) return;

		enemyHealth -= amount;
		gradientBlendAmount = 1;

		if(enemyHealth <= 0)
		{
			isDead = true;
			if(countKill) FindObjectOfType<TargetManager>().CountKill();
			agent.velocity = Vector3.zero;
			Destroy(gameObject, 1.2f);
		}
	}
}
