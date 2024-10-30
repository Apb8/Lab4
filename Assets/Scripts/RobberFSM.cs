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
    //    while (/* condición para permanecer en Wander */)
    //    {
    //        // Código para movimiento aleatorio
    //        yield return wait;
    //    }
    //    // Cambia a otro estado, como `ApproachState`
    //    currentState = ApproachState;
    //}

    //private IEnumerator ApproachState()
    //{
    //    Debug.Log("Approach state");
    //    // Mueve al ladrón hacia el tesoro
    //    while (/* condición para permanecer en Approach */)
    //    {
    //        // Código para moverse hacia el tesoro
    //        yield return wait;
    //    }
    //    // Cambia de estado según las condiciones (e.g., vuelve a `WanderState` si el policía regresa)
    //    currentState = HideState;

    //}
    // Update is called once per frame
    void Update()
    {
        
    }
}
