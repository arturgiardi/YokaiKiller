using UnityEngine;

public class Area1Room2Cutscene1PositionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cutscene_Raiko;
    public bool Triggered { get; private set; } = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cutscene_Raiko)
        {
            Triggered = true;
        }
    }
}
