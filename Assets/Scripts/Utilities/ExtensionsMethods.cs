using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsMethods {
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float LinearToDecibel(this float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

     public static float DecibelToLinear(this float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);

        return linear;
    }

}
