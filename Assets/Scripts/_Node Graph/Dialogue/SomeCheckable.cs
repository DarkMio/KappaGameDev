using UnityEngine;
using System.Collections;
using NodeEditorFramework.Standard;

public class SomeCheckable : AbstractCheckable {
    public bool isTrue;
    public override bool VariableCheck() {
        return isTrue;
    }
}
