using System;
using System.Collections.Generic;

namespace ImageMoji.KMP
{
    public class KMP
    {
        static int[] pi;
        static string T, P;
        static int TLen, PLen;
        static int cnt;
        static List<int> result = new List<int>();

        /** initialize */
        public static void Initial(string inputT, string inputP)
        {
            T = inputT;
            P = inputP;
            TLen = T.Length;
            PLen = P.Length;
            pi = new int[PLen + 1];
            cnt = 0;
        }

        static void GetPi()
        {
            int i = -1, j = 0;
            pi[j] = i;
            while (j < PLen)
            {
                if (i == -1 || P[i] == P[j])
                    pi[++j] = ++i;
                else
                    i = pi[i];
            }
        }

        static void KMPAlgorithm()
        {
            int i = 0, j = 0;
            while (i < TLen)
            {
                if (j == -1 || T[i] == P[j])
                {
                    i++;
                    j++;
                }
                else
                    j = pi[j];
                if (j == PLen)
                {
                    cnt++;
                    result.Add(i - PLen + 1);
                    j = pi[j];
                }
            }
        }

        public static void Main(string T, string P)
        {
            Initial(inputT: T, inputP: P);
            GetPi();
            KMPAlgorithm();
            Console.WriteLine(cnt);
            Console.WriteLine(string.Join(" ", result));
        }
    }
}