using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_5_Rasterization.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
