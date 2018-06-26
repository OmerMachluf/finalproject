using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent
{
    class DataServiceManager
    {
        public double getPredictionFromService()
        {

            ServiceReference.DataServiceClient client = new
            ServiceReference.DataServiceClient();


            double rate = 0;

            try {
                rate = client.getPrediction();
                if(!(rate >= 0))
                    rate = 0;

            } catch (Exception e)
            {

            }
            if(client.State == System.ServiceModel.CommunicationState.Opened ||
                client.State == System.ServiceModel.CommunicationState.Opening)
                client.Close();

            return rate;

            
        }
    }
}
