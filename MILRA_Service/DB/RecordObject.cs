using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MILRA_Service.DB
{
    public class RecordObject
    {

        [BsonId]
        public long Time { get; set; }
        public string Operation { get; set; }
        public string KeyWord { get; set; }
        public string Window { get; set; }

        private enum INPUT_ORDER : int
        {
            TIME = 0,
            ACTION_TYPE,
            ACTION,
            WINDOW
        }


        public RecordObject(string recordString)
        {
            string[] arg = recordString.Split(',');
            string currTime = arg[(int)INPUT_ORDER.TIME];

            Time = long.Parse(currTime.Substring(1, currTime.Length - 2));

            Operation = arg[(int)INPUT_ORDER.ACTION_TYPE];
            KeyWord = arg[(int)INPUT_ORDER.ACTION];
            Window = arg[(int)INPUT_ORDER.WINDOW];
        }

        public RecordObject(long time,string opr, string key, string win)
        {
            Time = time;
            Operation = opr;
            KeyWord = key;
            Window = win;
        }
    }
}
