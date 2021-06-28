using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    object captureState();
    void restoreState(object state);
}
