using System.Web;
using System.Web.Optimization;

namespace eAttendance
{
    public class BundleConfig
    {




        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js", new IItemTransform[0]));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*", new IItemTransform[0]));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js", new IItemTransform[0]));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(new string[] { 
            "~/Scripts/plugins/metisMenu/jquery.metisMenu.js", "~/Scripts/peity/jquery.peity.min.js", "~/Scripts/demo/peity.demo.js", "~/Scripts/jquery.flexdatalist.min.js", "~/Scripts/plugins/dataTables/jquery.dataTables.js", "~/Scripts/plugins/dataTables/dataTables.bootstrap.js", "~/Scripts/plugins/dataTables/dataTables.responsive.js", "~/Scripts/plugins/dataTables/dataTables.tableTools.min.js", "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js", "~/Scripts/inspinia.js", "~/Scripts/plugins/iCheck/icheck.min.js", "~/Scripts/plugins/steps/jquery.steps.min.js", "~/Scripts/respond.js", "~/Scripts/plugins/toastr/toastr.min.js", "~/Scripts/plugins/validate/jquery.validate.min.js", "~/Scripts/plugins/jasny/jasny-bootstrap.min.js",
            "~/Scripts/plugins/chosen/chosen.jquery.js", "~/Scripts/jQuery.print.js"
        }));
            bundles.Add(new ScriptBundle("~/bundles/float").Include(new string[] { "~/Scripts/plugins/flot/jquery.flot.js", "~/Scripts/plugins/flot/jquery.flot.tooltip.min.js", "~/Scripts/plugins/flot/jquery.flot.spline.js", "~/Scripts/plugins/flot/jquery.flot.resize.js", "~/Scripts/plugins/flot/jquery.flot.pie.js", "~/Scripts/plugins/flot/jquery.flot.symbol.js", "~/Scripts/plugins/flot/jquery.flot.time.js", "~/Scripts/plugins/flot/curvedLines.js" }));
            bundles.Add(new ScriptBundle("~/bundles/highcharts").Include("~/Scripts/Highcharts-4.0.1/js/highcharts.js", new IItemTransform[0]));
            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(new string[] { "~/Scripts/plugins/dropzone/dropzone-amd-module.js", "~/Scripts/plugins/dropzone/dropzone.js" }));
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include("~/Scripts/plugins/nepalidatepicker/nepali.datepicker.min.js", new IItemTransform[0]));
            bundles.Add(new ScriptBundle("~/bundles/DashboardChart").Include("~/Scripts/plugins/chartJs/Chart.min.js", new IItemTransform[0]));
            bundles.Add(new StyleBundle("~/Content/css").Include(new string[] { 
            "~/Content/bootstrap.min.css", "~/Content/plugins/jQueryUI/jquery-ui-1.10.4.custom.min.css", "~/content/font-awesome.min.css", "~/Content/jquery.flexdatalist.min.css", "~/Content/plugins/dataTables/dataTables.bootstrap.css", "~/Content/plugins/dataTables/dataTables.responsive.css", "~/Content/plugins/dataTables/dataTables.tableTools.min.css", "~/Content/plugins/iCheck/custom.css", "~/Content/plugins/steps/jquery.steps.css", "~/Content/plugins/toastr/toastr.min.css", "~/Content/plugins/chosen/chosen.css", "~/Content/plugins/jasny/jasny-bootstrap.min.css", "~/Content/plugins/dropzone/basic.css", "~/Content/plugins/dropzone/dropzone.css", "~/Content/animate.css", "~/Content/style.css",
            "~/Content/site.css"
        }));
            bundles.Add(new StyleBundle("~/Content/nepalidatepicker").Include("~/Content/plugins/nepalidatepicker/nepali.datepicker.css", new IItemTransform[0]));
        }
    }
}

