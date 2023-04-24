using System;
using UnityEngine;

namespace EmbASP
{
    public class Scratch : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("START");
            AIController controller = new AIController();
            
            controller.ConfigAsp();
        }
    }
}