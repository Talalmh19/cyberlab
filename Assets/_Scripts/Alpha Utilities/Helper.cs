using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class Helper
{
    private static bool waitArrayPopulated;
    private static Camera _camera;
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    // Dictionaries to cache WaitForSeconds and WaitForSecondsRealtime instances
    private static readonly WaitForSeconds[] waitArray = new WaitForSeconds[5];
    private static readonly Dictionary<float, WaitForSeconds> waitDictionary = new();
    private static readonly Dictionary<int, WaitForSeconds> waitDictionaryInt = new();
    private static readonly Dictionary<float, WaitForSecondsRealtime> realWaitDictionary = new();
    private static readonly Dictionary<int, WaitForSecondsRealtime> realWaitDictionaryInt = new();

    public static Camera Camera
    {
        get
        {
            if (_camera == null) { _camera = Camera.main; }
            return _camera;
        }
    }

    public static void SaveData()
    {
        //DataManager.SaveData();
    }

    public static WaitForSeconds WaitArray(int timeIndex)
    {
        if (waitArrayPopulated)
        {
            return waitArray[timeIndex];
        }

        for (int i = 0; i < 6; i++)
        {
            waitArray[i] = new WaitForSeconds(i);
        }

        waitArrayPopulated = true;
        return waitArray[timeIndex];
    }

    /// <summary>
    /// Gets a cached WaitForSeconds instance for the specified time in seconds.
    /// </summary>
    /// <param name="time">The time to wait in seconds.</param>
    /// <returns>A WaitForSeconds instance for the specified time.</returns>
    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out WaitForSeconds wait))
        {
            return wait;
        }

        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }

    /// <summary>
    /// Gets a cached WaitForSeconds instance for the specified time in seconds (integer).
    /// </summary>
    /// <param name="time">The time to wait in seconds (integer).</param>
    /// <returns>A WaitForSeconds instance for the specified time.</returns>
    public static WaitForSeconds GetWait(int time)
    {
        if (waitDictionaryInt.TryGetValue(time, out WaitForSeconds wait))
        {
            return wait;
        }

        waitDictionaryInt[time] = new WaitForSeconds(time);
        return waitDictionaryInt[time];
    }

    /// <summary>
    /// Gets a cached WaitForSecondsRealtime instance for the specified time in seconds.
    /// </summary>
    /// <param name="time">The time to wait in real time seconds.</param>
    /// <returns>A WaitForSecondsRealtime instance for the specified time.</returns>
    public static WaitForSecondsRealtime GetRealWait(float time)
    {
        if (realWaitDictionary.TryGetValue(time, out WaitForSecondsRealtime wait))
        {
            return wait;
        }

        realWaitDictionary[time] = new WaitForSecondsRealtime(time);
        return realWaitDictionary[time];
    }

    /// <summary>
    /// Gets a cached WaitForSecondsRealtime instance for the specified time in seconds (integer).
    /// </summary>
    /// <param name="time">The time to wait in real time seconds (integer).</param>
    /// <returns>A WaitForSecondsRealtime instance for the specified time.</returns>
    public static WaitForSecondsRealtime GetRealWait(int time)
    {
        if (realWaitDictionaryInt.TryGetValue(time, out WaitForSecondsRealtime wait))
        {
            return wait;
        }

        realWaitDictionaryInt[time] = new WaitForSecondsRealtime(time);
        return realWaitDictionaryInt[time];
    }

    /// <summary>
    /// Determines whether the cursor is currently over any UI element.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the cursor is over a UI element; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    /// <summary>
    /// Determines whether the cursor is over exactly one UI element.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the cursor is over exactly one UI element; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsFirstOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count is > 0 and < 2;
    }

    /// <summary>
    /// Gets the world position of a canvas element using the default camera.
    /// </summary>
    /// <param name="element">The RectTransform of the canvas element.</param>
    /// <returns>The world position of the canvas element.</returns>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main, out var result);
        return result;
    }

    /// <summary>
    /// Gets the world position of a canvas element using a specified camera.
    /// </summary>
    /// <param name="element">The RectTransform of the canvas element.</param>
    /// <param name="camera">The camera used to calculate the world position.</param>
    /// <returns>The world position of the canvas element.</returns>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element, Camera camera)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, camera, out var result);
        return result;
    }

    /// <summary>
    /// Gets the world position of a canvas element's transform using the default camera.
    /// </summary>
    /// <param name="element">The Transform of the canvas element.</param>
    /// <returns>The world position of the canvas element.</returns>
    public static Vector2 GetCanvasElementWorldPosition(Transform element)
    {
        return Camera.main.ScreenToWorldPoint(RectTransformUtility.WorldToScreenPoint(Camera.main, element.position));
    }

    /// <summary>
    /// Gets the world position of a canvas element's transform using a specified camera.
    /// </summary>
    /// <param name="element">The Transform of the canvas element.</param>
    /// <param name="camera">The camera used to calculate the world position.</param>
    /// <returns>The world position of the canvas element.</returns>
    public static Vector2 GetCanvasElementWorldPosition(Transform element, Camera camera)
    {
        return camera.ScreenToWorldPoint(RectTransformUtility.WorldToScreenPoint(camera, element.position));
    }

    /// <summary>
    /// Shuffles the elements of the specified list in place.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rand = new();
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            if (k != i)
            {
                (list[i], list[k]) = (list[k], list[i]);
            }
        }
    }

    /// <summary>
    /// Shuffles the elements of the specified array in place.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to shuffle.</param>
    public static void Shuffle<T>(T[] array)
    {
        System.Random rand = new();
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            if (k != i)
            {
                (array[i], array[k]) = (array[k], array[i]);
            }
        }
    }

    /// <summary>
    /// Generates a random string of the specified length consisting of uppercase letters and digits.
    /// </summary>
    /// <param name="length">The length of the random string to generate.</param>
    /// <returns>A random string of the specified length.</returns>
    public static string RandomString(int length)
    {
        System.Random rand = new();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Generates a unique ID string consisting of a mix of letters, digits, and special characters.
    /// The length of the ID is randomly chosen between 100 and 200 characters.
    /// </summary>
    /// <returns>A unique ID string.</returns>
    public static string GetUniqueID()
    {
        const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{};:'\"\\|,.<>?/`~";
        int length = UnityEngine.Random.Range(100, 201);
        char[] shuffledChars = AllowableCharacters.OrderBy(c => Guid.NewGuid()).ToArray();
        return new string(shuffledChars.Take(length).ToArray());
    }

    /// <summary>
    /// Generates a random string of length 22 consisting of lowercase letters and digits.
    /// </summary>
    /// <returns>A random string of length 22.</returns>
    public static string GetgameID()
    {
        System.Random rand = new();
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 22).Select(s => s[rand.Next(s.Length)]).ToArray());
    }

    public static string RandomName()
    {
        int length = 3;
        System.Random rand = new();
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        string name = new(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());
        return "Player_" + name;
    }

    /// <summary>
    /// Gets a string containing all uppercase letters from A to Z.
    /// </summary>
    /// <value>
    /// A string with the value "ABCDEFGHIJKLMNOPQRSTUVWXYZ".
    /// </value>
    public static string Get_AtoZ => "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Gets a string containing all lowercase letters from a to z.
    /// </summary>
    /// <value>
    /// A string with the value "abcdefghijklmnopqrstuvwxyz".
    /// </value>
    public static string Get_aTOz => "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Gets a string containing all digits from 0 to 9.
    /// </summary>
    /// <value>
    /// A string with the value "0123456789".
    /// </value>
    public static string Get_ZeroTo9 => "0123456789";

    /// <summary>
    /// Quits the game. If running in the Unity Editor, stops play mode.
    /// If running in a built application, closes the application.
    /// </summary>
    public static void QuitGame()
    {
#if UNITY_EDITOR
        // Stops play mode in the Unity Editor.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quits the application in a built executable.
        Application.Quit();
#endif
    }

    public static string PlayerName()
    {
        string fullName = "";// GameStats.gameData.identity.playerName;
        string[] nameParts = fullName.Split('#');

        return nameParts[0];
    }

    public static string PlayerName(string name)
    {
        string fullName = name;
        string[] nameParts = fullName.Split('#');

        return nameParts[0];
    }

    public static bool Chance => UnityEngine.Random.Range(0, 11) % 2 == 0;

    // 50% chance
    public static bool Chance50 => UnityEngine.Random.Range(0, 100) < 50;

    // 25% chance
    public static bool Chance25 => UnityEngine.Random.Range(0, 100) < 25;

    // 10% chance
    public static bool Chance10 => UnityEngine.Random.Range(0, 100) < 10;

    // 5% chance
    public static bool Chance5 => UnityEngine.Random.Range(0, 100) < 5;
}

