using Gtk;
using Npgsql;
using System;
using System.Data;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExecuteActionActivated (object sender, System.EventArgs e)
	{
		string connectionString = "Server=localhost;Database=dbprueba;User Id=dbprueba;Password=sistemas";
		NpgsqlConnection dbConnection = new NpgsqlConnection(connectionString);
		NpgsqlCommand selectCommand = dbConnection.CreateCommand ();
		selectCommand.CommandText = "select * from categoria";
		NpgsqlDataAdapter dbDataAdapter = new NpgsqlDataAdapter();
		new NpgsqlCommandBuilder(dbDataAdapter);
		
		dbDataAdapter.SelectCommand = selectCommand;
		
		DataSet dataSet = new DataSet();
		
		dbDataAdapter.Fill(dataSet);
		
		Console.WriteLine("Tables.Count={0}", dataSet.Tables.Count);
		foreach (DataTable dataTable in dataSet.Tables)
			show (dataTable);
		
		
		DataRow dataRow = dataSet.Tables[0].Rows[0];
		dataRow["nombre"] = DateTime.Now.ToString ();
		
		Console.WriteLine("Tabla con los cambios:");
		show (dataSet.Tables[0]);
		
//		dbDataAdapter.RowUpdating += delegate(object dbDataAdapterSender, 
//		                                      NpgsqlRowUpdatingEventArgs eventArgs) {
//			Console.WriteLine ("RowUpdating command.CommandText={0}", 
//			                   eventArgs.Command.CommandText);
//			
//			foreach (IDataParameter dataParameter in eventArgs.Command.Parameters)
//				Console.WriteLine ("{0}={1}", dataParameter.ParameterName, dataParameter.Value);
//		};
		
		dbDataAdapter.Update (dataSet.Tables[0]);
		//dbDataAdapter.Update (dataSet);
	}
	
	private void show(DataTable dataTable) {
		//foreach (DataColumn dataColumn in dataTable.Columns)
		//	Console.WriteLine("Column.Name={0}", dataColumn.ColumnName);
		
		foreach (DataRow dataRow in dataTable.Rows) {
			foreach (DataColumn dataColumn in dataTable.Columns)
				Console.Write("[{0}={1}] ", dataColumn.ColumnName, dataRow[dataColumn]);
			Console.WriteLine ();
		}
	}
}
