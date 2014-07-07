namespace Wiki.ASOIAF.DictSync
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
                        .ToEnumerable();
                var list = new List<string>();
                foreach (var result in results)
                {
                    try
                    {
                        var lang = result.Data.FirstOrDefault();
                        if (lang == null || string.IsNullOrEmpty(lang.value))
                        {
                            Log.Warn(string.Format("Noen {0}", result.Info.title));
                            continue;
                        }

                        var entry = string.Format("{0}#{1}", lang.value, result.Info.title);
                        list.Add(entry);
                        Log.Debug(string.Format("Entry :{0}", entry));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error occurred", ex);
                    }
                }

                using (var sw = new StreamWriter(@"..\..\..\..\dict\词条.txt"))
                {
                    sw.WriteLine("#WARNING: THIS FILE IS AUTO GENERATED. PLEASE DO NOT MODIFY.");
                    sw.WriteLine("#GENERATED DATE: " + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
                    foreach (var entry in list)
                    {
                        sw.WriteLine(entry);
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
