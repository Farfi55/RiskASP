using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
        
    }
}
