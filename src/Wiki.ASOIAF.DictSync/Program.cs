namespace Wiki.ASOIAF.DictSync
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Common.Logging;

    using LinqToWiki.Generated;

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
                        .ToEnumerable().Take(3);
                var list = new List<KeyValuePair<string, string>>();
                foreach (var result in results)
                {
                    try
                    {
                        var lang = result.Data.FirstOrDefault();
                        if (lang == null)
                        {
                            Log.Warn(string.Format("Noen {0}", result.Info.title));
                            continue;
                        }

                        list.Add(new KeyValuePair<string, string>(result.Info.title, lang.value));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error occurred", ex);
                    }
                }

                using (var sw = new StreamWriter(@"C:\Users\lianzhao\Documents\GitHub\ASOIAFWikiTranslater\dict\词条.txt"))
                {
                    foreach (var kvp in list)
                    {
                        sw.WriteLine("{0}#{1}", kvp.Key, kvp.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("Error occurred", ex);
            }
        }
    }
}
