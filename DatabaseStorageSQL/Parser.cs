using System.Data.SqlClient;

namespace DatabaseStorageSQL
{
    internal static class Parser
    {
        public static T ConvertToObject<T>(SqlDataReader reader) where T : class, new()
        {
            Type type = typeof(T);

            var props = type.GetProperties();
            var t = new T();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string fieldName = reader.GetName(i);

                    var prop = props.First(pr => string.Equals(pr.Name, fieldName, StringComparison.OrdinalIgnoreCase));
                    if (prop != null)
                    {
                        prop.SetValue(t, Convert.ChangeType(reader.GetValue(i), prop.PropertyType));
                    }
                }
            }

            return t;
        }
    }
}