using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberFSM : MonoBehaviour
{
    private delegate IEnumerator State();
    private State currentState;
    private WaitForSeconds wait = new WaitForSeconds(0.05f);

    // Start is called before the first frame update
    void Start()
    {    
        //currentState = WanderState;
        StartCoroutine(StateMachine());

    }

    private IEnumerator StateMachine()
    {
        while (enabled)
        {
            yield return StartCoroutine(currentState());
        }
    }

    //private IEnumerator WanderState()
    //{
    //    Debug.Log("Wander state");
    //    while (/* condici�n para permanecer en Wander */)
    //    {
    //        // C�digo para movimiento aleatorio
    //        yield return wait;
    //    }
    //    // Cambia a otro estado, como `ApproachState`
    //    currentState = ApproachState;
    //}

    //private IEnumerator ApproachState()
    //{
    //    Debug.Log("Approach state");
    //    // Mueve al ladr�n hacia el tesoro
    //    while (/* condici�n para permanecer en Approach */)
    //    {
    //        // C�digo para moverse hacia el tesoro
    //        yield return wait;
    //    }
    //    // Cambia de estado seg�n las condiciones (e.g., vuelve a `WanderState` si el polic�a regresa)
    //    currentState = HideState;

    //}
    // Update is called once per frame
    void Update()
    {
        
    }
}
