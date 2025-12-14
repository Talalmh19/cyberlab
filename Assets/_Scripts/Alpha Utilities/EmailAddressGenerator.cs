using System.Collections.Generic;
using UnityEngine;

public static class EmailAddressGenerator
{
    private static readonly List<string> emailAddresses = new()
    {
        "john.doe@example.com",
        "jane.smith@example.net",
        "michael.brown@example.org",
        "susan.white@example.co",
        "david.johnson@example.com",
        "emily.clark@example.net",
        "chris.evans@example.org",
        "patricia.martin@example.co",
        "robert.wilson@example.com",
        "linda.moore@example.net",
        "james.taylor@example.org",
        "barbara.anderson@example.co",
        "daniel.hall@example.com",
        "nancy.thomas@example.net",
        "mark.jackson@example.org",
        "lisa.harris@example.co",
        "paul.lee@example.com",
        "sandra.walker@example.net",
        "andrew.king@example.org",
        "mary.robinson@example.co",
        "kevin.young@example.com",
        "karen.scott@example.net",
        "steven.green@example.org",
        "betty.baker@example.co",
        "richard.adams@example.com",
        "dorothy.mitchell@example.net",
        "brian.hernandez@example.org",
        "sharon.roberts@example.co",
        "jason.campbell@example.com",
        "michelle.morgan@example.net",
        "matthew.carter@example.org",
        "debora.ward@example.co",
        "george.reed@example.com",
        "diana.bailey@example.net",
        "ryan.phillips@example.org",
        "janet.cooper@example.co",
        "anthony.turner@example.com",
        "carol.parker@example.net",
        "william.collins@example.org",
        "martha.edwards@example.co",
        "jacob.bennett@example.com",
        "laura.flores@example.net",
        "joseph.cox@example.org",
        "anna.rivera@example.co",
        "nicholas.richardson@example.com",
        "sarah.wood@example.net",
        "patrick.james@example.org",
        "judy.russell@example.co",
        "benjamin.wright@example.com",
        "helen.hernandez@example.net"
    };

    public static string GetRandomEmail => emailAddresses[Random.Range(0, emailAddresses.Count)];
}
