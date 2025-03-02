using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Utilities
{
    public static class PathDb
    {
        public static string GetPath(string dbName)
        {
            string dbPathSql = string.Empty;

            if(DeviceInfo.Platform == DevicePlatform.Android)
            {
                dbPathSql = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dbPathSql = Path.Combine(dbPathSql, dbName);
            }
            if(DeviceInfo.Platform == DevicePlatform.iOS)
            {
                dbPathSql = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dbPathSql = Path.Combine(dbPathSql, "..", "Library", dbName);
            }

            return dbPathSql;
        }
    }
}
