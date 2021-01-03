using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    class SUTTestGenerator : MonoBehaviour
    {
        private static float fastFastRatio = 71.82f;
        private static float fastSlowRatio = 5.45f;
        private static float slowFastRatio = 0.92f;
        private static float slowSlowRatio = 21.82f;

        private static List<float> situationCapValues =
            new List<float> {
            fastFastRatio,
            fastFastRatio + fastSlowRatio,
            fastFastRatio + fastSlowRatio + slowFastRatio,
            fastFastRatio + fastSlowRatio + slowFastRatio + slowSlowRatio };

        private static float minFirst = 0.20f;
        private static float meanFirst = 1.57f;
        private static float medianFirst = 1.35f;
        private static float maxFirst = 4.32f;

        public static float minSecond = -0.20f;
        private static float meanSecond = 2.64f;
        private static float medianSecond = 2.48f;
        public static float maxSecond = 2.44f;

        //Do not sure if needed
        private static float fastRatioOfFirstCar = 77.27f;
        private static float slowRatioOfFirstCar = 22.73f;

        private static float fastRatioOfSecondCar = 72.73f;
        private static float slowRatioOfSecondCar = 27.27f;
        //
        private static float maxSecond2 = 4.78f;


        // Cluster centers
        private static float fastFirstClusterCenter = 1.07f;
        private static float slowFirstClusterCenter = 2.97f;

        private static float fastSecondClusterCenter = 2.18f;
        private static float slowSecondClusterCenter = 3.75f;

        private static List<string> situations =
            new List<string>() { "ff", "fs", "sf", "ss" };

        public Transform _transform;
        public float timer = 0f;

        [Serializable]
        public struct Dict
        {
            public float key;
            public float value;
        }

        public Dict[] dictOfValuesGlobal = new Dict[1000];

        public void Start()
        {
            _transform = this.transform;
        }

        public void FixedUpdate()
        {
            if (transform.hasChanged)
            {
                Generate(0.66f, 1.98f);
                transform.hasChanged = false;
            }
        }
        public List<float> SetUpValuesForFirstTwoCars()
        {
            string reactionSituation = ChooseSituation();

            List<float> returnFloats = null;
            switch (reactionSituation)
            {
                case "ff":
                    return SetUpProperSUTToSituation(true, true);
                case "fs":
                    return SetUpProperSUTToSituation(true, false);
                case "sf":
                    return SetUpProperSUTToSituation(false, true);
                case "ss":
                    return SetUpProperSUTToSituation(false, false);
            }
            return returnFloats;
        }

        private string ChooseSituation()
        {
            var randomValue = UnityEngine.Random.Range(0.0f, 100.0f);

            if (randomValue >= situationCapValues[2])
            {
                return situations[3];
            }
            if (randomValue >= situationCapValues[1])
            {
                return situations[2];
            }
            if (randomValue >= situationCapValues[0])
            {
                return situations[1];
            }
            if (randomValue >= 0.0f)
            {
                return situations[0];
            }

            return situations[UnityEngine.Random.Range(0, 4)];
        }

        private List<float> SetUpProperSUTToSituation(bool firstFast, bool secondFast)
        {
            var results = Generation(firstFast, secondFast);
            while (results.Sum(x => x) > maxSecond2 || results.Sum(x => x) < 1.16f)
            {
                results = Generation(firstFast, secondFast);
            }
            Debug.Log(results.Sum(x => x));
            return results;
        }

        public List<float> Generation(bool firstFast, bool secondFast)
        {
            float firstCarValue;
            float secondCarValue;
            if (firstFast)
            {
                if (secondFast)
                {
                    firstCarValue = Random.Range(minFirst, minFirst + 2 * (fastFirstClusterCenter - minFirst));
                    secondCarValue = Random.Range(minSecond, minSecond + 2 * (fastSecondClusterCenter - minSecond));
                }
                else
                {
                    firstCarValue = Random.Range(minFirst, minFirst + 2 * (fastFirstClusterCenter - minFirst));
                    secondCarValue = Random.Range(maxSecond - 2 * (maxSecond - slowSecondClusterCenter), maxSecond);
                }
            }
            else
            {
                if (secondFast)
                {
                    firstCarValue = Random.Range(maxFirst - 2 * (maxFirst - slowFirstClusterCenter), maxFirst);
                    secondCarValue = Random.Range(minSecond, minSecond + 2 * (fastSecondClusterCenter - minSecond));
                }
                else
                {
                    firstCarValue = Random.Range(maxFirst - 2 * (maxFirst - slowFirstClusterCenter), maxFirst);
                    secondCarValue = Random.Range(maxSecond - 2 * (maxSecond - slowSecondClusterCenter), maxSecond);
                }
            }

            return new List<float>() { firstCarValue, secondCarValue };
        }

        public void Generate(float secondFastClusterCenter, float secondSlowClusterCenter)
        {
            fastSecondClusterCenter = secondFastClusterCenter;
            slowSecondClusterCenter = secondSlowClusterCenter;
            Dictionary<float, float> dictOfValue = new Dictionary<float, float>();

            for (int i=0; i<1000; i++)
            {
                var generatedValues = SetUpValuesForFirstTwoCars();
                dictOfValue.Add(generatedValues[0],generatedValues[1]);
            }
            var min = dictOfValue.Min(x => x.Value);
            var max = dictOfValue.Max(x => x.Value);
            Debug.Log(min + " : " + max);
            int k = 0;

            foreach (KeyValuePair<float, float> pair in dictOfValue)
            {
                var temp = new Dict()
                {
                    key = pair.Key,
                    value = pair.Value
                };
                dictOfValuesGlobal[k] = temp;
                k++;
            }
        }
    }
}
