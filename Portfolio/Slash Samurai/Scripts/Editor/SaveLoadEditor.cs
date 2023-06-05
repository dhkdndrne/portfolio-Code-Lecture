using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine;
using UnityEditor;

public class SaveLoadEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Save Load")]
    private static void OpenWindow()
    {
        GetWindow<SaveLoadEditor>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        
        tree.Add("저장",new NewSave());
        return tree;
    }
    

    public class NewSave
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
        public SaveLoadSystem saveLoadSystem;

        public NewSave()
        {
            saveLoadSystem = FindObjectOfType<SaveLoadSystem>();
        }
        
    }
}
