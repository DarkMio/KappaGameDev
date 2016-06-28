using UnityEngine;
using System.Collections;

namespace NodeEditorFramework.Standard {
    /**
     * A placeholder class to implement any kind of boolean check from a class.
     * This is used to describe and check certain classes and their variables without me
     * programming to deep introspection.
     */
    public abstract class AbstractCheckable : MonoBehaviour {
        /**
         * Should return a booleans, corresponding to the ouput end results.
         * We use this to have some middelware to be able to check engine variables
         * inside the node graph without restricting ourselves too much.
         */
        public abstract bool VariableCheck();
    }
}
