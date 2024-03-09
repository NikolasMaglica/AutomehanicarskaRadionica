using System.ComponentModel.DataAnnotations.Schema;
using Vjezba.Model;

namespace Vjezba.Model
{
	public class VehicleFilterModel
	{
		public string ModelName { get; set; } = "";
		public int ModelYear { get; set; }
		public int ManufacturerID { get; set; }
	}
}
