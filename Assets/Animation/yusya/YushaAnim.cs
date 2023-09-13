using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YushaAnim : MonoBehaviour
{
    [SerializeField] private GameObject gameObject1;
    Yusha script;
    // Start is called before the first frame update
    void Start()
    { 
        script = gameObject1.GetComponent<Yusha>();
    }
    void update(){
       
            script.SA();
        
    }
    public void playAttackAnim()
    {
        script.SA();
    }
    
}
