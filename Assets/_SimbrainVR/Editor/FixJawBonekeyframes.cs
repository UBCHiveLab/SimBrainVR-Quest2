//Copyright(c) 2021 Robin Promesberger (schema_unity on forum.unity3d.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class FixJawBoneKeyframes : EditorWindow
{
    [MenuItem("Window/Fix Jawbone Keyframes")]
    public static void OpenWindow()
    {
        var window = GetWindow<FixJawBoneKeyframes>();
        window.titleContent = new GUIContent("Jaw Fixer");
    }
    private GUIContent statusContent = new GUIContent("Waiting...");
    public List<AnimationClip> animations = new List<AnimationClip>();
    private Transform rootBone;
    Vector2 scrollPos;
    private float keyValue = 1;
    private string bindingName = "Jaw Close";
    private string statusText = "Waiting...";
    private void OnGUI()
    {
        keyValue = EditorGUILayout.FloatField("Jaw Keyframe val", keyValue);

        GUIStyle GuistyleBoxDND = new GUIStyle(GUI.skin.box);
        Rect myRect = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
        GUI.Box(myRect, "Drag and Drop AnimationClips to this Box!", GuistyleBoxDND);
        //drag and drop box
        if (myRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                Debug.Log(DragAndDrop.objectReferences.Length);
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    if (DragAndDrop.objectReferences[i] is AnimationClip)
                    {
                        animations.Add(DragAndDrop.objectReferences[i] as AnimationClip);
                    }
                }
                Event.current.Use();
            }
        }
        EditorGUILayout.BeginHorizontal();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, true, true);

        //list of animation clips
        var list = animations;
        int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("size", list.Count));
        while (newCount < list.Count)
            list.RemoveAt(list.Count - 1);
        while (newCount > list.Count)
            list.Add(null);

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = EditorGUILayout.ObjectField(list[i], typeof(AnimationClip), true) as AnimationClip;
        }


        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndHorizontal();


        bool enabled = (list != null && list.Count > 0);
        if (!enabled)
        {
            statusText = "Add animation clips to process.";
        }
        GUI.enabled = enabled;
        //processing
        if (GUILayout.Button("Fix Jaw Bones"))
        {
            statusText = "++ Processing animations... ++\n\n";
            // process clips
            foreach (AnimationClip clip in list)
            {

                EditorCurveBinding[] binding = AnimationUtility.GetCurveBindings(clip);
                bool found = false; ;
                //we could reference the name directly, but since performance is not an issue, it's more convenient and safer to use the existing binding
                for (int i = 0; i < binding.Length; i++)
                {
                    if (binding[i].propertyName == bindingName)
                    {
                        found = true;

                        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding[i]);
                        //set each existing keyframe to the value provided (by default unity creates one at the start and end)
                        Keyframe[] copy = curve.keys; //this creates a copy of the array
                        for (int k = 0; k < copy.Length; k++)
                        {
                            copy[k].value = keyValue;
                        }
                        curve.keys = copy; //this copies the array into the curve
                        statusText += "Processed keyframes of " + clip.name + "\n";
                        //set the curve for the binding
                        AnimationUtility.SetEditorCurve(clip, binding[i], curve);
                        break;
                    }
                }
                if (!found)
                {
                    statusText += "ERROR: '" + bindingName + "' binding not found. Animation has to be humanoid\n";
                    break;
                }
            }
        }
        // Draw status info
        statusContent.text = statusText;
        EditorStyles.label.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, true, true);
        GUILayout.Label(statusContent);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndHorizontal();

    }
}