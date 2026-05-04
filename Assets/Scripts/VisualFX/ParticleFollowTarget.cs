using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowTarget : MonoBehaviour {
	[SerializeField] Transform target;
	[SerializeField] ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    public float m_Drift = 0.01f;

	public int lastCount;


	public IEnumerator CreateSystem(float waitTime = 1)
	{
		target = GameObject.FindWithTag("PlayerTarget").transform;
		m_System.Play(true);
		yield return new WaitForSeconds(waitTime);
		StartCoroutine(_FollowTarget());
	}

    IEnumerator _FollowTarget()
    {
        InitializeIfNeeded();
		int numParticlesAlive = m_System.GetParticles(m_Particles);
		lastCount = numParticlesAlive;
		while(true)
		{

			// GetParticles is allocation free because we reuse the m_Particles buffer between updates
			numParticlesAlive = m_System.GetParticles(m_Particles);

			if(lastCount > numParticlesAlive)
			{
				lastCount = numParticlesAlive;
				//PlayerFXFeedback.instance.GetSoul();
			}
			//print(numParticlesAlive);
			// Change only the particles that are alive
			for (int i = 0; i < numParticlesAlive; i++)
			{
				m_Particles[i].velocity = Vector3.Lerp(m_Particles[i].velocity, ((target.position) - m_Particles[i].position).normalized * 15, Time.deltaTime * m_Drift);
				//m_Particles[i].velocity = ((target.position + Vector3.up/2) - m_Particles[i].position).normalized * m_Drift;
				//m_Particles[i].position = Vector3.Lerp(m_Particles[i].position, target.position, Time.deltaTime);
			}
			// Apply the particle changes to the particle system
			m_System.SetParticles(m_Particles, numParticlesAlive);
			if(numParticlesAlive == 0)
				break;
			yield return null;
		}
        
    }

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }


}
