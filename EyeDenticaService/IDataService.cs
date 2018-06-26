using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration;
using System.Configuration.Install;

namespace EyeDenticaService
{
    [ServiceContract(Namespace = "EyeDenticaService")]
    public interface IDataService
    {
        [OperationContract]
        double getPrediction();

        [OperationContract(IsOneWay = false)]
        void flushUserRecord(string record);
    }
}
