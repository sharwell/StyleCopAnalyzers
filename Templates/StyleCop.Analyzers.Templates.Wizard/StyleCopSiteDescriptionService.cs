namespace StyleCop.Analyzers.Templates.Wizard
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using HtmlAgilityPack;

    public class StyleCopSiteDescriptionService : IDescriptionService
    {
        private const string baseUrl = @"http://www.stylecop.com/docs/SA{0:0.#}.html";

        public async Task<PageInfo> GetPageInfo(decimal saId)
        {
            HtmlDocument document = new HtmlDocument();
            PageInfo pageInfo = new PageInfo();

            string page = string.Format(baseUrl, saId);
            pageInfo.Link = page;

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    document.Load(await content.ReadAsStreamAsync());
                }
                else
                {
                    throw new RuleNotFoundException();
                }
            }

            HtmlNode mainbody = document.GetElementbyId("mainbody");

            pageInfo.TypeName = mainbody.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/table[1]/tr[1]/td[2]").InnerText;
            pageInfo.CheckId = mainbody.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/table[1]/tr[2]/td[2]").InnerText;
            pageInfo.Category = mainbody.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/table[1]/tr[3]/td[2]").InnerText;

            bool cause = false;
            bool ruleDescription = false;
            bool inExample = false;

            foreach (var item in mainbody.ChildNodes)
            {
                if (item.Name.ToUpper() == "H2")
                {
                    switch (item.InnerText)
                    {
                        case "Cause":
                            cause = true;
                            ruleDescription = false;
                            break;
                        case "Rule Description":
                            cause = false;
                            ruleDescription = true;
                            break;
                        default:
                            cause = false;
                            ruleDescription = false;
                            break;
                    }
                }

                if (item.Name.ToUpper() == "P")
                {
                    if (item.Attributes.Contains("class") && item.Attributes["class"].Value == "MsoNormal")
                    {
                        if (!inExample)
                        {
                            pageInfo.RuleDescription.Add(new Line("<code>"));
                            inExample = true;
                        }

                        pageInfo.RuleDescription.Add(new Line(WebUtility.HtmlDecode(item.InnerText)));

                        continue;
                    }

                    if (inExample)
                    {
                        pageInfo.RuleDescription.Add(new Line("</code>"));
                        inExample = false;
                    }

                    if (cause)
                    {
                        pageInfo.Cause.Add(new Line(item.InnerHtml));
                    }

                    if (ruleDescription)
                    {
                        pageInfo.RuleDescription.Add(new Line(item.InnerHtml, true));
                    }
                }
            }

            return pageInfo;
        }

        public async Task<bool> RuleExists(decimal saId)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format(baseUrl, saId)))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
