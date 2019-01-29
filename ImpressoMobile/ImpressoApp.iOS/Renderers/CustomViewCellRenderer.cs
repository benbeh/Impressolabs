using System;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as CustomViewCell;

            if (view.SelectedBackgroundColor != Color.Transparent)
            {
                cell.SelectedBackgroundView = new UIView
                {
                    BackgroundColor = view.SelectedBackgroundColor.ToUIColor(),
                };
            }
            else
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }

            return cell;
        }
    }
}
