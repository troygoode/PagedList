using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PagedList.Mvc
{
	/// <summary>
	/// The order in which to display the navigation controls
	/// </summary>
	public enum DisplayNavigationBehaviour
	{
		/// <summary>
		/// Primarily show First an Last elements
		/// </summary>
		FirstLastPrimary,
		/// <summary>
		/// Primarily show Prev an Next elements
		/// </summary>
		PrevNextPrimary
	}
}
