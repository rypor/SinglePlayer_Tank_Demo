using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static float FloatStepWithIncrement(float currentVal, float targetVal, float increment)
    {
        increment = Mathf.Abs(increment);
        if(currentVal > targetVal)
        {
            return Mathf.Clamp(currentVal - increment, targetVal, currentVal);
        }
        else
        {
            return Mathf.Clamp(currentVal + increment, currentVal, targetVal);
        }
    }
}
