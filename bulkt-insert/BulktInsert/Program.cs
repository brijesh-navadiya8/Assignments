using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-3PQ09PG\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;"))
{
    connection.Open();
    DataTable dt = ConvertCSVtoDataTable(AppDomain.CurrentDomain.BaseDirectory + "\\Assets\\Customers.csv");
    string tableName = "CustomersBulkDemo";
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandType = CommandType.Text;
        command.Parameters.Clear();
        command.CommandText = "IF OBJECT_ID('" + tableName + "', 'U') IS NULL\r\nBEGIN\r\n" + GenerateCreateTableScript(tableName, dt) + "\r\nEND";
        command.ExecuteNonQuery();
    }

    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
    {
        bulkCopy.DestinationTableName = tableName;
        bulkCopy.WriteToServer(dt);
    }
}

DataTable ConvertCSVtoDataTable(string sCsvFilePath)
{
    DataTable dtTable = new DataTable();
    Regex CSVParser = new Regex("[,|;](?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

    using (StreamReader sr = new StreamReader(sCsvFilePath))
    {
        string? head = sr.ReadLine();
        if (head != null)
        {
            string[]? headers = new Regex("[,|;]").Split(head);
            foreach (string header in headers ?? Array.Empty<string>())
            {
                dtTable.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string? s = sr.ReadLine();
                if (s != null)
                {

                    string[] rows = CSVParser.Split(s);
                    DataRow dr = dtTable.NewRow();
                    for (int i = 0; i < (headers?.Length ?? 0); i++)
                    {
                        dr[i] = rows[i].Replace("\"", string.Empty);
                    }
                    dtTable.Rows.Add(dr);
                }
            }
        }
    }

    return dtTable;
}

string GenerateCreateTableScript(string tableName, DataTable table)
{
    string sqlsc;
    sqlsc = "CREATE TABLE " + tableName + "(";
    for (int i = 0; i < table.Columns.Count; i++)
    {
        sqlsc += "\n [" + table.Columns[i].ColumnName + "] ";
        string columnType = table.Columns[i].DataType.ToString();
        switch (columnType)
        {
            case "System.Int32":
                sqlsc += " int ";
                break;
            case "System.Int64":
                sqlsc += " bigint ";
                break;
            case "System.Int16":
                sqlsc += " smallint";
                break;
            case "System.Byte":
                sqlsc += " tinyint";
                break;
            case "System.Decimal":
                sqlsc += " decimal ";
                break;
            case "System.DateTime":
                sqlsc += " datetime ";
                break;
            case "System.String":
            default:
                sqlsc += string.Format(" nvarchar({0}) ", table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString());
                break;
        }
        if (table.Columns[i].AutoIncrement)
            sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
        if (!table.Columns[i].AllowDBNull)
            sqlsc += " NOT NULL ";
        sqlsc += ",";
    }
    return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
}
