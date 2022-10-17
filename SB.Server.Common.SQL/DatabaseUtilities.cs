using Microsoft.Data.SqlClient;

namespace SB.Server.Common.SQL;

public class DatabaseUtilities
{

	public static async Task<T> ExecuteAsync<T>( String connectionString, Func<SqlConnection, Task<T>> func )
	{
		using( var sqlConnection = new SqlConnection( connectionString ) )
		{
			await sqlConnection.OpenAsync();

			return await func( sqlConnection );
		}
	}
	public static async Task ExecuteAsync( String connectionString, Func<SqlConnection, Task> func )
	{
		using( var sqlConnection = new SqlConnection( connectionString ) )
		{
			await sqlConnection.OpenAsync();

			await func( sqlConnection );
		}
	}

	public static async Task ExecuteAsync( String connectionString, Func<SqlConnection, SqlTransaction, Task> func )
	{
		using( var sqlConnection = new SqlConnection( connectionString ) )
		{
			await sqlConnection.OpenAsync();

			var tran = sqlConnection.BeginTransaction();

			await func( sqlConnection, tran );

			tran.Commit();
		}
	}

	public static String GetString( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (String)null : reader.GetString( ordinal ).Trim();
	}
	public static Guid? GetGuid( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (Guid?)null : reader.GetGuid( ordinal );
	}
	public static Int16? GetInt16( SqlDataReader reader, String column, Int16? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (Int16?)null : reader.GetInt16( ordinal );
	}
	public static Int32? GetInt32( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (Int32?)null : reader.GetInt32( ordinal );
	}
	public static Boolean? GetBoolean( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (Boolean?)null : reader.GetBoolean( ordinal );
	}
	public static DateTime? GetDateTime( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (DateTime?)null : reader.GetDateTime( ordinal );
	}
	public static DateTimeOffset? GetDateTimeOffset( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return reader.IsDBNull( ordinal ) ? (DateTimeOffset?)null : reader.GetDateTimeOffset( ordinal );
	}
	public static Int64 GetInt64( SqlDataReader reader, String column, Int32? o = null )
	{
		var ordinal = o.HasValue ? o.Value : reader.GetOrdinal( column );
		return BitConverter.ToInt64( reader.GetSqlBytes( ordinal ).Value.Reverse<byte>().ToArray<byte>(), 0 ); //reader.GetInt64(ordinal);
	}

}