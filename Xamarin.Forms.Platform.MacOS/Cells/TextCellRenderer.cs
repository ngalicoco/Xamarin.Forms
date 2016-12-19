﻿using System.ComponentModel;
using AppKit;

namespace Xamarin.Forms.Platform.MacOS
{
	public class TextCellRenderer : CellRenderer
	{
		static readonly Color defaultDetailColor = new Color(.32, .4, .57);
		static readonly Color defaultTextColor = Color.Black;

		public override NSView GetCell(Cell item, NSView reusableView, NSTableView tv)
		{
			var textCell = (TextCell)item;

			var tvc = reusableView as CellNSView;
			if (tvc == null)
				tvc = new CellNSView(NSTableViewCellStyle.Subtitle);

			if (tvc.Cell != null)
				tvc.Cell.PropertyChanged -= tvc.HandlePropertyChanged;

			tvc.Cell = textCell;
			textCell.PropertyChanged += tvc.HandlePropertyChanged;
			tvc.PropertyChanged = HandlePropertyChanged;

			tvc.TextLabel.StringValue = textCell.Text ?? "";
			tvc.DetailTextLabel.StringValue = textCell.Detail ?? "";
			tvc.TextLabel.TextColor = textCell.TextColor.ToNSColor(defaultTextColor);
			tvc.DetailTextLabel.TextColor = textCell.DetailColor.ToNSColor(defaultDetailColor);

			WireUpForceUpdateSizeRequested(item, tvc, tv);

			UpdateIsEnabled(tvc, textCell);

			UpdateBackground(tvc, item);

			return tvc;
		}

		protected virtual void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			var tvc = (CellNSView)sender;
			var textCell = (TextCell)tvc.Cell;
			if (args.PropertyName == TextCell.TextProperty.PropertyName)
			{
				tvc.TextLabel.StringValue = ((TextCell)tvc.Cell).Text;
				tvc.TextLabel.SizeToFit();
			}
			else if (args.PropertyName == TextCell.DetailProperty.PropertyName)
			{
				tvc.DetailTextLabel.StringValue = ((TextCell)tvc.Cell).Detail;
				tvc.DetailTextLabel.SizeToFit();
			}
			else if (args.PropertyName == TextCell.TextColorProperty.PropertyName)
				tvc.TextLabel.TextColor = textCell.TextColor.ToNSColor(defaultTextColor);
			else if (args.PropertyName == TextCell.DetailColorProperty.PropertyName)
				tvc.DetailTextLabel.TextColor = textCell.DetailColor.ToNSColor(defaultTextColor);
			else if (args.PropertyName == Cell.IsEnabledProperty.PropertyName)
				UpdateIsEnabled(tvc, textCell);
		}

		static void UpdateIsEnabled(CellNSView cell, TextCell entryCell)
		{
			cell.TextLabel.Enabled = entryCell.IsEnabled;
			cell.DetailTextLabel.Enabled = entryCell.IsEnabled;
		}
	}
}
