using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurosuAnim : MonoBehaviour
{
    [SerializeField] private GameObject gameObject1;
    Minotaurosu script;
    // Start is called before the first frame update
    void Start()
    {
        script = gameObject1.GetComponent<Minotaurosu>();
        script.SA();
    }

    public void playAttackAnim()
    {
        script.SA();
    }

}
