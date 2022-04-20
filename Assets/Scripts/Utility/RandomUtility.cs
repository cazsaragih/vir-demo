using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace VirTest.Utility
{
    public class RandomUtility
    {
        public static string GenerateAlphaNumericString()
        {
            StringBuilder sb = new StringBuilder();

            // ASCII code for 0-9
            for (int i = 48; i <= 57; i++)
            {
                char c = Convert.ToChar(i);
                sb.Append(c);
            }

            // ASCII code for A-Z
            for (int i = 65; i <= 90; i++)
            {
                char c = Convert.ToChar(i);
                sb.Append(c);
            }

            // ASCII code for a-z
            for (int i = 97; i <= 122; i++)
            {
                char c = Convert.ToChar(i);
                sb.Append(c);
            }

            int length = sb.Length;
            int randIndex = UnityEngine.Random.Range(0, length);

            string randString = sb[randIndex].ToString();

            return randString;
        }
    }
}