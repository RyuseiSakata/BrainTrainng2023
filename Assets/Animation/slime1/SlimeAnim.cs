using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnim : MonoBehaviour
{
    [SerializeField] private GameObject gameObject1;
    Slime script;
    // Start is called before the first frame update
    void Start()
    {
        script = gameObject1.GetComponent<Slime>();
        script.SA();
    }

    public void playAttackAnim()
    {
        script.SA();
    }

}
