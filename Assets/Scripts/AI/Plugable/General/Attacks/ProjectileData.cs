using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Projectiles/Projectile Data")]
public class ProjectileData : ScriptableObject
{
	public float throwForce;
	public GameObject projectile;
}