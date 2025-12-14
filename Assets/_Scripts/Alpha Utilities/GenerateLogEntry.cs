using UnityEngine;

public class GenerateLogEntry
{
    [System.Serializable]
    public struct LogEntry
    {
        public string IPAddress;
        public int RequestRate;
        public string RequestType;
        public string TimeStamp;
    }

    private static readonly string[] requestTypes = { "GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "TRACE" };

    private static string GenerateRandomIPAddress()
    {
        return $"{Random.Range(1, 256)}.{Random.Range(0, 256)}.{Random.Range(0, 256)}.{Random.Range(1, 256)}";
    }

    private static string GenerateRandomTimeStamp()
    {
        int hour = Random.Range(1, 13);
        int minute = Random.Range(0, 60);
        string period = Random.Range(0, 2) == 0 ? "AM" : "PM";
        return $"{hour:D2}:{minute:D2} {period}";
    }

    public static LogEntry GenerateRandomLogEntry()
    {
        return new LogEntry
        {
            IPAddress = GenerateRandomIPAddress(),
            RequestRate = Random.Range(50, 2001),
            RequestType = requestTypes[Random.Range(0, requestTypes.Length)],
            TimeStamp = GenerateRandomTimeStamp()
        };
    }

    //Debug.Log($"Random Log: IP - {randomLog.IPAddress}, Rate - {randomLog.RequestRate}, Type - {randomLog.RequestType}, Time - {randomLog.TimeStamp}");
}
