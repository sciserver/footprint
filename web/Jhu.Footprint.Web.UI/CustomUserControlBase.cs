namespace Jhu.Footprint.Web.UI
{
    public class CustomUserControlBase : Jhu.Graywulf.Web.UI.UserControlBase
    {
        public new CustomPageBase Page
        {
            get { return (CustomPageBase)base.Page; }
        }
    }
}