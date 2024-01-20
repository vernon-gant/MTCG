using System.Data;
using System.Data.Common;
using System.Reflection;

namespace MTCG.Persistance.Database;

public static class TypedQueryExtension
{

    public static async Task<T?> QuerySingleAsync<T>(this DbConnection connection, string sql, object? parameters = null)
    {
        await using DbCommand dbCommand = connection.CreateCommand();
        dbCommand.CommandText = sql;
        dbCommand.AddParameters(parameters);

        await using DbDataReader dataReader = await dbCommand.ExecuteReaderAsync();

        if (!await dataReader.ReadAsync()) return default;

        return Map<T>(dataReader);
    }

    public static async Task<IEnumerable<T>> QueryAsync<T>(this DbConnection connection, string sql, object param = null)
    {
        await using DbCommand dbCommand = connection.CreateCommand();

        dbCommand.CommandText = sql;
        dbCommand.AddParameters(param);

        await using DbDataReader dataReader = await dbCommand.ExecuteReaderAsync();

        List<T> entities = new ();
        while (await dataReader.ReadAsync()) entities.Add(Map<T>(dataReader));

        return entities;
    }

    public static void AddParameters(this DbCommand command, object? parameters)
    {
        if (parameters == null) return;

        foreach (PropertyInfo property in parameters.GetType().GetProperties())
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = property.Name;
            parameter.Value = property.GetValue(parameters);
            command.Parameters.Add(parameter);
        }
    }

    private static T Map<T>(IDataReader reader)
    {
        T entity = Activator.CreateInstance<T>();
        PropertyInfo[] properties = typeof(T).GetProperties();
        Dictionary<string,PropertyInfo> propertyMap = properties.ToDictionary(p => p.Name.ToLower(), p => p);

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string dbColumnName = reader.GetName(i).ToLower();

            if (propertyMap.TryGetValue(dbColumnName, out PropertyInfo? property) && reader.IsDBNull(i) == false && !IsClass(property))
            {
                object value = Convert.ChangeType(reader.GetValue(i), property.PropertyType);
                property.SetValue(entity, value);
            }
        }

        return entity;
    }

    private static bool IsClass(PropertyInfo property) => property.PropertyType != typeof(string) && property.PropertyType.IsClass;

}