using Microsoft.Data.SqlClient;

namespace DogGo.Utils
{
    public static class DbUtils
    {
        public static string GetString(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if(reader.IsDBNull(ordinal))
            {
                string ifNull = "N/A";
                return ifNull;
            }
            return reader.GetString(ordinal);
        }
    }
}
