// See https://aka.ms/new-console-template for more information

using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Logging;
using LogTest;

FbLogManager.Provider = new LoggerProvider(FbLogLevel.Debug);
FbLogManager.IsParameterLoggingEnabled = true;
var con = new FbConnection("CONNECTION STRING");
con.Open();
var cmd = con.CreateCommand();
cmd.CommandText = "COMMAND TEXT";
var read = cmd.ExecuteReader();
while (read.Read())
{
}
read.Close();
con.Close();