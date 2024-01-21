using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlotter : MonoBehaviour
{

    public string inputfile;
    public GameObject DataPointPrefab;
    public GameObject DataPoints;
    public float plotScale = 10;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    public int columnX = 7;
    public int columnY = 9;
    public int columnZ = 4;

    public string xName;
    public string yName;
    public string zName;

    void Start()
    {

        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);

        Debug.Log(pointList);

        List<string> columnList = new List<string>(pointList[1].Keys);

        //Debug.Log("There are " + columnList.Count + " columns in CSV");

        //foreach (string key in columnList)
            //Debug.Log("Column: " + key);


        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];

        for (var i = 0; i < pointList.Count; i++)
        {

            // Get max
            float xMax = FindMax(xName);
            float yMax = FindMax(yName);
            float zMax = FindMax(zName);

            // Get min
            float xMin = FindMin(xName);
            float yMin = FindMin(yName);
            float zMin = FindMin(zName);

            float x = (Convert.ToSingle(pointList[i][xName]) - xMin) / (xMax - xMin);
            float y = (Convert.ToSingle(pointList[i][yName]) - yMin) / (yMax - yMin);
            float z = (Convert.ToSingle(pointList[i][zName]) - zMin) / (zMax - zMin);

            GameObject dataPoint = Instantiate(DataPointPrefab, new Vector3(x, y, z) * plotScale, Quaternion.identity);

            dataPoint.transform.parent = DataPoints.transform;

            string dataPointName =
            pointList[i][xName] + " "
            + pointList[i][yName] + " "
            + pointList[i][zName];

            dataPoint.transform.name = dataPointName;

            dataPoint.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

            dataPoint.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(x, y, z, 1.0f));

            dataPoint.GetComponent<Renderer>().material.color = 
                new Color(x,y,z, 1.0f);

        }
        DataPoints.transform.position = new Vector3(-36f, 7f, 9f);


    }

    private float FindMax(string column)
    {
        float maxValue = Convert.ToSingle(pointList[0][column]);

        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][column]))
                maxValue = Convert.ToSingle(pointList[i][column]);
        }
        return maxValue;
    }

    private float FindMin(string column)
    {
        float minValue = Convert.ToSingle(pointList[0][column]);

        for (var i = 0; i < pointList.Count; i++)
        {
            if (minValue > Convert.ToSingle(pointList[i][column]))
                minValue = Convert.ToSingle(pointList[i][column]);
        }
        return minValue;
    }
}
