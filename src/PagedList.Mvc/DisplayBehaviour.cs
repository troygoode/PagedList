namespace PagedList.Mvc
{
	/// <summary>
	/// Values for Display options in PagedListRenderOptions
	/// </summary>
	public enum DisplayBehaviour
	{
		/// <summary>
		/// Not includes a hyperlink
		/// </summary>
		None,
		/// <summary>
		/// Always includes a hyperlink
		/// </summary>
		ShowAlways,
		/// <summary>
		/// Includes a hyperlink, if needed
		/// </summary>
		ShowIfNeed
	}
}
