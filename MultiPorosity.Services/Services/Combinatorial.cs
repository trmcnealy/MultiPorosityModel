using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiPorosity.Services
{
    [System.Runtime.Versioning.NonVersionable]
    public static class Combinatorial
    {
        public static class Constants
        {
            public static class D
            {
                public const double DECIMAL_DIG = 17;                      // # of decimal digits of rounding precision
                public const double DIG         = 15;                      // # of decimal digits of precision
                public const double EPSILON     = 2.2204460492503131e-016; // smallest such // that 1.0+DBL_EPSILON != 1.0
                public const double HAS_SUBNORM = 1;                       // type does support subnormal numbers
                public const double MANT_DIG    = 53;                      // # of bits in mantissa
                public const double MAX         = 1.7976931348623158e+308; // max value
                public const double MAX_10_EXP  = 308;                     // max decimal exponent
                public const double MAX_EXP     = 1024;                    // max binary exponent
                public const double MIN         = 2.2250738585072014e-308; // min positive value
                public const double MIN_10_EXP  = -307;                    // min decimal exponent
                public const double MIN_EXP     = -1021;                   // min binary exponent
                public const double RADIX       = 2;                       // exponent radix
                public const double TRUE_MIN    = 4.9406564584124654e-324; // min positive value

                public const double OneHalf           = 1.0 / 2.0;
                public const double OneThird          = 1.0 / 3.0;
                public const double OneQuarter        = 1.0 / 4.0;
                public const double E                 = 2.71828182845904523536;                              // e
                public const double Log2E             = 1.44269504088896340736;                              // log2(e)
                public const double OneOverLog2E      = 1.0 / Log2E;                                         // 1/log2(e)
                public const double Log10E            = 0.434294481903251827651;                             // log10(e)
                public const double Ln2               = 0.693147180559945309417;                             // ln(2)
                public const double LnPI              = 1.1447298858494001741434273513530587116472948129153; // ln(pi)
                public const double LogTwoSqrtEOverPi = 0.6207822376352452223455184457816472122518527279025978;
                public const double OneOverLn2        = 1.0 / Ln2;               // 1/ln(2)
                public const double Ln10              = 2.30258509299404568402;  // ln(10)
                public const double OneOverLn10       = 1.0 / Ln10;              // 1/ln(10)
                public const double PI                = 3.14159265358979323846;  // pi
                public const double PIOver2           = 1.57079632679489661923;  // pi/2
                public const double PIOver4           = 0.785398163397448309616; // pi/4
                public const double OneOverPI         = 0.318309886183790671538; // 1/pi
                public const double TwoOverPI         = 0.636619772367581343076; // 2/pi
                public const double TwoOverSQRTPI     = 1.12837916709551257390;  // 2/sqrt(pi)
                public const double SQRT2             = 1.41421356237309504880;  // sqrt(2)
                public const double OneOverSQRT2      = 0.707106781186547524401; // 1/sqrt(2)
            }

            public static class F
            {
                public const float DECIMAL_DIG = 9;                // # of decimal digits of rounding precision
                public const float DIG         = 6;                // # of decimal digits of precision
                public const float EPSILON     = 1.192092896e-07F; // smallest such that 1.0+FLT_EPSILON // != 1.0
                public const float HAS_SUBNORM = 1;                // type does support subnormal numbers
                public const float GUARD       = 0;
                public const float MANT_DIG    = 24;               // # of bits in mantissa
                public const float MAX         = 3.402823466e+38F; // max value
                public const float MAX_10_EXP  = 38;               // max decimal exponent
                public const float MAX_EXP     = 128;              // max binary exponent
                public const float MIN         = 1.175494351e-38F; // min normalized positive value
                public const float MIN_10_EXP  = -37;              // min decimal exponent
                public const float MIN_EXP     = -125;             // min binary exponent
                public const float NORMALIZE   = 0;
                public const float RADIX       = 2;                // exponent radix
                public const float TRUE_MIN    = 1.401298464e-45F; // min positive value

                public const float OneHalf           = 1.0f / 2.0f;
                public const float OneThird          = 1.0f / 3.0f;
                public const float OneQuarter        = 1.0f / 4.0f;
                public const float E                 = 2.71828182845904523536f;                              // e
                public const float Log2E             = 1.44269504088896340736f;                              // log2(e)
                public const float OneOverLog2E      = 1.0f / Log2E;                                         // 1/log2(e)
                public const float Log10E            = 0.434294481903251827651f;                             // log10(e)
                public const float Ln2               = 0.693147180559945309417f;                             // ln(2)
                public const float LnPI              = 1.1447298858494001741434273513530587116472948129153f; // ln(pi)
                public const float LogTwoSqrtEOverPi = 0.6207822376352452223455184457816472122518527279025978f;
                public const float OneOverLn2        = 1.0f / Ln2;               // 1/ln(2)
                public const float Ln10              = 2.30258509299404568402f;  // ln(10)
                public const float OneOverLn10       = 1.0f / Ln10;              // 1/ln(10)
                public const float PI                = 3.14159265358979323846f;  // pi
                public const float PIOver2           = 1.57079632679489661923f;  // pi/2
                public const float PIOver4           = 0.785398163397448309616f; // pi/4
                public const float OneOverPI         = 0.318309886183790671538f; // 1/pi
                public const float TwoOverPI         = 0.636619772367581343076f; // 2/pi
                public const float TwoOverSQRTPI     = 1.12837916709551257390f;  // 2/sqrt(pi)
                public const float SQRT2             = 1.41421356237309504880f;  // sqrt(2)
                public const float OneOverSQRT2      = 0.707106781186547524401f; // 1/sqrt(2)
            }
        }

        private static readonly double[] factorialCache =
        {
            1.00000000000000E+00, 1.00000000000000E+00, 2.00000000000000E+00, 6.00000000000000E+00, 2.40000000000000E+01, 1.20000000000000E+02, 7.20000000000000E+02, 5.04000000000000E+03,
            4.03200000000000E+04, 3.62880000000000E+05, 3.62880000000000E+06, 3.99168000000000E+07, 4.79001600000000E+08, 6.22702080000000E+09, 8.71782912000000E+10, 1.30767436800000E+12,
            2.09227898880000E+13, 3.55687428096000E+14, 6.40237370572800E+15, 1.21645100408832E+17, 2.43290200817664E+18, 5.10909421717094E+19, 1.12400072777761E+21, 2.58520167388850E+22,
            6.20448401733239E+23, 1.55112100433310E+25, 4.03291461126606E+26, 1.08888694504184E+28, 3.04888344611714E+29, 8.84176199373970E+30, 2.65252859812191E+32, 8.22283865417792E+33,
            2.63130836933694E+35, 8.68331761881189E+36, 2.95232799039604E+38, 1.03331479663861E+40, 3.71993326789901E+41, 1.37637530912263E+43, 5.23022617466601E+44, 2.03978820811974E+46,
            8.15915283247898E+47, 3.34525266131638E+49, 1.40500611775288E+51, 6.04152630633738E+52, 2.65827157478845E+54, 1.19622220865480E+56, 5.50262215981209E+57, 2.58623241511168E+59,
            1.24139155925361E+61, 6.08281864034268E+62, 3.04140932017134E+64, 1.55111875328738E+66, 8.06581751709439E+67, 4.27488328406003E+69, 2.30843697339241E+71, 1.26964033536583E+73,
            7.10998587804863E+74, 4.05269195048772E+76, 2.35056133128288E+78, 1.38683118545690E+80, 8.32098711274139E+81, 5.07580213877225E+83, 3.14699732603879E+85, 1.98260831540444E+87,
            1.26886932185884E+89, 8.24765059208247E+90, 5.44344939077443E+92, 3.64711109181887E+94, 2.48003554243683E+96, 1.71122452428141E+98, 1.19785716699699E+100, 8.50478588567862E+101,
            6.12344583768861E+103, 4.47011546151268E+105, 3.30788544151939E+107, 2.48091408113954E+109, 1.88549470166605E+111, 1.45183092028286E+113, 1.13242811782063E+115, 8.94618213078297E+116,
            7.15694570462638E+118, 5.79712602074737E+120, 4.75364333701284E+122, 3.94552396972066E+124, 3.31424013456535E+126, 2.81710411438055E+128, 2.42270953836727E+130, 2.10775729837953E+132,
            1.85482642257398E+134, 1.65079551609085E+136, 1.48571596448176E+138, 1.35200152767840E+140, 1.24384140546413E+142, 1.15677250708164E+144, 1.08736615665674E+146, 1.03299784882391E+148,
            9.91677934870949E+149, 9.61927596824821E+151, 9.42689044888324E+153, 9.33262154439441E+155, 9.33262154439441E+157, 9.42594775983835E+159, 9.61446671503512E+161, 9.90290071648618E+163,
            1.02990167451456E+166, 1.08139675824029E+168, 1.14628056373471E+170, 1.22652020319614E+172, 1.32464181945183E+174, 1.44385958320249E+176, 1.58824554152274E+178, 1.76295255109024E+180,
            1.97450685722107E+182, 2.23119274865981E+184, 2.54355973347219E+186, 2.92509369349301E+188, 3.39310868445190E+190, 3.96993716080872E+192, 4.68452584975429E+194, 5.57458576120760E+196,
            6.68950291344912E+198, 8.09429852527344E+200, 9.87504420083360E+202, 1.21463043670253E+205, 1.50614174151114E+207, 1.88267717688893E+209, 2.37217324288005E+211, 3.01266001845766E+213,
            3.85620482362580E+215, 4.97450422247729E+217, 6.46685548922047E+219, 8.47158069087882E+221, 1.11824865119600E+224, 1.48727070609069E+226, 1.99294274616152E+228, 2.69047270731805E+230,
            3.65904288195255E+232, 5.01288874827499E+234, 6.91778647261949E+236, 9.61572319694109E+238, 1.34620124757175E+241, 1.89814375907617E+243, 2.69536413788816E+245, 3.85437071718007E+247,
            5.55029383273930E+249, 8.04792605747199E+251, 1.17499720439091E+254, 1.72724589045464E+256, 2.55632391787286E+258, 3.80892263763057E+260, 5.71338395644585E+262, 8.62720977423323E+264,
            1.31133588568345E+267, 2.00634390509568E+269, 3.08976961384735E+271, 4.78914290146339E+273, 7.47106292628289E+275, 1.17295687942641E+278, 1.85327186949373E+280, 2.94670227249504E+282,
            4.71472363599206E+284, 7.59070505394721E+286, 1.22969421873945E+289, 2.00440157654530E+291, 3.28721858553429E+293, 5.42391066613159E+295, 9.00369170577843E+297, 1.50361651486500E+300,
            2.52607574497320E+302, 4.26906800900470E+304, 7.25741561530799E+306
        };

        public static double Factorial(int n)
        {
            if(n <= 171)
            {
                return factorialCache[n];
            }

            return double.NaN;
        }

        public static double FactorialLn(int x)
        {
            if(x < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if(x <= 1)
            {
                return 0d;
            }

            if(x < factorialCache.Length)
            {
                return Math.Log(factorialCache[x]);
            }

            return GammaLn(x + 1.0);
        }

        public static double Binomial(int n,
                                      int k)
        {
            if(k < 0 || n < 0 || k > n)
            {
                return 0.0;
            }

            return Math.Floor(0.5 + Math.Exp(FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k)));
        }

        private const int GammaN = 10;

        /// <summary>
        ///     Auxiliary variable when evaluating the <see cref="GammaLn" /> function.
        /// </summary>
        private const double GammaR = 10.900511;

        /// <summary>
        ///     Polynomial coefficients for the <see cref="GammaLn" /> approximation.
        /// </summary>
        private static readonly double[] GammaDk =
        {
            2.48574089138753565546e-5, 1.05142378581721974210, -3.45687097222016235469, 4.51227709466894823700, -2.98285225323576655721, 1.05639711577126713077, -1.95428773191645869583e-1,
            1.70970543404441224307e-2, -5.71926117404305781283e-4, 4.63399473359905636708e-6, -2.71994908488607703910e-9
        };

        public static double GammaLn(double z)
        {
            if(z < 0.5)
            {
                double s = GammaDk[0];

                for(int i = 1; i <= GammaN; i++)
                {
                    s += GammaDk[i] / (i - z);
                }

                return Constants.D.LnPI - Math.Log(Math.Sin(Constants.D.PI * z)) - Math.Log(s) - Constants.D.LogTwoSqrtEOverPi - (0.5 - z) * Math.Log((0.5 - z + GammaR) / Constants.D.E);
            }
            else
            {
                double s = GammaDk[0];

                for(int i = 1; i <= GammaN; i++)
                {
                    s += GammaDk[i] / (z + i - 1.0);
                }

                return Math.Log(s) + Constants.D.LogTwoSqrtEOverPi + (z - 0.5) * Math.Log((z - 0.5 + GammaR) / Constants.D.E);
            }
        }

        public static double SizeOfCombination(int n,
                                               int k)
        {
            return (Factorial(n) / Factorial(n - k)) + n;
        }

        public static int SizeOfPermutations(int n,
                                             int k)
        {
            return (int)(Factorial(n) / (Factorial(n - k) * Factorial(k)));
        }

        public static int SizeOfPermutationsWithRepetition(int n,
                                                           int k)
        {
            return (int)Math.Pow(n, k);
        }

        public static List<List<T>> Combinations<T>(int numberChosen,
                                                    T[] elements)
        {
            int           levelIndex = numberChosen - 1;
            List<List<T>> results    = new List<List<T>>((int)Math.Pow(elements.Length, numberChosen));
            int[]         indices    = new int[numberChosen];

            for(int i = 0; i < elements.Length; i++)
            {
                indices[levelIndex] = i;
                results             = Combinations(levelIndex, elements, results, indices);

                //if(combinations != null)
                //{
                //    results.AddRange(combinations);
                //}

                //for (int j = 0/*i + 1*/; j < elements.Length; j++)
                //{
                //    for (int k = 0/*j + 1*/; k < elements.Length; k++)
                //    {
                //        results.Add(new List<T>
                //                    {
                //                        parameterAttributeKeywords[i],
                //                        parameterAttributeKeywords[j],
                //                        parameterAttributeKeywords[k]
                //                    });
                //    }
                //}
            }

            return results;
        }

        public static List<List<(T1, T2)>> Combinations<T1, T2>(int  numberChosen,
                                                                T1[] k_elements,
                                                                T2[] n_elements)
        {
            int                  levelIndex = numberChosen - 1;
            List<List<(T1, T2)>> results    = new List<List<(T1, T2)>>(SizeOfPermutationsWithRepetition(n_elements.Length, numberChosen));
            int[]                indices    = new int[numberChosen];

            for(int i = 0; i < n_elements.Length; i++)
            {
                indices[levelIndex] = i;
                results             = Combinations(levelIndex, k_elements, n_elements, results, indices);
            }

            return results;
        }

        private static List<List<T>> Combinations<T>(int           numberChosenMinusOne,
                                                     T[]           elements,
                                                     List<List<T>> combinations,
                                                     int[]         indices)
        {
            int levelIndex = numberChosenMinusOne - 1;

            if(levelIndex >= 0)
            {
                //List<List<T>> newCombinations;

                for(int i = 0; i < elements.Length; i++)
                {
                    indices[levelIndex] = i;
                    combinations        = Combinations(levelIndex, elements, combinations, indices);

                    //return newCombinations;

                    //combinations?.AddRange(newCombinations);
                }
            }
            else
            {
                List<T> results = new List<T>(indices.Length);

                for(int i = 0; i < indices.Length; i++)
                {
                    results.Add(elements[indices[i]]);
                }

                combinations.Add(results);
            }

            return combinations;
        }

        private static List<List<(T1, T2)>> Combinations<T1, T2>(int                  numberChosenMinusOne,
                                                                 T1[]                 k_elements,
                                                                 T2[]                 n_elements,
                                                                 List<List<(T1, T2)>> combinations,
                                                                 int[]                indices)
        {
            int levelIndex = numberChosenMinusOne - 1;

            if(levelIndex >= 0)
            {
                for(int i = 0; i < n_elements.Length; i++)
                {
                    indices[levelIndex] = i;
                    combinations        = Combinations(levelIndex, k_elements, n_elements, combinations, indices);
                }
            }
            else
            {
                List<(T1, T2)> results = new List<(T1, T2)>(indices.Length);

                for(int i = 0; i < indices.Length; i++)
                {
                    results.Add((k_elements[numberChosenMinusOne], n_elements[indices[i]]));
                }

                combinations.Add(results);
            }

            return combinations;
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> source)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Permutations(source, new T[0]);
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> source,
                                                                  IEnumerable<T> prefix)
        {
            if(source.Count() == 0)
            {
                yield return prefix;
            }

            foreach(T x in source)
            {
                foreach(IEnumerable<T> permutation in Permutations(source.Except(new[]
                                                                   {
                                                                       x
                                                                   }),
                                                                   prefix.Union(new[]
                                                                   {
                                                                       x
                                                                   })))
                {
                    yield return permutation;
                }
            }
        }

        /// <summary>
        ///     Count the number of possible variations without repetition.
        ///     The order matters and each object can be chosen only once.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Each element is chosen at most once.</param>
        /// <returns>Maximum number of distinct variations.</returns>
        public static double Variations(int n,
                                        int k)
        {
            if(k < 0 || n < 0 || k > n)
            {
                return 0;
            }

            return Math.Floor(0.5 + Math.Exp(FactorialLn(n) - FactorialLn(n - k)));
        }

        /// <summary>
        ///     Count the number of possible variations with repetition.
        ///     The order matters and each object can be chosen more than once.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Each element is chosen 0, 1 or multiple times.</param>
        /// <returns>Maximum number of distinct variations with repetition.</returns>
        public static double VariationsWithRepetition(int n,
                                                      int k)
        {
            if(k < 0 || n < 0)
            {
                return 0;
            }

            return Math.Pow(n, k);
        }

        /// <summary>
        ///     Count the number of possible combinations without repetition.
        ///     The order does not matter and each object can be chosen only once.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Each element is chosen at most once.</param>
        /// <returns>Maximum number of combinations.</returns>
        public static double Combinations(int n,
                                          int k)
        {
            return Binomial(n, k);
        }

        /// <summary>
        ///     Count the number of possible combinations with repetition.
        ///     The order does not matter and an object can be chosen more than once.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Each element is chosen 0, 1 or multiple times.</param>
        /// <returns>Maximum number of combinations with repetition.</returns>
        public static double CombinationsWithRepetition(int n,
                                                        int k)
        {
            if(k < 0 || n < 0 || n == 0 && k > 0)
            {
                return 0;
            }

            if(n == 0 && k == 0)
            {
                return 1;
            }

            return Math.Floor(0.5 + Math.Exp(FactorialLn(n + k - 1) - FactorialLn(k) - FactorialLn(n - 1)));
        }

        /// <summary>
        ///     Count the number of possible permutations (without repetition).
        /// </summary>
        /// <param name="n">Number of (distinguishable) elements in the set.</param>
        /// <returns>Maximum number of permutations without repetition.</returns>
        public static double Permutations(int n)
        {
            return Factorial(n);
        }

        /// <summary>
        ///     Generate a random permutation, without repetition, by generating the index numbers 0 to N-1 and shuffle them
        ///     randomly.
        ///     Implemented using Fisher-Yates Shuffling.
        /// </summary>
        /// <returns>An array of length <c>N</c> that contains (in any order) the integers of the interval <c>[0, N)</c>.</returns>
        /// <param name="n">Number of (distinguishable) elements in the set.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        public static int[] GeneratePermutation(int    n,
                                                Random randomSource = null)
        {
            if(n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            int[] indices = new int[n];

            for(int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            SelectPermutationInplace(indices, randomSource);

            return indices;
        }

        /// <summary>
        ///     Select a random permutation, without repetition, from a data array by reordering the provided array in-place.
        ///     Implemented using Fisher-Yates Shuffling. The provided data array will be modified.
        /// </summary>
        /// <param name="data">The data array to be reordered. The array will be modified by this routine.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        public static void SelectPermutationInplace<T>(T[]    data,
                                                       Random randomSource = null)
        {
            Random random = randomSource ?? new Random();

            // Fisher-Yates Shuffling
            for(int i = data.Length - 1; i > 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                T   swap      = data[i];
                data[i]         = data[swapIndex];
                data[swapIndex] = swap;
            }
        }

        /// <summary>
        ///     Select a random permutation from a data sequence by returning the provided data in random order.
        ///     Implemented using Fisher-Yates Shuffling.
        /// </summary>
        /// <param name="data">The data elements to be reordered.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        public static IEnumerable<T> SelectPermutation<T>(this IEnumerable<T> data,
                                                          Random              randomSource = null)
        {
            Random random = randomSource ?? new Random();
            T[]    array  = data.ToArray();

            // Fisher-Yates Shuffling
            for(int i = array.Length - 1; i >= 0; i--)
            {
                int k = random.Next(i + 1);

                yield return array[k];

                array[k] = array[i];
            }
        }

        /// <summary>
        ///     Generate a random combination, without repetition, by randomly selecting some of N elements.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>Boolean mask array of length <c>N</c>, for each item true if it is selected.</returns>
        public static bool[] GenerateCombination(int    n,
                                                 Random randomSource = null)
        {
            if(n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            Random random = randomSource ?? new Random();
            bool[] mask   = new bool[n];

            for(int i = 0; i < mask.Length; i++)
            {
                mask[i] = random.NextDouble() >= 0.5;
            }

            return mask;
        }

        /// <summary>
        ///     Generate a random combination, without repetition, by randomly selecting k of N elements.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Each element is chosen at most once.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>Boolean mask array of length <c>N</c>, for each item true if it is selected.</returns>
        public static bool[] GenerateCombination(int    n,
                                                 int    k,
                                                 Random randomSource = null)
        {
            if(n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if(k < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            if(k > n)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "k >n");
            }

            Random random = randomSource ?? new Random();
            bool[] mask   = new bool[n];

            if((k * 3) < n)
            {
                // just pick and try
                int selectionCount = 0;

                while(selectionCount < k)
                {
                    int index = random.Next(n);

                    if(!mask[index])
                    {
                        mask[index] = true;
                        selectionCount++;
                    }
                }

                return mask;
            }

            // based on permutation
            int[] permutation = GeneratePermutation(n, random);

            for(int i = 0; i < k; i++)
            {
                mask[permutation[i]] = true;
            }

            return mask;
        }

        /// <summary>
        ///     Select a random combination, without repetition, from a data sequence by selecting k elements in original order.
        /// </summary>
        /// <param name="data">The data source to choose from.</param>
        /// <param name="elementsToChoose">Number of elements (k) to choose from the data set. Each element is chosen at most once.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>The chosen combination, in the original order.</returns>
        public static IEnumerable<T> SelectCombination<T>(this IEnumerable<T> data,
                                                          int                 elementsToChoose,
                                                          Random              randomSource = null)
        {
            T[] array = data as T[] ?? data.ToArray();

            if(elementsToChoose < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elementsToChoose));
            }

            if(elementsToChoose > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(elementsToChoose), "elementsToChoose >data.Count");
            }

            bool[] mask = GenerateCombination(array.Length, elementsToChoose, randomSource);

            for(int i = 0; i < mask.Length; i++)
            {
                if(mask[i])
                {
                    yield return array[i];
                }
            }
        }

        /// <summary>
        ///     Generates a random combination, with repetition, by randomly selecting k of N elements.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Elements can be chosen more than once.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>Integer mask array of length <c>N</c>, for each item the number of times it was selected.</returns>
        public static int[] GenerateCombinationWithRepetition(int    n,
                                                              int    k,
                                                              Random randomSource = null)
        {
            if(n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if(k < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            Random random = randomSource ?? new Random();
            int[]  mask   = new int[n];

            for(int i = 0; i < k; i++)
            {
                mask[random.Next(n)]++;
            }

            return mask;
        }

        /// <summary>
        ///     Select a random combination, with repetition, from a data sequence by selecting k elements in original order.
        /// </summary>
        /// <param name="data">The data source to choose from.</param>
        /// <param name="elementsToChoose">
        ///     Number of elements (k) to choose from the data set. Elements can be chosen more than
        ///     once.
        /// </param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>The chosen combination with repetition, in the original order.</returns>
        public static IEnumerable<T> SelectCombinationWithRepetition<T>(this IEnumerable<T> data,
                                                                        int                 elementsToChoose,
                                                                        Random              randomSource = null)
        {
            if(elementsToChoose < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elementsToChoose));
            }

            T[]   array = data as T[] ?? data.ToArray();
            int[] mask  = GenerateCombinationWithRepetition(array.Length, elementsToChoose, randomSource);

            for(int i = 0; i < mask.Length; i++)
            {
                for(int j = 0; j < mask[i]; j++)
                {
                    yield return array[i];
                }
            }
        }

        /// <summary>
        ///     Generate a random variation, without repetition, by randomly selecting k of n elements with order.
        ///     Implemented using partial Fisher-Yates Shuffling.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Each element is chosen at most once.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>
        ///     An array of length <c>K</c> that contains the indices of the selections as integers of the interval
        ///     <c>[0, N)</c>.
        /// </returns>
        public static int[] GenerateVariation(int    n,
                                              int    k,
                                              Random randomSource = null)
        {
            if(n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if(k < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            if(k > n)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "k > n");
            }

            Random random  = randomSource ?? new Random();
            int[]  indices = new int[n];

            for(int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            // Partial Fisher-Yates Shuffling
            int[] selection = new int[k];

            for(int i = 0, j = indices.Length - 1; i < selection.Length; i++, j--)
            {
                int swapIndex = random.Next(j + 1);
                selection[i]       = indices[swapIndex];
                indices[swapIndex] = indices[j];
            }

            return selection;
        }

        /// <summary>
        ///     Select a random variation, without repetition, from a data sequence by randomly selecting k elements in random
        ///     order.
        ///     Implemented using partial Fisher-Yates Shuffling.
        /// </summary>
        /// <param name="data">The data source to choose from.</param>
        /// <param name="elementsToChoose">Number of elements (k) to choose from the set. Each element is chosen at most once.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>The chosen variation, in random order.</returns>
        public static IEnumerable<T> SelectVariation<T>(this IEnumerable<T> data,
                                                        int                 elementsToChoose,
                                                        Random              randomSource = null)
        {
            Random random = randomSource ?? new Random();
            T[]    array  = data.ToArray();

            if(elementsToChoose < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elementsToChoose));
            }

            if(elementsToChoose > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(elementsToChoose), "elementsToChoose > data.Count");
            }

            // Partial Fisher-Yates Shuffling
            for(int i = array.Length - 1; i >= (array.Length - elementsToChoose); i--)
            {
                int swapIndex = random.Next(i + 1);

                yield return array[swapIndex];

                array[swapIndex] = array[i];
            }
        }

        /// <summary>
        ///     Generate a random variation, with repetition, by randomly selecting k of n elements with order.
        /// </summary>
        /// <param name="n">Number of elements in the set.</param>
        /// <param name="k">Number of elements to choose from the set. Elements can be chosen more than once.</param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>
        ///     An array of length <c>K</c> that contains the indices of the selections as integers of the interval
        ///     <c>[0, N)</c>.
        /// </returns>
        public static int[] GenerateVariationWithRepetition(int    n,
                                                            int    k,
                                                            Random randomSource = null)
        {
            if(n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if(k < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            Random random = randomSource ?? new Random();
            int[]  ret    = new int[k];

            for(int i = 0; i < ret.Length; i++)
            {
                ret[i] = random.Next(0, n);
            }

            return ret;
        }

        /// <summary>
        ///     Select a random variation, with repetition, from a data sequence by randomly selecting k elements in random order.
        /// </summary>
        /// <param name="data">The data source to choose from.</param>
        /// <param name="elementsToChoose">
        ///     Number of elements (k) to choose from the data set. Elements can be chosen more than
        ///     once.
        /// </param>
        /// <param name="randomSource">
        ///     The random number generator to use. Optional; the default random source will be used if
        ///     null.
        /// </param>
        /// <returns>The chosen variation with repetition, in random order.</returns>
        public static IEnumerable<T> SelectVariationWithRepetition<T>(this IEnumerable<T> data,
                                                                      int                 elementsToChoose,
                                                                      Random              randomSource = null)
        {
            if(elementsToChoose < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elementsToChoose));
            }

            T[]   array   = data as T[] ?? data.ToArray();
            int[] indices = GenerateVariationWithRepetition(array.Length, elementsToChoose, randomSource);

            for(int i = 0; i < indices.Length; i++)
            {
                yield return array[indices[i]];
            }
        }
    }
}