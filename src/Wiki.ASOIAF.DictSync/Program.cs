namespace Wiki.ASOIAF.DictSync
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Common.Logging;

    using LinqToWiki.Generated;

    using Newtonsoft.Json;

    class Program
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                var wiki = new Wiki("Wiki.ASOIAF.DictSync", "zh.asoiaf.wikia.com", "/api.php");
                var allPagesources =
                    wiki.Query.allpages().Where(p => p.filterredir == allpagesfilterredir.nonredirects).Pages;
                var results =
                    allPagesources.Select(
                        p => PageResult.Create(p.info, p.langlinks().Where(l => l.lang == "en").ToEnumerable()))
                        .ToEnumerable();
                var dict = results.Select(result => new { Lang = result.Data.FirstOrDefault(), Result = result })
                    .Where(
                        item =>
                            {
                                if (item.Lang == null || string.IsNullOrEmpty(item.Lang.value))
                                {
                                    Log.Warn(string.Format("Noen {0}", item.Result.Info.title));
                                    return false;
                                }

                                return true;
                            })
                    .GroupBy(item => item.Lang.value)
                    .Select(
                        group =>
                            {
                                if (group.Count() > 1)
                                {
                                    Log.Warn(
                                        string.Format(
                                            "Duplicated item, en:{0}, ch:{1}",
                                            group.Key,
                                            string.Join(",", group.Select(item => item.Result.Info.title))));
                                }

                                return group.First();
                            })
                    .ToDictionary(item => item.Lang.value, item => item.Result.Info.title);
                var json = JsonConvert.SerializeObject(dict);
                using (var sw = new StreamWriter(@"..\..\..\..\dict\entry.json"))
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("Error occurred", ex);
            }
        }
    }
}
