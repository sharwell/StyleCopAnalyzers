using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace StyleCop.Analyzers.Templates.Wizard
{
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
                document.Load(await content.ReadAsStreamAsync());
            }

            HtmlNode mainbody = document.GetElementbyId("mainbody");

            pageInfo.TypeName = mainbody.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/table[1]/tr[1]/td[2]").InnerText;
            pageInfo.CheckId = mainbody.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/table[1]/tr[2]/td[2]").InnerText;
            pageInfo.Category = mainbody.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/table[1]/tr[3]/td[2]").InnerText;

            bool cause = false;
            bool ruleDescription = false;

            StringBuilder causeText = new StringBuilder();
            StringBuilder ruleDescriptionText = new StringBuilder();
            StringBuilder exampleText = new StringBuilder();

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
                        exampleText.AppendLine(WebUtility.HtmlDecode(item.InnerText));

                        continue;
                    }

                    if (cause)
                    {
                        causeText.AppendLine(item.InnerHtml);
                    }

                    if (ruleDescription)
                    {
                        ruleDescriptionText.AppendLine(item.InnerHtml);
                    }
                }
            }

            pageInfo.Cause = causeText.ToString();
            pageInfo.RuleDescription = ruleDescriptionText.ToString();
            pageInfo.Examples = exampleText.ToString();

            return pageInfo;
        }
    }
}
