using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
namespace SB.Utilities;

public class JSONUtilities
{
    public static String GetString( JObject obj, String name, String defaultValue = "" )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return str.ToString();
    }
    public static Boolean GetBool( JObject obj, String name, Boolean defaultValue = false )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return bool.Parse( str.ToString() );
    }
    public static Int32 GetInt( JObject obj, String name, Int32 defaultValue = 0 )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return int.Parse( str.ToString() );
    }
    public static Int64 GetInt64( JObject obj, String name, Int64 defaultValue = 0 )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return Int64.Parse( str.ToString(), NumberStyles.Any );
    }
    public static Int32? GetNullableInt( JObject obj, String name, Int32? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return Int32.Parse( str.ToString(), NumberStyles.Any );
    }

    public static Int64? GetNullableInt64( JObject obj, String name, Int64? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return Int64.Parse( str.ToString(), NumberStyles.Any );
    }
    public static Single GetFloat( JObject obj, String name, Single defaultValue = 0f )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return float.Parse( str.ToString(), new CultureInfo( "en-US" ) );
    }

    public static Single? GetNullableFloat( JObject obj, String name, Single? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return float.Parse( str.ToString(), new CultureInfo( "en-US" ) );
    }
    public static Double GetDouble( JObject obj, String name, Double defaultValue = 0d )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return Double.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }
    public static Double? GetNullableDouble( JObject obj, String name, Double? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return Double.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }
    public static decimal GetDecimal( JObject obj, String name, decimal defaultValue = 0 )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return Decimal.Parse( str.ToString(), NumberStyles.Any );
    }
    public static Guid GetGuid( JObject obj, String name, Guid defaultValue = default( Guid ) )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return Guid.Parse( str.ToString() );
    }
    public static Guid? GetNullableGuid( JObject obj, String name, Guid? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return Guid.Parse( str.ToString() );
    }
    public static DateTime GetDateTime( JObject obj, String name, DateTime defaultValue = default( DateTime ) )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return DateTime.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }
    public static DateTime? GetNullableDateTime( JObject obj, String name, DateTime? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return DateTime.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }
    public static DateTimeOffset GetDateTimeOffset( JObject obj, String name, DateTimeOffset defaultValue = default( DateTimeOffset ) )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return DateTimeOffset.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }

    public static DateTimeOffset? GetNullableDateTimeOffset( JObject obj, String name, DateTimeOffset? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return DateTimeOffset.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }

    public static TimeSpan GetTimeSpan( JObject obj, String name, TimeSpan defaultValue = default( TimeSpan ) )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null )
        {
            return defaultValue;
        }

        return TimeSpan.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }
    public static TimeSpan? GetNullableTimeSpan( JObject obj, String name, TimeSpan? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return TimeSpan.Parse( str.ToString(), CultureInfo.InvariantCulture );
    }
    public static Boolean GetBoolean( JObject obj, String name, Boolean defaultValue = false )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return Boolean.Parse( str.ToString() );
    }
    public static Boolean? GetNullableBoolean( JObject obj, String name, Boolean? defaultValue = null )
    {
        var v = (JValue)obj[name];

        if( v == null )
        {
            return defaultValue;
        }

        var str = ( v ).Value;

        if( str == null || String.IsNullOrEmpty( str.ToString() ) )
        {
            return defaultValue;
        }

        return Boolean.Parse( str.ToString() );
    }

    public static void Set( JObject obj, String name, Object value )
    {
        var property = obj.Property( name );

        if( property == null )
        {
            obj.Add( new JProperty( name, value ) );
        }
        else
        {
            property.Value = new JValue( value );
        }
    }

    public static Boolean Exists( JObject obj, String name )
    {
        var property = obj.Property( name );

        if( property == null )
        {
            return false;
        }
        else
        {
            if( property.HasValues )
            {
                return property.Value.Type != JTokenType.Null;
            }
            else
            {
                return false;
            }
        }
    }
}

public static class JsonHelper
{
    public static JObject ReadJObjectAsync( Stream json )
    {
        using( var streamReader = new StreamReader( json ) )
        {
            using( var jsonReader = new JsonTextReader( streamReader ) )
            {
                return (JObject)JToken.ReadFrom( jsonReader );
            }
        }
    }
    public static JArray ReadJArray( Stream json )
    {
        using( var streamReader = new StreamReader( json ) )
        {
            using( var jsonReader = new JsonTextReader( streamReader ) )
            {
                return (JArray)JToken.ReadFrom( jsonReader );
            }
        }
    }

  public static JObject GetObjectFromValueTag(string result )
  {
    var json = JObject.Parse( result );
    return (JObject)json["value"];
  }

  public static JArray GetArrayFromValueTag(string result )
  {
    var json = JObject.Parse( result );
    return (JArray)json["value"];
  }


}