using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PagedList.Mvc
{
    /// <summary>
    ///	Extension methods for generating pagination controls that can operate on instances of IPagedList.
    ///	</summary>
    ///	<remarks>Based on RDIO / Twitter Bootstrap</remarks>
    public static class Pagination
    {
        private static TagBuilder WrapInListItem(TagBuilder inner, params string[] classes)
        {
            var li = new TagBuilder("li");
            foreach (var @class in classes)
                li.AddCssClass(@class);
            li.InnerHtml = inner.ToString();
            return li;
        }

        private static TagBuilder Previous(IPagedList list, Func<int, string> generatePageUrl)
        {
            var targetPageNumber = list.PageNumber - 1;
            var previous = new TagBuilder("a");
            previous.InnerHtml = "&larr; Previous";

            if (!list.HasPreviousPage)
                return WrapInListItem(previous, "prev", "disabled");

            previous.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(previous, "prev");
        }

        private static TagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl, string format)
        {
            return Page(i, list, generatePageUrl, (pageNumber => string.Format(format, pageNumber)));
        }

        private static TagBuilder Page(int i, IPagedList list, Func<int, string> generatePageUrl, Func<int, string> format)
        {
            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.SetInnerText(format(targetPageNumber));

            if (i == list.PageNumber)
                return WrapInListItem(page, "active");

            page.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(page);
        }

        private static TagBuilder Next(IPagedList list, Func<int, string> generatePageUrl)
        {
            var targetPageNumber = list.PageNumber + 1;
            var next = new TagBuilder("a");
            next.InnerHtml = "Next &rarr;";

            if (!list.HasNextPage)
                return WrapInListItem(next, "next", "disabled");

            next.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(next, "next");
        }

        private static TagBuilder Ellipses()
        {
            TagBuilder li = new TagBuilder("li");
            li.AddCssClass("disabled");
            li.InnerHtml = "<a href=\"#\">…</a>";
            return li;
        }

        ///<summary>
        ///	Displays a pagination control for instances of PagedList.
        ///</summary>
        ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name="list">The PagedList to use as the data source.</param>
        ///<param name="generatePageUrl">A function that takes the page number  of the desired page and returns a URL-string that will load that page.</param>
        ///<param name="maxPages">The maximum number of pages to show to select from.</param>
        ///<returns>Outputs the paging control HTML.</returns>
        public static MvcHtmlString Pager(this System.Web.Mvc.HtmlHelper html, IPagedList list, Func<int, string> generatePageUrl, int maxPages = 11)
        {
            /* http://twitter.github.com/bootstrap/#tables
            
 <div class="pagination">
  <ul>
    <li class="prev disabled"><a href="#">&larr; Previous</a></li>
    <li class="active"><a href="#">1</a></li>
    <li><a href="#">2</a></li>
    <li><a href="#">3</a></li>
    <li><a href="#">4</a></li>
    <li><a href="#">5</a></li>
    <li class="next"><a href="#">Next &rarr;</a></li>
  </ul>
</div>
            */

            StringBuilder listItemLinks = new StringBuilder();

            // previous
            listItemLinks.Append(Previous(list, generatePageUrl));

            //calculate start and end of range of page numbers
            var start = 1;
            var end = list.PageCount;
            if (list.PageCount > maxPages)
            {
                start = list.PageNumber - maxPages / 2;
                if (start < 1)
                    start = 1;
                end = maxPages;
                if ((start + end - 1) > list.PageCount)
                    start = list.PageCount - maxPages + 1;
            }

            //if there are previous page numbers not displayed, show an ellipsis
            if (start > 1)
                listItemLinks.Append(Ellipses());

            foreach (var i in Enumerable.Range(start, end))
            {
                //show page number link
                listItemLinks.Append(Page(i, list, generatePageUrl, "{0}"));
            }

            //if there are subsequent page numbers not displayed, show an ellipsis
            if ((start + end - 1) < list.PageCount)
                listItemLinks.Append(Ellipses());

            // next
            listItemLinks.Append(Next(list, generatePageUrl));

            var ul = new TagBuilder("ul");
            ul.InnerHtml = listItemLinks.ToString();

            var div = new TagBuilder("div");
            div.AddCssClass("pagination");
            div.InnerHtml = ul.ToString();

            return new MvcHtmlString(div.ToString());
        }
    }
}
