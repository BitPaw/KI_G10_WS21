namespace BitPaw
{
    // Set of values that every Sensor has in this scene
    public class SensorDataSet   
    {
        public int SensorDataVieldOfViewListSize = 0;
        public SensorDataVieldOfView[] SensorDataVieldOfViewList = null;

        public void Reserve(int size)
        {
            if (SensorDataVieldOfViewListSize == size)
            {
                // We have the size, dont allocate.
                return;
            }

            SensorDataVieldOfViewListSize = size;
            SensorDataVieldOfViewList = new SensorDataVieldOfView[size];

            for (int i = 0; i < SensorDataVieldOfViewListSize; i++)
            {
                SensorDataVieldOfViewList[i] = new SensorDataVieldOfView();
            }
        }
    }
}