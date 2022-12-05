using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluePoint : MonoBehaviour
{
    public Clue _clue = new Clue() { id = -1 };
    public string hint;

    private void Awake()
    {
       // _clue = new Clue() { id = -1 };
    }

    public void Set_Clue(Clue clue)
    {
        _clue = clue;
    }
}
