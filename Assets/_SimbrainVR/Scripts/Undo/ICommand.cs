using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    // Start is called before the first frame update
    void Execute();


    // Update is called once per frame
    void Undo();
}
