using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour{
    // Start is called before the first frame update
    private void Start(){
        if (transform == transform){
            Update();
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Debug.Log(i+j);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update(){
        
    }
}
