﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace NodeEditorFramework.Standard {
    [System.Serializable]
    [Node(false, "Ingredient & Crafting Node")]
    public class IngredientNode : Node {
        public const string ID = "ingredientNode";
        private Vector2 scroller;
        public string ingredientName = "";
        [SerializeField]
        private AbstractCheckable _preCheckables = null;

        public override string GetID {
            get { return ID; }
        }

        public override bool AllowRecursion {
            get { return true; }
        }

        public override Node Create(Vector2 pos) {
            IngredientNode node = CreateInstance<IngredientNode>();
            node.rect = new Rect(pos.x, pos.y, 300, 200);
            node.name = "Ingredient Node";
            node.CreateOutput("Outcome", "Void");
            return node;
        }

        protected internal override void NodeGUI() {
            // if we ever manage to break the UI again, then we have scrollbars.
            scroller = GUILayout.BeginScrollView(scroller);
            rect.height = 92 + 18* Inputs.Count; // we can calculate a fixed node size with a fixed calculation
            ingredientName = GUILayout.TextField(ingredientName); // main dialogue

            GUILayout.BeginHorizontal(); // new row to throw our pre check in
            GUILayout.BeginVertical();
            _preCheckables = (AbstractCheckable) EditorGUILayout.ObjectField(_preCheckables, typeof(AbstractCheckable), true);
            GUILayout.EndVertical();
            GUILayout.BeginVertical(); // and the output on the same line
            Outputs[0].DisplayLayout();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            foreach (NodeInput input in Inputs) { // then add all outputs
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                input.DisplayLayout();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.FlexibleSpace(); // Fills out the bottom

            // node option panel
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Add Ingredient")) {
                CreateInput("Ingredient", "Void");
                CheckTitleType();
                NodeGUI();
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Remove Ingredient")) {
                DeleteInput(Inputs[Inputs.Count - 1]);
                CheckTitleType();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }

        private void CheckTitleType() {
            this.name = Inputs.Count == 0 ? "Ingredient Node" : "Recipe Node";
        }
    }
}