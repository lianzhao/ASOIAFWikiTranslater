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

        private static readonly IDictionary<string, string> EntryFormats = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            try
            {
                EntryFormats.Add("[[{0}]]", "[[{0}]]");
                EntryFormats.Add(" {0} ", " {0} ");
                EntryFormats.Add(" {0}.", " {0}。");
                EntryFormats.Add(" {0},", " {0}，");

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

                        var entries = GetEntries(lang.value, result.Info.title);
                        list.AddRange(entries);
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

        private static IEnumerable<string> GetEntries(string en, string ch)
        {
            return EntryFormats.Select(
                kvp =>
                    {
                        var entry = string.Format("{0}#{1}", string.Format(kvp.Key, en), string.Format(kvp.Value, ch));
                        Log.Debug(string.Format("Entry :{0}", entry));
                        return entry;
                    });
        }
    }
}
