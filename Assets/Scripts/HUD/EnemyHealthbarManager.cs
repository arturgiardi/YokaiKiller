using UnityEngine;

public class EnemyHealthbarManager : MonoBehaviour
{
    [SerializeField] GameObject healthbarPrefab;
    [SerializeField] Transform worldCanvas;

    public EnemyHealthbar RequestNewHealthbar(Transform followTarget, float fillAmmount)
    {
        GameObject newHealthbar = Instantiate(healthbarPrefab) as GameObject;
        newHealthbar.transform.SetParent(worldCanvas, false);
        EnemyHealthbar hb = newHealthbar.GetComponent<EnemyHealthbar>();
        hb.SetTargetPosition(followTarget);
        hb.SetCurrentFill(fillAmmount);
        return hb;
    }
}