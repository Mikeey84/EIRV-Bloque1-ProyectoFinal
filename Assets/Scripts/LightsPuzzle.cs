using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightsPuzzle : MonoBehaviour
{
    [Header("Levers (assign in order)")]
    public Lever lever1;
    public Lever lever2;
    public Lever lever3;
    public AudioSource a;
    [Header("Events")]
    public List<UnityEvent> onPuzzleSolved;

    private bool solved = false;

    public void CheckSolution()
    {
        a.PlayOneShot(a.clip);

        if (solved) return;
        if (lever1.GetValue() == 1 &&
            lever2.GetValue() == -1 &&
            lever3.GetValue() == -1)
        {
            solved = true;
            
            foreach(var a in onPuzzleSolved)
            {
                a?.Invoke();
            }
        }
    }
}