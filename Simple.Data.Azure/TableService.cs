﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Simple.Data.Azure.Helpers;
using System.Net;
using Simple.OData;

namespace Simple.Data.Azure
{
    public class TableService
    {
        private readonly ProviderHelper _providerHelper;

        public TableService(ProviderHelper providerHelper)
        {
            _providerHelper = providerHelper;
        }

        public IEnumerable<string> ListTables()
        {
            var request = _providerHelper.CreateTableRequest("Tables", RestVerbs.GET);

            IEnumerable<string> list;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Trace.WriteLine(response.StatusCode, "HttpResponse");
                list = TableHelper.ReadTableList(response.GetResponseStream()).ToList();
            }

            return list;
        }

        public void CreateTable(string tableName)
        {
            var dict = new Dictionary<string, object> { { "TableName", tableName } };
            var data = DataServicesHelper.CreateDataElement(dict);

            DoRequest(data, "Tables", RestVerbs.POST);
        }

        private void DoRequest(XElement element, string command, string method)
        {
            var request = _providerHelper.CreateTableRequest(command, method, element.ToString());

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Trace.WriteLine(response.StatusCode);
            }
        }
    }
}
