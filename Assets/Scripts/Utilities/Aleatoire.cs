using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;


namespace Utilities
{

    static class Aleatoire
    {
        //VALEUR MAX 255
        static public int AleatoireBetween(int min, int max)
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            int result = 0;
            byte[] number = new byte[1];
            random.GetBytes(number);
            result = (number[0] % ((max + 1) - min)) + min;
            return result;
        }

        static public float AleatoireBetweenFloat(float min, float max)
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            float result = 0;
            byte[] number = new byte[4];
            random.GetBytes(number);
            result = number[0] + number[1] + number[2] + number[3];
            result = (result % (max - min) + min);
            return result;
        }

        static public bool AleatoireBool()
        {
            bool result = false;
            System.Random aleatoire = new System.Random();
            int temp = aleatoire.Next(2);
            if (temp == 0)
            {
                result = false;
            }
            else result = true;

            return result;
        }

        static Vector2 AleatoireOrientation4()
        {
            Vector2 result = new Vector2();
            int orientation = AleatoireBetween(4, 1);

            switch (orientation)
            {
                case 1:
                    result = new Vector2(1, 0);
                    break;
                case 2:
                    result = new Vector2(0, 1);
                    break;
                case 3:
                    result = new Vector2(-1, 0);
                    break;
                case 4:
                    result = new Vector2(0, 1);
                    break;
            }
            return result;
        }

        static public Vector3 AleatoireVectorDirection()
        {
            return new Vector3(AleatoireBetween(-1, 1), AleatoireBetween(-1, 1), AleatoireBetween(-1, 1));
        }

    }
}
