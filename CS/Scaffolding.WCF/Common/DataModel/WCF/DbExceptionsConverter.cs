﻿using System;
using System.Linq;
using System.Text;

namespace Scaffolding.WCF.Common.DataModel.WCF {
    public static class DbExceptionsConverter {
        public static Exception Convert(Exception exception) {
            System.Data.Services.Client.DataServiceRequestException dataServiceRequestException = exception as System.Data.Services.Client.DataServiceRequestException;
            if(dataServiceRequestException != null)
                return new DbException(dataServiceRequestException.Message, CommonResources.Exception_DataServiceRequestErrorCaption, exception);
            return exception;
        }
    }
}