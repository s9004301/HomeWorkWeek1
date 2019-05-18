using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeW1.DataTypeAttributes
{
    public class ValidateLogic : DataTypeAttribute
    {
        public ValidateLogic(DataType dataType) : base(dataType)
        {
        }

        public ValidateLogic(string customDataType) : base(customDataType)
        {
        }
    }
}