public enum Mode { Arcade, Freeplay }

public enum CardCorners { None, Zero, Three, Four, Five }

public enum MatchType { None, Quick, Series, Tournament, Friend }

public enum CardSuit { None, Pentagone, Squre, Triangle, Circle }

public enum CardColor { None, Brown, Red, Green, Blue }

public enum Combination { None, Wengla, Street, Quad, Trio, Twin }

public enum DeckType
{
    None, Others, Street, Wengal
}

public enum CollectorType
{
    Twins, Trios, Quads, Street, Wengal
}

[Serializable]
public struct TransformSet
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformSet(Vector3 position = new(), Quaternion rotation = new(), Vector3 scale = new())
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}

[Serializable]
public struct VectDuo
{
    public Vector3 vect1;
    public Vector3 vect2;

    public VectDuo(Vector3 vect1 = default, Vector3 vect2 = default)
    {
        this.vect1 = vect1;
        this.vect2 = vect2;
    }
}

[Serializable]
public struct RotDuo
{
    public Quaternion rot1;
    public Quaternion rot2;

    public RotDuo(Quaternion rot1 = default, Quaternion rot2 = default)
    {
        this.rot1 = rot1;
        this.rot2 = rot2;
    }
}

[Serializable]
public class CombinationBase
{
    public string name;
    public int[] cardIDs;
    public int CardNumber { get; set; }
    public CardSuit CardSuit { get; set; }

