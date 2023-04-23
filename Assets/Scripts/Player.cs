using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name;
    public List<Territory> Territories;
    public int Troops;
    public Color Color;

    public void LoseTerritory(Territory territory)
    {
        throw new System.NotImplementedException();
    }

    public void WinTerritory(Territory territory)
    {
        throw new System.NotImplementedException();
    }
}