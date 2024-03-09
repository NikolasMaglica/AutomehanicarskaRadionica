using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Services
{
	public class Result
	{
		public bool IsSuccess { get; set; }
		public string ErrorMessage { get; set; }

		public static Result Ok()
		{
			return new Result { IsSuccess = true };
		}

		public static Result Fail(string errorMessage)
		{
			return new Result { IsSuccess = false, ErrorMessage = errorMessage };
		}
	}
}
