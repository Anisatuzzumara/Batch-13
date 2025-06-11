namespace Prima
{
    public class Prima
    {
        public bool IsPrima(int candidate)
        {
            if (candidate < 2)
                return false;
            
            if (candidate == 2)
                return true;

            if (candidate % 2 == 0)
                return false;
            
            var boundary = (int)Math.Floor(Math.Sqrt(candidate));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (candidate % i == 0)
                    return false;
            }
            return true;
        }

        public int[] FindPrimaUpTo(int limit)
        {
            if (limit < 2)
                return new int[0];
            
            bool[] isPrima = new bool[limit + 1];
            for (int i = 2; i <= limit; i++)
                isPrima[i] = true;
            
            for (int i = 2; i * i <= limit; i++)
            {
                if (isPrima[i])
                {
                    for (int j = i * i; j <= limit; j += i)
                    {
                        isPrima[j] = false;
                    }
                }
            }

            var prima = new List<int>();
            for (int i = 2; i <= limit; i++)
            {
                if (isPrima[i])
                    prima.Add(i);
            }

            return prima.ToArray();
        }

        public int GetNextPrima(int number)
        {
            if (number < 2)
                return 2;

            int candidate = number + 1;

            while (candidate <= int.MaxValue)
            {
                if (IsPrima(candidate))
                    return candidate;

                candidate++;

                if (candidate < 0) 
                {
                    throw new OverflowException("No prime found within integer range");
                }
            }

            throw new OverflowException("No prime found within integer range");
        }
    }
}