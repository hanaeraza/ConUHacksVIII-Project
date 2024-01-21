using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class QueueCalculator : MonoBehaviour
{
    public string inputfile;
    private List<Dictionary<string, object>> pointList;
    public int timeColumn = 1;
    public int dayColumn = 2;
    public int roleColumn = 3;
    public int partyColumn = 4;
    public int regionColumn = 5;
    public int rankColumn = 9;


    public string timeColName;
    public string dayColName;
    public string roleColName;
    public string partyColName;
    public string regionColName;
    public string rankColName;

    void Start()
    {
        // Read from CSV data and input into List
        pointList = CSVReader.Read(inputfile);

        List<string> columnList = new List<string>(pointList[1].Keys);

        // Get column names 
        timeColName = columnList[timeColumn];
        dayColName = columnList[dayColumn];
        roleColName = columnList[roleColumn];
        partyColName = columnList[partyColumn];
        regionColName = columnList[regionColumn];
        rankColName = columnList[rankColumn];

        // Pick random player row
        int randPlayer = Random.Range(0, pointList.Count); 
        Debug.Log("Random player row: " + randPlayer); 

        Debug.Log("Player " + randPlayer + " Data: " + pointList[randPlayer][timeColName] + " - "
         + pointList[randPlayer][dayColName] + " - "
         + pointList[randPlayer][roleColName] + " - "
         + pointList[randPlayer][partyColName] + " - "
         + pointList[randPlayer][regionColName] + " - "
         + pointList[randPlayer][rankColName]  );

        string timeData = pointList[randPlayer][timeColName].ToString();
        string dayData = pointList[randPlayer][dayColName].ToString();
        string roleData = pointList[randPlayer][roleColName].ToString();
        int partyData = int.Parse(pointList[randPlayer][partyColName].ToString());
        string regionData = pointList[randPlayer][regionColName].ToString();
        int rankData =  int.Parse(pointList[randPlayer][rankColName].ToString());
        



        // timeVar calculation 
        string[] daysOfWeek = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        
        int dayPoint = 0;

        switch (dayData)
        {
            case "Mon":
                dayPoint = 4;
                break;

            case "Tue":
                dayPoint = 4;
                break;

            case "Wed":
                dayPoint = 4;
                break;

            case "Thu":
                dayPoint = 4;
                break;

            case "Fri":
                dayPoint = 2;
                break;

            case "Sat":
                dayPoint = 1;
                break;

            case "Sun":
                dayPoint = 2;
                break;

            default:
                break;

        }



        int hourPoint = 0;
        string timeStart = "12:53:35";
        string[] timeSplit = timeStart.Split(':');
        int hour = int.Parse(timeSplit[0]);
        int min = int.Parse(timeSplit[1]);
        int roundedTime = (int)Mathf.Round(hour + min / 60f);
        print("roundedTime" + roundedTime);

        switch (hour)
        {
            case 1:
                hourPoint = 2;
                break;

            case 2:
                hourPoint = 2;
                break;

            case 3:
                hourPoint = 3;
                break;

            case 4:
                hourPoint = 4;
                break;

            case 5:
                hourPoint = 5;
                break;

            case 6:
                hourPoint = 6;
                break;

            case 7:
                hourPoint = 7;
                break;

            case 8:
                hourPoint = 8;
                break;

            case 9:
                hourPoint = 9;
                break;

            case 10:
                hourPoint = 10;
                break;

            case 11:
                hourPoint = 10;
                break;

            case 12:
                hourPoint = 9;
                break;

            case 13:
                hourPoint = 8;
                break;

            case 14:
                hourPoint = 7;
                break;

            case 15:
                hourPoint = 6;
                break;

            case 16:
                hourPoint = 5;
                break;

            case 17:
                hourPoint = 4;
                break;

            case 18:
                hourPoint = 3;
                break;

            case 19:
                hourPoint = 3;
                break;

            case 20:
                hourPoint = 2;
                break;

            case 21:
                hourPoint = 1;
                break;

            case 22:
                hourPoint = 1;
                break;

            case 23:
                hourPoint = 1;
                break;

            case 24:
                hourPoint = 1;
                break;

            default:
                break;

        }

        float timeVar = dayPoint * hourPoint;

        platformDictionary["Tokyo"] = "C";
        platformDictionary["Frankfurt"] = "C";
        platformDictionary["London"] = "C";
        platformDictionary["Singapore"] = "PC";
        platformDictionary["Sao Paolo"] = "PC";
        platformDictionary["Australia"] = "B";
        platformDictionary["North Virginia"] = "B";
        platformDictionary["Oregon"] = "B";


        partyDictionary["Tokyo"] = 0.3f;
        partyDictionary["Frankfurt"] = 0.8f;
        partyDictionary["London"] = 0.65f;
        partyDictionary["Singapore"] = 0.6f;
        partyDictionary["Sao Paolo"] = 0.15f;
        partyDictionary["Australia"] = 0.1f;
        partyDictionary["North Virginia"] = 0.55f;
        partyDictionary["Oregon"] = 0.4f;

        float platformVar = PlatformTime(roleData, "Tokyo");
        float partyVar = PartyTime(roleData, partyData, "Tokyo");
        float rankVar = Ranker(rankData);

        //Queue duration estimation
        print(platformVar);
        print(partyVar);
        print(rankVar);
        print(timeVar);
        float estimatedQueue = timeVar + (timeVar / 6) * platformVar + (timeVar / 2) * partyVar + (timeVar / 4) * rankVar;
        Debug.Log(estimatedQueue);
    }

    Dictionary<string, string> platformDictionary = new Dictionary<string, string>();

    public float PlatformTime(string role, string region)
    {
        float score = 0;
        string platform = platformDictionary[region];
        if (role == "Killer")
        {
            if (platform == "C")
                score = 3;
            else if (platform == "B")
                score = 6;
            else if (platform == "PC")
                score = 9;
        }

        else if (role == "Survivor")
        {
            if (platform == "C")
                score = 9;
            else if (platform == "B")
                score = 6;
            else if (platform == "PC")
                score = 3;
        }

        return score;
    }

    Dictionary<string, float> partyDictionary = new Dictionary<string, float>();
    public float PartyTime(string role, int partySize, string region)
    {
        float score = 0;

        if (role == "Survivor")
        {
            float p = partyDictionary[region];
            if (partySize == 1)
            {
                score = Mathf.Pow(p, 3) + p * (1 - p) / 2 + (1 - p) / 2;
            }
            else if (partySize == 2)
            {
                score = Mathf.Pow(p, 2) + (1 - p) / 2;
            }
            else if (partySize == 3)
            {
                score = p;
            }
        }

        return 1 - score;
    }


    public float Ranker(int rank)
    {
        if (rank <= 8)
        {
            return 2f;
        }

        else if (rank <= 17)
        {
            return 4f;
        }
        else
        {
            return 2f;
        }
    }

}


