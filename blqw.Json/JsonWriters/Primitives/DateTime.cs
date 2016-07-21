using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class DateTimeWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(DateTime);
            }
        }

        private static readonly DateTime _OnlyTime = DateTime.MinValue.AddDays(1).AddTicks(-1);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (DateTime)obj;
            var writer = args.Writer;
            var mode = 0; //0:empty 1:date 2:time 3:datetime

            if (args.FormatDate && value > _OnlyTime)
            {
                mode |= 1;
            }

            if (args.FormatTime)
            {
                if (args.IgnoreEmptyTime && value.Millisecond == 0 && value.Hour == 0 && value.Minute == 0 && value.Second == 0)
                {

                }
                else
                {
                    mode |= 2;
                }
            }

            switch (mode)
            {
                case 1:
                    JsonWriterContainer.StringWriter.Write(value.ToString(args.DateFormatString ?? "yyyy-MM-dd"), args);
                    break;
                case 2:
                    JsonWriterContainer.StringWriter.Write(value.ToString(args.TimeFormatString ?? "HH:mm:ss"), args);
                    break;
                case 3:
                    JsonWriterContainer.StringWriter.Write(value.ToString(args.DateTimeFormatString ?? "yyyy-MM-dd HH:mm:ss"), args);
                    break;
                default:
                    JsonWriterContainer.StringWriter.Write("", args);
                    break;
            }
        }
    }
}
