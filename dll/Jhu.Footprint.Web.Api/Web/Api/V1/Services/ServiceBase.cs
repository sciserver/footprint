using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    public class ServiceBase : RestServiceBase
    {
        #region Context creators

        protected Lib.FootprintContext CreateContext()
        {
            return CreateContext(false);
        }

        protected Lib.FootprintContext CreateContext(bool autoDispose)
        {
            var context = new Lib.FootprintContext()
            {
                AutoDispose = autoDispose
            };

            if (System.Threading.Thread.CurrentPrincipal is Jhu.Graywulf.AccessControl.Principal)
            {
                context.Principal = (Jhu.Graywulf.AccessControl.Principal)System.Threading.Thread.CurrentPrincipal;
            }
            else
            {
                context.Principal = Jhu.Graywulf.AccessControl.Principal.Guest;
            }

            return context;
        }

        #endregion

        protected IEnumerable<string> GetMatchingKeys(IEnumerable<string> keys, string pattern, int? from, int? max, out bool hasBefore, out bool hasAfter)
        {
            if (pattern != null && pattern.Contains(','))
            {
                var res = pattern.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                hasBefore = hasAfter = false;
                return res;
            }
            else
            {
                var res = PatternSearch(keys, pattern, from, max, out hasBefore, out hasAfter);
                return res;
            }
        }

        protected IEnumerable<string> PatternSearch(IEnumerable<string> keys, string pattern, int? from, int? max, out bool hasBefore, out bool hasAfter)
        {
            Regex rex = null;

            if (!String.IsNullOrWhiteSpace(pattern) && pattern != "*")
            {
                rex = Jhu.Graywulf.Util.WildCardSearch.GetRegex(pattern);
            }

            var res = new List<string>();
            int found = 0;

            hasBefore = hasAfter = false;

            foreach (var key in keys)
            {
                if (rex == null || rex.IsMatch(key))
                {
                    found++;

                    if (from.HasValue && from.Value < found)
                    {
                        hasBefore = true;
                    }

                    if ((!from.HasValue || from.Value < found) &&
                        (!max.HasValue || res.Count < max.Value))
                    {
                        res.Add(key);
                    }

                    if (max.HasValue && res.Count < found)
                    {
                        hasAfter = true;
                    }
                }
            }

            return res;
        }
    }
}
