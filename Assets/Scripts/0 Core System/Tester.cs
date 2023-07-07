using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : BaseControllerMono<Tester>
{
    // Start is called before the first frame update
    void Start()
    {
        GUIController.Controller().ShowPanel<BottomPanel>("BottomPanel", 2);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
