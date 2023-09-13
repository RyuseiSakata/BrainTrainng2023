using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnim : MonoBehaviour
{
    [SerializeField] private GameObject gameObject1;
    Dragon script;
    // Start is called before the first frame update
    void Start()
    {
        script = gameObject1.GetComponent<Dragon>();
    }

    public void playAttackAnim()
    {
        script.SA();
    }

}
