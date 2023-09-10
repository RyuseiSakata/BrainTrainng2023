using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YushaAnim : MonoBehaviour
{
    int num;
    [SerializeField] private GameObject gameObject1;
    [SerializeField] private GameObject gameObject2;
    Yusha script;
    // Start is called before the first frame update
    void Start()
    {

        script = gameObject1.GetComponent<Yusha>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) == true){
            script.SA();
            gameObject2.gameObject.SetActive(false);
        }
    }
}