    public CombinationBase(int arrayCount)
    {
        cardIDs = new int[arrayCount];
        Clear();
    }

    public void Clear()
    {
        Array.Fill(cardIDs, -1);
    }
}

[Serializable]
public class Twin : CombinationBase
{
    public Twin() : base(2) { }
}

[Serializable]
public class Trio : CombinationBase
{
    public Trio() : base(3) { }
}

[Serializable]
public class Quad : CombinationBase
{
    public Quad() : base(4) { }
}

[Serializable]
public class Street : CombinationBase
{
    public Street() : base(8) { }
}

[Serializable]
public class Wengla : CombinationBase
{
    public Wengla() : base(12) { }
}

[Serializable]
public class CombinationContainer
{
    public List<CombinationBase> Combinations { get; set; }

    public CombinationContainer()
    {
        Combinations = new List<CombinationBase>();
    }
}

[Serializable]
public class ExposeCombination
{
    public string name;
    public int[] cardIDs;
}

/// <summary>
/// Debug.Log
/// </summary>
public static class D
{
    /// <summary>
    /// Debug Your Object
    /// </summary>
    /// <param name="message"></param>
    public static void L(object message)
    {
        Debug.Log(message);
    }
}

[Serializable]
public struct CardMapType
{
    public string name;
    public CardMapIndexes[] cardMapIndexes;
}

[Serializable]
public struct CardMapIndexes
{
    public string name;
    public CardColor cardColor;
    [Range(0, 8)]
    public int[] indexes;
}

[Serializable]
public class GameRound
{
    public string name;
    public GameLevel[] gameLevels = new GameLevel[15];
}

[Serializable]
public class GameLevel
{
    public string name;
    [Range(5, 12)]
    public int gridSize = 5;
    [Range(5.0f, 8.5f)]
    public float gameTime = 5.0f;
    public GameNumber[] gameNumbers = new GameNumber[15];
}

[Serializable]
public class GameNumber
{
    public string name;
    [Range(0, 8)]
    public int gameDificulty;
    public List<string> gameWords;
}
