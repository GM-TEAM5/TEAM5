using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute), true)]
public class ReadOnlyDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
    {
        bool disabled = true;
        switch (((ReadOnlyAttribute)attribute).runtimeOnly) {
            case EReadOnlyType.FULLY_DISABLED:
                disabled = true;
                break;
            case EReadOnlyType.EDITABLE_RUNTIME:
                disabled = !UnityEngine.Application.isPlaying;
                break;
            case EReadOnlyType.EDITABLE_EDITOR:
                disabled = UnityEngine.Application.isPlaying;
                break;
        }
            
        using (var scope = new EditorGUI.DisabledGroupScope(disabled))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

}

[CustomPropertyDrawer(typeof(BeginReadOnlyAttribute))]
public class BeginReadOnlyGroupDrawer : DecoratorDrawer
{

    public override float GetHeight()
    {
        return 0;
    }

    public override void OnGUI(UnityEngine.Rect position)
    {
        EditorGUI.BeginDisabledGroup(true);
    }

}

[CustomPropertyDrawer(typeof(EndReadOnlyAttribute))]
public class EndReadOnlyGroupDrawer : DecoratorDrawer
{

    public override float GetHeight()
    {
        return 0;
    }

    public override void OnGUI(UnityEngine.Rect position)
    {
        EditorGUI.EndDisabledGroup();
    }

}