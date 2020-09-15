using System;
using UnityEditor;
using UnityEngine;

public class Example : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionResults;
    [MyRange(0, 10)]
    public int range;

    [SerializeField]
    private GameObject sp;

    private GameObject gg;
    private float z = 0;
    // Start is called before the first frame update
    void Start()
    {
        gg = Instantiate(sp, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * 三种旋转
         */
        var rot = gg.transform.rotation;
        rot.z += Time.deltaTime * 10;
        gg.transform.rotation = rot;

        //var ang = gg.transform.rotation.eulerAngles;
        //ang.z += Time.deltaTime * 10;
        //gg.transform.rotation = Quaternion.Euler(ang);

        //z += Time.deltaTime * 10;
        //gg.transform.rotation = Quaternion.Euler(0, 0, z);

        /*
         * 随机数    [0,9]
         */
        //Debug.Log(Random.Range(0, 10));
    }
}

/*
 * 属性编辑器
 */
public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
[Serializable]
public class Ingredient
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}
[CustomPropertyDrawer(typeof(Ingredient))]
public class IngredientDrawer : PropertyDrawer
{
    // 在给定的矩形内绘制属性
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 在父属性上使用 BeginProperty/EndProperty 意味着
        // 预制件重写逻辑作用于整个属性。
        EditorGUI.BeginProperty(position, label, property);

        //绘制标签
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // 不要让子字段缩进
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // 计算矩形
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // 绘制字段 - 将 GUIContent.none 传入每个字段，从而可以不使用标签来绘制字段
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        // 将缩进恢复原样
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
public class MyRangeAttribute : PropertyAttribute
{
    public readonly float min;
    public readonly float max;

    public MyRangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
[CustomPropertyDrawer(typeof(MyRangeAttribute))]
public class RangeDrawer : PropertyDrawer
{
    // 在给定的矩形内绘制属性
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //首先获取该特性，因为它包含滑动条的范围
        MyRangeAttribute range = (MyRangeAttribute)attribute;

        // 现在根据属性是浮点值还是整数来确定将属性绘制为 Slider 还是 IntSlider。
        if (property.propertyType == SerializedPropertyType.Float)
            EditorGUI.Slider(position, property, range.min, range.max, label);
        else if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);
        else
            EditorGUI.LabelField(position, label.text, "Use MyRange with float or int.");
    }
}