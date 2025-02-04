using UnityEngine;

//has all the common functions needed by any States
public interface IState 
{
    //what happens when entering a State
    void OnStateEnter();
    //what happens in Update while in a State
    void Update();
    //what happens in FixedUpdate while in a State
    void FixedUpdate();
    //what happens exiting the current State into the next State
    void OnStateExit();
}
