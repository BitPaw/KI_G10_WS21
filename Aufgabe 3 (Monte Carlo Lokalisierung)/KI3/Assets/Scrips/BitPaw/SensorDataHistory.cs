namespace BitPaw
{
    public class SensorDataHistory    
    {
        public int TimeStapListSize = 0;
        public SensorDataSet[] TimeStapList = null;       
         

        public SensorDataHistory()
        {
            TimeStapListSize = 1;
            TimeStapList = new SensorDataSet[TimeStapListSize];

            for (int i = 0; i < TimeStapListSize; i++)
            {
                TimeStapList[i] = new SensorDataSet();
            }
        }

        public SensorDataSet GetCurrent()
        {
            return TimeStapList[TimeStapListSize - 1];
        }

        public SensorDataSet GetNext()
        {
            return TimeStapList[TimeStapListSize-1];
        }
    }
}
