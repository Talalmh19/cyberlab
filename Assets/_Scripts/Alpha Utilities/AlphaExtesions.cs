using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public static class AlphaExtensions
{
    /// <summary>
    /// Set alpha value of a SpriteRenderer
    /// </summary>
    /// <param name="renderer">SpriteRenderer</param>
    /// <param name="alpha">Alpha Value</param>
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    /// <summary>
    /// Set alpha value of a Image
    /// </summary>
    /// <param name="image">Image</param>
    /// <param name="alpha">Alpha Value</param>
    public static void Fade(this Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    /// <summary>
    /// Enable GameObject
    /// </summary>
    /// <param name="target">GameObject</param>
    public static void Enable(this GameObject target) { target.SetActive(true); }

    /// <summary>
    /// Disable GameObject
    /// </summary>
    /// <param name="target">GameObject</param>
    public static void Disable(this GameObject target) { target.SetActive(false); }

    /// <summary>
    /// Enable Components GameObject
    /// </summary>
    /// <param name="target">Component</param>
    public static void GOEnable<T>(this T target) where T : Component { target.gameObject.Enable(); }

    /// <summary>
    /// Disable Components GameObject
    /// </summary>
    /// <param name="target">Component</param>
    public static void GODisable<T>(this T target) where T : Component { target.gameObject.Disable(); }

    /// <summary>
    /// Set SetActive Components GameObject
    /// </summary>
    /// <param name="target">Component</param>
    public static void GOSetActive<T>(this T target, bool value = true) where T : Component { target.gameObject.SetActive(value); }

    /// <summary>
    /// Enable GameObject
    /// </summary>
    /// <param name="target">GameObject</param>
    public static void Activate(this GameObject target) { target.SetActive(true); }

    /// <summary>
    /// Set SetActive Default to Enable GameObject
    /// </summary>
    /// <param name="target">GameObject</param>
    public static void SetActive(this GameObject target) { target.SetActive(true); }

    /// <summary>
    /// Disable GameObject
    /// </summary>
    /// <param name="target">GameObject</param>
    public static void Deactivate(this GameObject target) { target.SetActive(false); }

    public static void LocalScaleOne(this Transform target) { target.localScale = Vector3.one; }

    public static void LocalScaleZero(this Transform target) { target.localScale = Vector3.zero; }

    public static T GetRandom<T>(this IList<T> ts, int initialInclusive = 0, int finalExclusive = 0)
    {
        if (finalExclusive == 0) { finalExclusive = ts.Count; }
        return ts[UnityEngine.Random.Range(initialInclusive, finalExclusive)];
    }

    public static T GetRandom<T>(this T[] ts, int initialInclusive = 0, int finalExclusive = 0)
    {
        if (finalExclusive == 0) { finalExclusive = ts.Length; }
        return ts[UnityEngine.Random.Range(initialInclusive, finalExclusive)];
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) { UnityEngine.Object.Destroy(child.gameObject); }
    }

    public static void SetLayersRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetLayersRecursively(layer);
        }
    }

    /// <summary>
    /// Gets, or adds if doesn't contain yet, a component
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    /// <param name="gameObject">GameObject to use</param>
    /// <returns>Component</returns>
    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }

    /// <summary>
    /// Returns true if GameObject has component
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    /// <param name="gameObject">GameObject to use</param>
    /// <returns>If has component</returns>
    public static bool Has<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() != null;
    }

    public static Vector2 ToV2(this Vector3 input) => new(input.x, input.y);

    public static Vector3 Flat(this Vector3 input) => new(input.x, 0, input.z);

    public static Vector3Int ToVector3Int(this Vector3 vec3) => new((int)vec3.x, (int)vec3.y, (int)vec3.z);

    public static string RandomString(this string str, int length)
    {
        System.Random rand = new();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return str + new string(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());
    }

    public static T AddComponentCopy<T>(this GameObject gameObject, T original) where T : Component
    {
        // Get the type of the component
        System.Type type = original.GetType();

        // Add the new component to the GameObject
        T copy = gameObject.AddComponent(type) as T;

        // Copy all fields from the original to the new component
        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            // Deep copy for reference types
            if (field.FieldType.IsClass && field.FieldType != typeof(string))
            {
                object originalValue = field.GetValue(original);
                if (originalValue != null)
                {
                    object copyValue = DeepCopy(originalValue);
                    field.SetValue(copy, copyValue);
                }
            }
            else
            {
                field.SetValue(copy, field.GetValue(original));
            }
        }

        // Copy all properties from the original to the new component
        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (property.CanWrite)
            {
                // Deep copy for reference types
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    object originalValue = property.GetValue(original, null);
                    if (originalValue != null)
                    {
                        object copyValue = DeepCopy(originalValue);
                        property.SetValue(copy, copyValue, null);
                    }
                }
                else
                {
                    property.SetValue(copy, property.GetValue(original, null), null);
                }
            }
        }

        return copy;
    }

    public static T AssignComponentCopy<T>(this GameObject gameObject, T original) where T : Component
    {
        // Get the type of the component
        System.Type type = original.GetType();

        // Add the new component to the GameObject
        T copy = gameObject.GetComponent(type) as T;

        // Copy all fields from the original to the new component
        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            // Deep copy for reference types
            if (field.FieldType.IsClass && field.FieldType != typeof(string))
            {
                object originalValue = field.GetValue(original);
                if (originalValue != null)
                {
                    object copyValue = DeepCopy(originalValue);
                    field.SetValue(copy, copyValue);
                }
            }
            else
            {
                field.SetValue(copy, field.GetValue(original));
            }
        }

        // Copy all properties from the original to the new component
        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (property.CanWrite)
            {
                // Deep copy for reference types
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    object originalValue = property.GetValue(original, null);
                    if (originalValue != null)
                    {
                        object copyValue = DeepCopy(originalValue);
                        property.SetValue(copy, copyValue, null);
                    }
                }
                else
                {
                    property.SetValue(copy, property.GetValue(original, null), null);
                }
            }
        }

        return copy;
    }

    private static object DeepCopy(object obj)
    {
        if (obj == null) return null;
        System.Type type = obj.GetType();

        if (type.IsValueType || type == typeof(string))
        {
            return obj;
        }

        if (type.IsArray)
        {
            System.Type elementType = type.GetElementType();
            var array = obj as System.Array;
            System.Array copiedArray = System.Array.CreateInstance(elementType, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                copiedArray.SetValue(DeepCopy(array.GetValue(i)), i);
            }
            return copiedArray;
        }

        if (type.IsClass)
        {
            object copy = System.Activator.CreateInstance(type);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(obj);
                if (fieldValue != null)
                {
                    field.SetValue(copy, DeepCopy(fieldValue));
                }
            }
            return copy;
        }

        throw new System.ArgumentException("Unknown type");
    }

    public static bool TryCompareTag(this GameObject gameObject, string tag)
    {
        if (gameObject == null || string.IsNullOrEmpty(tag))
        {
            return false;
        }

        try
        {
            return gameObject.CompareTag(tag);
        }
        catch (UnityException)
        {
            Debug.Log($"No such tag have set in Unity {tag}");
            return false;
        }
    }

    public static bool GOTryCompareTag(this Collision2D collision, string tag)
    {
        return collision.gameObject.TryCompareTag(tag);
    }

    public static bool GOTryCompareTag(this Collision collision, string tag)
    {
        return collision.gameObject.TryCompareTag(tag);
    }

    public static bool GOTryCompareTag(this Collider2D collision, string tag)
    {
        return collision.gameObject.TryCompareTag(tag);
    }
}
