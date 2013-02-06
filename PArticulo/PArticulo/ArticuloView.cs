using Gtk;
using Serpis.Ad;
using System;
using System.Data;

namespace PArticulo
{
	public partial class ArticuloView : Gtk.Window
	{
		private IDbConnection dbConnection;
		public ArticuloView (long id) : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			
			dbConnection = ApplicationContext.Instance.DbConnection;
			
			if (id == 0) //nuevo
				nuevo();
			else
				editar(id);
			
		}
		
		private void nuevo() {
			//inicializo los controles que quiera
			entryNombre.Text = "Pon el nombre";
			spinButtonPrecio.Value = 1;
			
			saveAction.Activated += delegate {
				Console.WriteLine("saveAction.Activated");
				
				IDbCommand dbCommand = dbConnection.CreateCommand ();
				dbCommand.CommandText = "insert into articulo (nombre, precio) values (:nombre, :precio)";
				
				DbCommandExtensions.AddParameter (dbCommand, "nombre", entryNombre.Text);
				DbCommandExtensions.AddParameter (dbCommand, "precio", Convert.ToDecimal (spinButtonPrecio.Value ));
	
				dbCommand.ExecuteNonQuery ();
				
				Destroy ();
			};
		}
		
		private void editar(long id) {
			IDbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = string.Format ("select * from articulo where id={0}", id);
			
			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();
			
			entryNombre.Text = (string)dataReader["nombre"];
			spinButtonPrecio.Value = Convert.ToDouble( (decimal)dataReader["precio"] );

			long categoria = dataReader["categoria"] is DBNull ? 0 : (long)dataReader["categoria"];
			
			Console.WriteLine("categoria={0}", categoria);
			
			dataReader.Close ();
			
			saveAction.Activated += delegate {
				Console.WriteLine("saveAction.Activated");
				
				IDbCommand dbUpdateCommand = dbConnection.CreateCommand ();
				dbUpdateCommand.CommandText = "update articulo set nombre=:nombre, precio=:precio where id=:id";
				
				DbCommandExtensions.AddParameter (dbUpdateCommand, "nombre", entryNombre.Text);
				DbCommandExtensions.AddParameter (dbUpdateCommand, "precio", Convert.ToDecimal (spinButtonPrecio.Value ));
				DbCommandExtensions.AddParameter (dbUpdateCommand, "id", id);
	
				dbUpdateCommand.ExecuteNonQuery ();
				
				Destroy ();
			};
		}
	}
}

