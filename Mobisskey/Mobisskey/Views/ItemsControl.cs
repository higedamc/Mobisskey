using System;
using System.Collections;

using Xamarin.Forms;

namespace Mobisskey.Views
{
	// Referenced and modified this code from https://stackoverflow.com/questions/37240486/how-to-create-a-simple-xamarin-forms-items-view
	public class ItemsControl : ScrollView
	{
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
			nameof(ItemTemplate),
			typeof(DataTemplate),
			typeof(ItemsControl),
			null,
			propertyChanged: (bindable, value, newValue) => Populate(bindable));

		public static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(
			nameof(ItemsLayout),
			typeof(Layout<View>),
			typeof(ItemsControl),
			null,
			propertyChanged: (bindable, value, newValue) => Populate(bindable));

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
			nameof(ItemsSource),
			typeof(IEnumerable),
			typeof(ItemsControl),
			null,
			BindingMode.OneWay,
			propertyChanged: (bindable, value, newValue) => Populate(bindable));

		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public Layout<View> ItemsLayout
		{
			get
			{
				return (Layout<View>)GetValue(ItemsLayoutProperty);
			}
			set
			{
				SetValue(ItemsLayoutProperty, value);
			}
		}

		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(ItemTemplateProperty);
			}
			set
			{
				SetValue(ItemTemplateProperty, value);
			}
		}

		private static void Populate(BindableObject bindable)
		{
			var repeater = (ItemsControl)bindable;

			// Clean
			repeater.Content = null;

			// Only populate once both properties are recieved
			if (repeater.ItemsSource == null || repeater.ItemTemplate == null)
			{
				return;
			}

			// Create a stack to populate with items
			var list = repeater.ItemsLayout;
			list.Children.Clear();

			foreach (var viewModel in repeater.ItemsSource)
			{
				var content = repeater.ItemTemplate.CreateContent();
				if (!(content is View) && !(content is ViewCell))
				{
					throw new Exception($"Invalid visual object {nameof(content)}");
				}

				var view = content is View ? content as View : ((ViewCell)content).View;
				view.BindingContext = viewModel;

				list.Children.Add(view);
			}

			// Set stack as conent to this ScrollView
			repeater.Content = list;
		}
	}
}